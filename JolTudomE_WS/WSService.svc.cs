using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
using JolTudomE_WS.DAL;
using System.Data;
using JolTudomE_WS.Security;
using System;
using JolTudomE_WS.Model;
using System.Data.SqlClient;

namespace JolTudomE_WS {
  public class WSService : IWSService {

    public string Login(string username, string password, out PersonDetails loggedinuser) {

      string token = string.Empty;

      try {
        JolTudomEEntities ent = new JolTudomEEntities();
        var loggedin = ent.usp_Authenticate(username, password);
        loggedinuser = loggedin.First();

        SessionManager sm = SessionManager.NewSession(loggedinuser);
        return sm.Session.Token;
      }
      catch (EntityCommandExecutionException) {
        // send only a general error message
        throw new Exception("Hibás felhasználónév vagy jelszó!");
      }

    }

    public void Logout(string token) {
      SessionManager sm = new SessionManager(token);
      sm.DeleteSession();
    }

    public List<Statistics> GetStatistics(string token, int? personid) {
      SessionManager sm = new SessionManager(token);

      JolTudomEEntities ent = new JolTudomEEntities();
      List<Statistics> statofperson = new List<Statistics>();

      var statistics = ent.usp_Statistics(personid ?? sm.Session.PersonID, sm.Session.PersonID, sm.Session.RoleID);
      statofperson = statistics.ToList();
      sm.UpdateSessionLastAction();

      return statofperson;
    }

    public List<TestDetails> GetTestDetails(string token, int testid, int? personid) {
      SessionManager sm = new SessionManager(token);

      JolTudomEEntities ent = new JolTudomEEntities();
      List<TestDetails> testdet = new List<TestDetails>();

      var details = ent.usp_Eval(testid, personid ?? sm.Session.PersonID, sm.Session.PersonID, sm.Session.RoleID);
      testdet = details.ToList();
      sm.UpdateSessionLastAction();

      return testdet;
    }

    /// <summary>
    /// we must provide the personid here, though in a normal case we could get that from session
    /// but if the session is over we don't have that
    /// </summary>
    /// <param name="token"></param>
    /// <param name="testid"></param>
    /// <param name="personid"></param>
    public void CancelTest(string token, int testid, int personid) {
      SessionManager sm;
      try {
        sm = new SessionManager(token);
      }
      catch (SessionNotAvailable) {
        // this is a special case that in the meantime the session is timed out
        // but the started test must be cancelled
        using (JolTudomEEntities ents = new JolTudomEEntities()) {
          ents.usp_CancelTest(testid, personid);
        }
        // and throw the exception to the client
        throw;
      }

      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        ent.usp_CancelTest(testid, personid);
        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }
    }

    public List<Courses> GetCourses(string token) {
      SessionManager sm = new SessionManager(token);

      List<Courses> courselist = new List<Courses>();
      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        var courses = ent.usp_GetCourses();
        courselist = courses.ToList();

        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }

      return courselist;
    }

    public List<Topics> GetTopics(string token, int courseid) {
      SessionManager sm = new SessionManager(token);

      List<Topics> topiclist = new List<Topics>();
      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        var topics = ent.usp_GetTopics(courseid);
        topiclist = topics.ToList();

        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }

      return topiclist;
    }

    public void NewUser(string token, string username, string prefix, string lastname, string middlename, string firstname, string password, int roleid) {

      JolTudomERoles newuserrole = (JolTudomERoles)roleid;

      // if token is available, it means only administrator can create a new user
      if (!string.IsNullOrEmpty(token)) {
        SessionManager sm = new SessionManager(token);
        if (sm.Session.UserRole != JolTudomERoles.Admin) {
          throw new ApplicationException("Csak Adminisztrátor hozhat létre új felhasználót!");
        }
      }
      // if token is null, it means the new user will be student
      else {
        // force roleid to be a Student
        newuserrole = JolTudomERoles.Student;
      }

      // the rest of the validation will be performed in backend
      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        ent.usp_AddNewUser(username, prefix, lastname, middlename, firstname, password, (byte)newuserrole);
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }

    }

    public NewTest StartTest(string token, int count, List<int> topicids) {
      SessionManager sm = new SessionManager(token);

      DataTable dtTopics = new DataTable();
      dtTopics.Columns.Add("TID", typeof(int));
      foreach (int i in topicids) {
        var r = dtTopics.NewRow();
        r[0] = i;
        dtTopics.Rows.Add(r);
      }

      SqlParameter ptopicid = new SqlParameter("@topicid", dtTopics);
      ptopicid.SqlDbType = SqlDbType.Structured;
      ptopicid.TypeName = "dbo.TopicIDs";

      SqlParameter pcount = new SqlParameter("@count", count);
      SqlParameter ppersonid = new SqlParameter("@personid", sm.Session.PersonID);

      string query = "[test].[usp_NewTest] @count, @topicid, @personid";

      string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["JolTudomESqlConnectionString"].ConnectionString;
      
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

      sm.UpdateSessionLastAction();

      return newtest;
    }

    /// <summary>
    /// we must provide the personid here, though in a normal case we could get that from session
    /// but if the session is over we don't have that, 
    /// but we have to call the canceltest - which needs that parameter as well
    /// </summary>
    /// <param name="token"></param>
    /// <param name="personid"></param>
    /// <param name="testid"></param>
    /// <param name="questid"></param>
    /// <param name="answid"></param>
    /// <param name="iscomplete"></param>
    public void CheckAnswer(string token, int personid, int testid, int questid, int? answid, bool iscomplete) {
      SessionManager sm;
      try {
        sm = new SessionManager(token);
      }
      catch (SessionNotAvailable) {
        // this is a special case that in the meantime the session is timed out
        // but the started test must be cancelled
        using (JolTudomEEntities ents = new JolTudomEEntities()) {
          ents.usp_CancelTest(testid, personid);
        }
        // and throw the exception to the client
        throw;
      }

      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        ent.usp_CheckedAnswer(testid, questid, answid ?? null, iscomplete);

        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }

    }

    public List<PersonDetails> GetUsers(string token, int? searchroleid) {
      SessionManager sm = new SessionManager(token);

      JolTudomEEntities ent = new JolTudomEEntities();
      List<PersonDetails> plist = new List<PersonDetails>();
      try {
        var persons = ent.usp_GetAllUsers(sm.Session.RoleID, searchroleid ?? null);
        plist = persons.ToList();

        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }

      return plist;
    }

    public void AddNewCourse(string token, string name, string description) {
      SessionManager sm = new SessionManager(token);

      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        ent.usp_AddNewCourse(name, description);
        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }
    }

    public void EditCourse(string token, int courseid, string name, string description) {
      SessionManager sm = new SessionManager(token);

      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        ent.usp_EditCourse(name, description, courseid);
        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }

    }

    public void AddNewTopic(string token, int courseid, string name, string description) {
      SessionManager sm = new SessionManager(token);

      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        ent.usp_AddNewTopic(name, description, courseid);
        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }
    }

    public void EditTopic(string token, int courseid, int topicid, string name, string description) {
      SessionManager sm = new SessionManager(token);

      JolTudomEEntities ent = new JolTudomEEntities();
      try {
        ent.usp_EditTopic(name, description, topicid, courseid);
        sm.UpdateSessionLastAction();
      }
      catch (EntityCommandExecutionException ece_exc) {
        throw ece_exc.InnerException;
      }

    }

  }
}
