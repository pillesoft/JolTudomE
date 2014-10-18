using JolTudomE_Api.Models;
using JolTudomE_Api.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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
    public LoginResponse Login() {
      var id = (CustomIdentity)User.Identity;

      return new LoginResponse { PersonID = id.PersonID, RoleID = id.RoleID };
    }

    [Route("detail")]
    [HttpGet]
    public UserDetail GetUserInfo() {
      var id = (CustomIdentity)User.Identity;

      var p = DBContext.Person.Find(id.PersonID);
      var newperson = new UserDetail {
        FirstName = p.FirstName,
        LastName = p.LastName,
        MiddleName = p.MiddleName,
        PersonID = p.PersonID,
        Prefix = p.Prefix,
        UserName = p.UserName, 
        RoleID = id.RoleID,
        Password = string.Empty
      };
      UpdateSession();

      return newperson;
    }

    [Route("searchbyrole/{roleid:int?}")]
    [HttpGet]
    public IEnumerable<usp_GetAllUsers_Result> GetUsers(int? roleid = null) {
      var id = (CustomIdentity)User.Identity;
      var users = DBContext.usp_GetAllUsers(id.RoleID, roleid);
      UpdateSession();

      return users;
    }

    [Route("addstudent")]
    [HttpPost]
    [AllowAnonymous]
    public HttpResponseMessage AddStudent(UserDetail student) {
      if (this.ModelState.IsValid) {
        try {
          // roleid = 1 ---> student
          DBContext.usp_AddNewUser(student.UserName, student.Prefix, student.LastName, student.MiddleName, student.FirstName, student.Password, 1);
          var response = Request.CreateResponse(HttpStatusCode.Created);
          return response;
        }
        catch (EntityCommandExecutionException ece_exc) {
          throw ece_exc.InnerException;
        }
      }
      else {
        return Request.CreateResponse(HttpStatusCode.BadRequest, this.ModelState);
      }
    }

  }
}
