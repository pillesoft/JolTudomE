using System;
using System.ServiceModel;
using JolTudomE_Client.Helper;
using JolTudomE_Client.JolTudomEWSService;
using GalaSoft.MvvmLight.Messaging;
using JolTudomE_Client.Message;
using System.Collections.Generic;
using JolTudomE_Client.Model;
using System.Linq;

namespace JolTudomE_Client {

  public class TranslatedException : ApplicationException {
    public TranslatedException(string message):base(message) { }
  }

  public class SessionExpiredException : ApplicationException { }

  public class WSManager {
    private string _Token;
    public PersonDetails LoggedInUser { get; private set; }

    public bool Login(string username, string password, out string ErrorMess) {
      
      ErrorMess = string.Empty;

      using (JolTudomEWSClient WSClient = new JolTudomEWSClient()) {
        try {
          PersonDetails pd = new PersonDetails();
          _Token = WSClient.Login(out pd, username, password);
          LoggedInUser = pd;

          Messenger.Default.Send<AfterLoginLogoutMessage>(new AfterLoginLogoutMessage());

          Messenger.Default.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Student });

        }
        catch (FaultException fe) {
          ErrorMess = fe.Message;
        }
      }
      return ErrorMess == string.Empty;
    }

    /// <summary>
    /// makes logout, and navigates to Login View
    /// </summary>
    public void Logout() {
      using (JolTudomEWSClient WSClient = new JolTudomEWSClient()) {

        try {
          WSClient.Logout(_Token);
          _Token = string.Empty;
          LoggedInUser = null;

          Messenger.Default.Send<AfterLoginLogoutMessage>(new AfterLoginLogoutMessage());
          Messenger.Default.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login });

        }
        catch (FaultException<ExceptionDetail> exc) {
          // this is OK, the session is expired, or not available
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            Messenger.Default.Send<AfterLoginLogoutMessage>(new AfterLoginLogoutMessage());
            Messenger.Default.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
          }
          else
            // otherwise throw the exception
            throw;
        }
      }
    }

    /// <summary>
    /// makes logout
    /// normally this is called from window (application) close
    /// </summary>
    public void ClearSession() {
      using (JolTudomEWSClient WSClient = new JolTudomEWSClient()) {

        try {
          if (!string.IsNullOrEmpty(_Token)) {
            WSClient.Logout(_Token);

            _Token = string.Empty;
            LoggedInUser = null;
          }
        }
        catch (FaultException<ExceptionDetail> exc) {
          // this is OK, the session is expired, or not available
          if (!ExceptionHandler.IsSessionNotAvailableException(exc)) {
            // otherwise throw the exception
            throw;
          }
        }
      }
    }

    public List<Courses> GetCourses() {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          return client.GetCourses(_Token);
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else
            throw;
        }
      }
    }

    public List<Topics> GetTopics(int courseid) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          return client.GetTopics(_Token, courseid);
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else
            throw;
        }
      }
    }

    public IEnumerable<Statistics> GetStatistics(int? personid) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          var result = client.GetStatistics(_Token, personid);
          return result.OrderByDescending(s => s.Generated);
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else
            throw;
        }
      }
    }

    public List<TestDetails> GetTestDetails(int testid, int? personid) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          return client.GetTestDetails(_Token, testid, personid);
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else
            throw;
        }
      }
    }

    public List<PersonRole> GetUsers(int? roletosearch) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {

        try {
          var userlist = client.GetUsers(_Token, roletosearch ?? null);

          List<PersonRole> rolelist = new List<PersonRole>();
          var ownperson = LoggedInUser;
          ownperson.IsSelected = true;

          rolelist.Add(new PersonRole { RoleName = "Saját", IsExpanded = true, Children = new List<PersonDetails>() { ownperson } });
          if (roletosearch == null || roletosearch == 1) {
            rolelist.Add(new PersonRole {
              RoleName = "Diák",
              Children = userlist.Where(p => p.RoleEnum == PersonRoleEnum.Student).Select(p => p).ToList()
            });
          }

          if (roletosearch == null) {
            var teacherlist = userlist.Where(p => p.RoleEnum == PersonRoleEnum.Teacher).Select(p => p);
            if (teacherlist.Count() > 0) {
              rolelist.Add(new PersonRole {
                RoleName = "Tanár",
                Children = teacherlist.ToList()
              });
            }

            // need to exclude the Admin, because it is visible in the Sajat category
            var adminlist = userlist.Where(p => p.RoleEnum == PersonRoleEnum.Admin && p.PersonID != LoggedInUser.PersonID).Select(p => p);
            if (adminlist.Count() > 0) {
              rolelist.Add(new PersonRole {
                RoleName = "Admin",
                Children = adminlist.ToList()
              });
            }
          }

          return rolelist;
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else
            throw;
        }
      }

    }

    public NewTest GetNewTest(int count, List<int> topicid) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {

        try {
          var result = client.StartTest(_Token, count, topicid);
          App.Current.Properties["CurrentTestID"] = result.TestID;

          return result;
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else if (ExceptionHandler.IsSqlException(exc)) {
            throw new TranslatedException(ExceptionHandler.GetUserFriendlyErrorMessage(exc));
          }
          else
            throw;
        }
      }

    }

    public void CancelTest(int testid, int personid) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {

        try {
          client.CancelTest(_Token, testid, personid);
          App.Current.Properties["CurrentTestID"] = null;

          Messenger.Default.Send<TestExecutionRunningMessage>(new TestExecutionRunningMessage { IsTestRunning = false });

          Messenger.Default.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Student });
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else
            throw;
        }
      }
    }

    public void CheckAnswer(int personid, int testid, int questionid, int? answerid, bool iscomplete) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {

        try {
          client.CheckAnswer(_Token, personid, testid, questionid, answerid, iscomplete);
          if (iscomplete) {
            App.Current.Properties["CurrentTestID"] = null;

            Messenger.Default.Send<TestExecutionRunningMessage>(new TestExecutionRunningMessage { IsTestRunning = false });
            
            Messenger.Default.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Student });
          }
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else
            throw;
        }
      }
    }

    public void RegisterPerson(NewPerson personinst) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          client.NewUser(_Token, personinst.UserName, personinst.Prefix, personinst.LastName, personinst.MiddleName, personinst.FirstName, personinst.Password, personinst.RoleId);
          if (_Token == null) {
            Messenger.Default.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login });
          }
          else {
            Messenger.Default.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Admin, UserState="NewPerson" });
          }
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else if (ExceptionHandler.IsSqlException(exc)) {
            throw new TranslatedException(ExceptionHandler.GetUserFriendlyErrorMessage(exc));
          }
          else
            throw;
        }
      }
    }

    public void EditCourse(int id, string name, string description) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          client.EditCourse(_Token, id, name, description);
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else if (ExceptionHandler.IsSqlException(exc)) {
            throw new TranslatedException(ExceptionHandler.GetUserFriendlyErrorMessage(exc));
          }
          else
            throw;
        }
      }
    }

    public void NewCourse(string name, string description) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          client.AddNewCourse(_Token, name, description);
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else if (ExceptionHandler.IsSqlException(exc)) {
            throw new TranslatedException(ExceptionHandler.GetUserFriendlyErrorMessage(exc));
          }
          else
            throw;
        }
      }
    }

    public void EditTopic(int courseid, int id, string name, string description) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          client.EditTopic(_Token, courseid, id, name, description);
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else if (ExceptionHandler.IsSqlException(exc)) {
            throw new TranslatedException(ExceptionHandler.GetUserFriendlyErrorMessage(exc));
          }
          else
            throw;
        }
      }
    }

    public void NewTopic(int courseid, string name, string description) {
      using (JolTudomEWSClient client = new JolTudomEWSClient()) {
        try {
          client.AddNewTopic(_Token, courseid, name, description);
        }
        catch (FaultException<ExceptionDetail> exc) {
          if (ExceptionHandler.IsSessionNotAvailableException(exc)) {
            throw new SessionExpiredException();
          }
          else if (ExceptionHandler.IsSqlException(exc)) {
            throw new TranslatedException(ExceptionHandler.GetUserFriendlyErrorMessage(exc));
          }
          else
            throw;
        }
      }
    }
  }
}
