using JolTudomE_Api.Models;
using JolTudomE_Api.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace JolTudomE_Api.Controllers {
  [RoutePrefix("api/account")]
  [Authorize]
  public class AccountController : BaseController {

    [Route("login")]
    [AllowAnonymous]
    public LoggedInUser Login() {
      var id = (CustomIdentity)User.Identity;

      return new LoggedInUser { PersonID = id.PersonID, RoleID = id.RoleID, UserName = id.Name, FullName = id.FullName };
    }

  }
}
