using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JolTudomE_WS.DAL;
using JolTudomE_WS.Properties;

namespace JolTudomE_WS.Security {

  public enum JolTudomERoles {
    Student = 1,
    Teacher = 2,
    Admin = 3
  }

  /// <summary>
  /// class to manage special exception, when there is any issue with a token
  /// this thrown when the session is expired, 
  /// if the session is not found - could be that it is over of the timeout, so removed
  /// </summary>
  public class SessionNotAvailable : ApplicationException { }

  public class SessionManager {
    private Session _Session;
    private string _Token;

    public Session Session {
      get { return _Session; }
    }

    public SessionManager(string token) {
      _Token = token;
      ValidateToken();
    }

    private void GetSession() {
      using (JolTudomEEntities ent = new JolTudomEEntities()) {
        _Session = ent.Sessions.FirstOrDefault(s => s.Token == _Token);
        if (_Session != null) {
          ent.Sessions.Detach(_Session);
        }
        else {
          throw new SessionNotAvailable();
        }
      }
    }

    private void ValidateToken() {
      GetSession();
      if (_Session.LastAction.AddMinutes(Settings.Default.SessionTimeoutMinute) < DateTime.UtcNow) {
        throw new SessionNotAvailable();
      }
    }

    public void UpdateSessionLastAction() {
      using (JolTudomEEntities ent = new JolTudomEEntities()) {
        ent.Attach(_Session);
        _Session.LastAction = DateTime.UtcNow;
        ent.SaveChanges();
      }
    }

    public void DeleteSession() {
      using (JolTudomEEntities ent = new JolTudomEEntities()) {
        ent.Attach(_Session);
        ent.Sessions.DeleteObject(_Session);
        ent.SaveChanges();
      }
    }

    public static SessionManager NewSession(PersonDetails loggedinuser) {
      // generate a token
      // this could be more secure ...
      byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
      byte[] key = Guid.NewGuid().ToByteArray();
      string token = Convert.ToBase64String(time.Concat(key).ToArray());

      using (JolTudomEEntities ent = new JolTudomEEntities()) {
        // delete those sessions, which are dead - over of the given timeout
        ent.usp_SessionsCleanup(Settings.Default.SessionTimeoutMinute);

        // delete those tests, which are not completed
        ent.usp_CleanupTests(Settings.Default.MaxTestExecutionHour);

        // this must be saved to the database with the timestamp
        ent.Sessions.AddObject(new Session { Token = token, PersonID = loggedinuser.PersonID, RoleID = loggedinuser.RoleID, LastAction = DateTime.UtcNow });
        ent.SaveChanges();
      }

      SessionManager sm = new SessionManager(token);
      return sm;
    }

  }
}