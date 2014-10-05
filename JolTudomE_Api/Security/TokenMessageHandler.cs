using JolTudomE_Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace JolTudomE_Api.Security {


  public class TokenMessageHandler : DelegatingHandler {
    private const string _AuthType = "token";
    private const string _CookieName = "JolTudomEToken";

    protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) {

      if (!CanHandleAuthentication(request)) {
        return await base.SendAsync(request, cancellationToken);
      }
      if (HttpContext.Current.User.Identity.IsAuthenticated) {
        return await base.SendAsync(request, cancellationToken);
      }
      bool isAuthenticated;
      try {
        isAuthenticated = Authenticate(request);
      }
      catch (Exception) {
        return CreateUnauthorizedResponse();
      }
      if (isAuthenticated) {
        var response = await base.SendAsync(request, cancellationToken);
        return response.StatusCode == HttpStatusCode.Unauthorized ? CreateUnauthorizedResponse() : response;
      }
      return CreateUnauthorizedResponse();
    }

    private bool Authenticate(HttpRequestMessage request) {
      var cookies = request.Headers.GetCookies(_CookieName);
      // cookies are checked at the CanHandleAuthentication
      // it is sure we have cookie
      return SetPrincipal(cookies.First().Cookies.First().Value);
    }

    private bool SetPrincipal(string token) {
      var user = ValidateToken(token);
      IPrincipal principal = null;
      if (user == null || (principal = GetPrincipal(user)) == null) {
        return false;
      }
      Thread.CurrentPrincipal = principal;
      if (HttpContext.Current != null) {
        HttpContext.Current.User = principal;
      }
      return true;
    }

    private IPrincipal GetPrincipal(LoggedInUser user) {
      var identity = new CustomIdentity(user.UserName, _AuthType, user.PersonID, user.RoleID, user.Token, user.FullName);
      return new CustomPrincipal(identity);
    }

    private LoggedInUser ValidateToken(string token) {
      try {
        SessionManager sm = new SessionManager(token);
        return new LoggedInUser {
          UserName = sm.Session.Person.UserName,
          PersonID = sm.Session.PersonID,
          RoleID = sm.Session.RoleID,
          Token = token,
          FullName = string.Format("{0} {1}", sm.Session.Person.FirstName, sm.Session.Person.LastName)
        };
      }
      catch (SessionNotAvailable) {
        throw;
      }
    }

    private bool CanHandleAuthentication(HttpRequestMessage request) {
      return (request.Headers != null && request.Headers.GetCookies(_CookieName).Count != 0);
    }

    private HttpResponseMessage CreateUnauthorizedResponse() {
      var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
      response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(_AuthType));
      return response;
    }
  }
}