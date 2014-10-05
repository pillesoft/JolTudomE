using JolTudomE_Api.Models;
using JolTudomE_Api.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JolTudomE_Api.Controllers {
  
  [RoutePrefix("api/test")]
  [Authorize]
  public class TestController : BaseController {

    [Route("statistic/{personid:int}")]
    public IEnumerable<usp_Statistics_Result> GetStatistics(int personid) {
      var id = (CustomIdentity)User.Identity;
      var statistics = DBContext.usp_Statistics(personid, id.PersonID, id.RoleID).OrderByDescending(s => s.Generated);
      UpdateSession();
      return statistics;
    }

    [Route("detail/{testid}/{personid:int}")]
    public IEnumerable<usp_Eval_Result> GetTestDetails(int testid, int personid) {
      var id = (CustomIdentity)User.Identity;

      var details = DBContext.usp_Eval(testid, personid, id.PersonID, id.RoleID);
      UpdateSession();

      return details;
    }

    [Route("start/{personid}/{count}/{topicids}")]
    [HttpGet]
    public NewTest StartTest(int personid, int count, string topicids) {

      // topicids needs to be parsed to List<int>
      List<int> inttopics = new List<int>();
      topicids.Split(',').ToList().ForEach(t => inttopics.Add(int.Parse(t)));

      DataTable dtTopics = new DataTable();
      dtTopics.Columns.Add("TID", typeof(int));
      foreach (int i in inttopics) {
        var r = dtTopics.NewRow();
        r[0] = i;
        dtTopics.Rows.Add(r);
      }

      SqlParameter ptopicid = new SqlParameter("@topicid", dtTopics);
      ptopicid.SqlDbType = SqlDbType.Structured;
      ptopicid.TypeName = "dbo.TopicIDs";

      SqlParameter pcount = new SqlParameter("@count", count);
      SqlParameter ppersonid = new SqlParameter("@personid", personid);

      string query = "[test].[usp_NewTest] @count, @topicid, @personid";

      string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["JolTudomEEntities"].ConnectionString;
      string searchterm = ";provider connection string=\"";
      connstring = connstring.Substring(connstring.IndexOf(searchterm)+searchterm.Length);
      connstring = connstring.Substring(0, connstring.IndexOf('"'));

      var conn = new SqlConnection(connstring);

      var cmd = conn.CreateCommand();
      cmd.CommandText = query;

      cmd.Parameters.Add(pcount);
      cmd.Parameters.Add(ptopicid);
      cmd.Parameters.Add(ppersonid);

      cmd.CommandTimeout = 700;
      SqlDataAdapter dap = new SqlDataAdapter(cmd);
      DataTable dtresult = new DataTable();
      bool closed = conn.State == System.Data.ConnectionState.Closed;
      if (closed) conn.Open();
      try {
        dap.Fill(dtresult);
      }
      finally {
        if (closed) conn.Close();
      }

      NewTest newtest = new NewTest();
      newtest.TestID = int.Parse(dtresult.Rows[0]["TestID"].ToString());
      newtest.PersonID = int.Parse(dtresult.Rows[0]["PersonID"].ToString());

      int questid = int.Parse(dtresult.Rows[0]["QuestionID"].ToString());
      string questtext = dtresult.Rows[0]["QuestionText"].ToString();

      List<NewTestQuestAnswer> questanswlist = new List<NewTestQuestAnswer>();

      foreach (DataRow drow in dtresult.Rows) {
        if (questid != int.Parse(drow["QuestionID"].ToString())) {

          var testq = new NewTestQuestion {
            QuestionID = questid,
            QuestionText = questtext,
            Answers = questanswlist
          };
          newtest.Questions.Add(testq);

          questid = int.Parse(drow["QuestionID"].ToString());
          questtext = drow["QuestionText"].ToString();

          questanswlist = new List<NewTestQuestAnswer>();
        }

        questanswlist.Add(new NewTestQuestAnswer {
          AnswerID = int.Parse(drow["AnswerID"].ToString()),
          AnswerText = drow["AnswerText"].ToString(),
        });

      }
      newtest.Questions.Add(new NewTestQuestion {
        QuestionID = questid,
        QuestionText = questtext,
        Answers = questanswlist
      });

      UpdateSession();

      return newtest;
    }

    [Route("answer/{testid}/{questionid}/{answerid}")]
    [HttpGet]
    public IHttpActionResult CheckTest(int testid, int questionid, int answerid) {
      var id = (CustomIdentity)User.Identity;

      DBContext.usp_CheckedAnswer(testid, questionid, answerid, false);
      UpdateSession();

      return Ok();
    }

    [Route("complete/{testid}/{questionid}/{answerid}")]
    [HttpGet]
    public IHttpActionResult CompleteTest(int testid, int questionid, int answerid) {
      var id = (CustomIdentity)User.Identity;

      DBContext.usp_CheckedAnswer(testid, questionid, answerid, true);
      UpdateSession();

      return Ok();
    }

    [Route("cancel/{testid}")]
    [HttpGet]
    public IHttpActionResult Cancel(int testid, int personid) {
      var id = (CustomIdentity)User.Identity;

      DBContext.usp_CancelTest(testid, personid);
      UpdateSession();

      return Ok();
    }

  }
}