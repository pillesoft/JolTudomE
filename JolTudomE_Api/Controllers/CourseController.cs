using JolTudomE_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JolTudomE_Api.Controllers {
  
  [Authorize]
  [RoutePrefix("api/course")]
  public class CourseController : BaseController {
    
    [Route("courses")]
    public IEnumerable<usp_GetCourses_Result> GetCourses() {
      var courses = DBContext.usp_GetCourses();
      UpdateSession();
      return courses;
    }

    [Route("topic/{courseid}")]
    public IEnumerable<usp_GetTopics_Result> GetTopics(int courseid) {
      var topics = DBContext.usp_GetTopics(courseid);
      UpdateSession();
      return topics;
    }

  }
}