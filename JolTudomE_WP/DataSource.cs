using JolTudomE_WP.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace JolTudomE_WP {
  sealed class DataSource {

    private static DataSource _DataSource = new DataSource();

    private WebAPIManager _WAM;
    private ObservableCollection<PersonRole> _Roles;
    private LoginResponse _LoggedInInfo;

    public DataSource() {
      _WAM = new WebAPIManager();

      _Roles = new ObservableCollection<PersonRole>();
      _Roles.Add(new PersonRole { RoleID = 1, Role = "Diák" });
      _Roles.Add(new PersonRole { RoleID = 2, Role = "Tanár" });
      _Roles.Add(new PersonRole { RoleID = 3, Role = "Admin" });
    }


    internal static LoginResponse LoggedInInfo {
      get{
        return _DataSource._LoggedInInfo;
      }
    }

    internal static ObservableCollection<PersonRole> GetRoles() {
      return _DataSource._Roles; 
    }

    internal static PersonRole GetRoleByName(string name) {
      return _DataSource._Roles.FirstOrDefault(r => r.Role == name);
    }
    internal static PersonRole GetRoleById(int id) {
      return _DataSource._Roles.FirstOrDefault(r => r.RoleID == id);
    }

    internal static UserDetail CreateUserStudent() {
      return new UserDetail { UserName = "", Prefix="", MiddleName="", Password = "", FirstName = "", LastName = "", Role = GetRoleByName("Diák") };
    }

    internal static bool IsAuthenticated() {
      return _DataSource._LoggedInInfo != null;
    }

    internal async static Task<bool> MakeLogin(string username, string password) {
      var loginresult = await _DataSource._WAM.Login(username, password);

      _DataSource._LoggedInInfo = JsonConvert.DeserializeObject<LoginResponse>(loginresult);

      return true;
    }

    internal async static Task<bool> MakeRegister(UserDetail newuser) {
      await _DataSource._WAM.Register(newuser);

      return true;
    }

    internal async static Task<UserDetail> GetLoginDetail() {
      var result = await _DataSource._WAM.GetLoginDetail();

      var userdet = JsonConvert.DeserializeObject<UserDetail>(result);
      //userdet.Role = DataSource.GetRoleById(_DataSource._LoggedInInfo.RoleID);
      return userdet;
    }

    internal async static Task<List<Statistic>> GetStatistic(int personid) {
      var result = await _DataSource._WAM.GetStatistics(personid);

      var statlist = JsonConvert.DeserializeObject<List<Statistic>>(result);
      return statlist;
    }

    internal async static Task<List<TestDetail>> GetTestDetail(int testid, int personid) {
      var result = await _DataSource._WAM.GetTestDetails(testid, personid);

      var testdetlist = JsonConvert.DeserializeObject<List<TestDetail>>(result);
      return testdetlist;
    }

    internal async static Task<ObservableCollection<Course>> GetCourses() {
      var result = await _DataSource._WAM.GetCourses();

      var courselist = JsonConvert.DeserializeObject<ObservableCollection<Course>>(result);
      return courselist;
    }

    internal async static Task<ObservableCollection<Topic>> GetTopics(int courseid) {
      var result = await _DataSource._WAM.GetTopics(courseid);

      var topiclist = JsonConvert.DeserializeObject<ObservableCollection<Topic>>(result);
      return topiclist;
    }

    internal async static Task<NewTest> GenerateTest(int personid, int count, List<int> topicids) {
      var result = await _DataSource._WAM.StartNewTest(personid, count, topicids);

      var newtest = JsonConvert.DeserializeObject<NewTest>(result);
      return newtest;
    }

    internal async static Task AnswerTest(int testid, int questionid, int answerid) {
      await _DataSource._WAM.AnswerTest(testid, questionid, answerid);
    }

    internal async static Task CompleteTest(int testid, int questionid, int answerid) {
      await _DataSource._WAM.CompleteTest(testid, questionid, answerid);
    }

    internal async static Task CancelTest(int testid, int personid) {
      await _DataSource._WAM.CancelTest(testid, personid);
    }

  }
}
