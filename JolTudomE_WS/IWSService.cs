using System.Collections.Generic;
using System.ServiceModel;
using JolTudomE_WS.DAL;
using JolTudomE_WS.Model;

namespace JolTudomE_WS
{
  [ServiceContract(Name="JolTudomEWS", Namespace="JolTudomEWS.org")]
  public interface IWSService
  {

    [OperationContract]
    string Login(string username, string password, out PersonDetails loggedinuser);

    [OperationContract]
    void Logout(string token);

    [OperationContract]
    List<Statistics> GetStatistics(string token, int? personid);

    [OperationContract]
    List<TestDetails> GetTestDetails(string token, int testid, int? personid);

    [OperationContract]
    void CancelTest(string token, int testid, int personid);

    [OperationContract]
    List<Courses> GetCourses(string token);

    [OperationContract]
    List<Topics> GetTopics(string token, int courseid);

    [OperationContract]
    void NewUser(string token, string username, string prefix, string lastname, string middlename, string firstname, string password, int roleid);

    [OperationContract]
    NewTest StartTest(string token, int count, List<int> topicids);

    [OperationContract]
    void CheckAnswer(string token, int personid, int testid, int questid, int? answid, bool iscomplete);

    [OperationContract]
    List<PersonDetails> GetUsers(string token, int? searchroleid);

    [OperationContract]
    void AddNewCourse(string token, string name, string description);

    [OperationContract]
    void EditCourse(string token, int courseid, string name, string description);

    [OperationContract]
    void AddNewTopic(string token, int courseid, string name, string description);

    [OperationContract]
    void EditTopic(string token, int courseid, int topicid, string name, string description);

  }

}
