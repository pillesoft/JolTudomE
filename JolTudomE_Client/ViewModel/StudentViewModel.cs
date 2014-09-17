using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JolTudomE_Client.Helper;
using JolTudomE_Client.JolTudomEWSService;
using JolTudomE_Client.Message;
using JolTudomE_Client.Model;

namespace JolTudomE_Client.ViewModel {
  public class StudentViewModel : BaseViewModel {

    private PersonDetails _LoggedInPerson;
    //private List<Topics> _SelectedTopicsList;

    private ObservableCollection<Statistics> _StatisticList;
    public ObservableCollection<Statistics> StatisticList {
      get { return _StatisticList; }
      set {
        _StatisticList = value;
        RaisePropertyChanged<ObservableCollection<Statistics>>(() => this.StatisticList);
        if (_StatisticList.Count > 0) {
          SelectedStatistics = _StatisticList.First();
        }
      }
    }


    private Statistics _SelectedStatistics;
    public Statistics SelectedStatistics {
      get { return _SelectedStatistics; }
      set {
        _SelectedStatistics = value;
        RaisePropertyChanged<Statistics>(() => this.SelectedStatistics);
      }
    }
        
    private List<Courses> _CourseList;
    public List<Courses> CourseList {
      get { return _CourseList; }
      set {
        _CourseList = value;
        RaisePropertyChanged<List<Courses>>(() => this.CourseList);
        SelectedCourse = _CourseList.FirstOrDefault();
      }
    }

    private Courses _SelectedCourse;
    public Courses SelectedCourse {
      get { return _SelectedCourse; }
      set {
        _SelectedCourse = value;
        RaisePropertyChanged<Courses>(() => this.SelectedCourse);
        if (_SelectedCourse != null) {
          try {
            TopicList = _DataService.GetTopics(_SelectedCourse.CourseID);
            ErrorMsg = string.Empty;
          }
          catch (SessionExpiredException) {
            MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
          }
        }
      }
    }
        
    private List<Topics> _TopicList;
    public List<Topics> TopicList {
      get { return _TopicList; }
      set {
        _TopicList = value;
        RaisePropertyChanged<List<Topics>>(() => this.TopicList);

        if (_TopicList.Count > 0) {
          TopicList.First().IsSelected = true;
          //_SelectedTopicsList = new List<Topics>();
          //_SelectedTopicsList.Add(_TopicList.First());
        }
      }
    }

    //private RelayCommand<List<object>> _TopicSelectionChangedCommand;
    //public RelayCommand<List<object>> TopicSelectionChangedCommand {
    //  get {
    //    if (_TopicSelectionChangedCommand == null) {
    //      _TopicSelectionChangedCommand = new RelayCommand<List<object>>((objlist) => {
    //        // add implement in here:
    //        _SelectedTopicsList = new List<Topics>();
    //        foreach (var item in objlist) {
    //          _SelectedTopicsList.Add((Topics)item);
    //        }
    //      });
    //    }
    //    return _TopicSelectionChangedCommand;
    //  }
    //}
      
    private int _NumberQuestion;
    public int NumberQuestion {
      get { return _NumberQuestion; }
      set {
        _NumberQuestion = value;
        RaisePropertyChanged<int>(() => this.NumberQuestion);
      }
    }
    
    private RelayCommand _StartTestCommand;
    public RelayCommand StartTestCommand {
      get {
        if (_StartTestCommand == null) {
          _StartTestCommand = new RelayCommand(() => {
            // check the selection
            if (SelectedCourse == null) {
              ErrorMsg = "Kurzust kell választani!";
            }
            else {
              if (TopicList.Count(t => t.IsSelected) == 0) {
                ErrorMsg = "Legalább egy témakört kell választani!";
              }
              else
                MessengerInstance.Send<NavigationMessage>(new NavigationMessage {
                  View = ViewEnum.TestExecution,
                  UserState = new TestExecutionInit { NumberOfQuestions = NumberQuestion, TopicIDs = TopicList.Where(t => t.IsSelected).Select(t => t.TopicID).ToList() }
                });
            }
            //else if (_SelectedTopicsList == null
            //  || _SelectedTopicsList != null && _SelectedTopicsList.Count == 0) {
            //  ErrorMsg = "Legalább egy témakört kell választani!";
            //}
            //else {
            //  MessengerInstance.Send<NavigationMessage>(new NavigationMessage {
            //    View = ViewEnum.TestExecution,
            //    UserState = new TestExecutionInit { NumberOfQuestions = NumberQuestion, TopicIDs = _SelectedTopicsList.Select(t => t.TopicID).ToList() }
            //  });

            //}

          });
        }
        return _StartTestCommand;
      }
    }
      
    private bool _ShowPersonList;
    public bool ShowPersonList {
      get { return _ShowPersonList; }
      set {
        _ShowPersonList = value;
        RaisePropertyChanged<bool>(() => this.ShowPersonList);
      }
    }
        
    private PersonDetails _SelectedUser;
    public PersonDetails SelectedUser {
      get { return _SelectedUser; }
      set {
        _SelectedUser = value;
        RaisePropertyChanged<PersonDetails>(() => this.SelectedUser);

        if (_SelectedUser != null) {
          //ErrorMsg = string.Empty;
          try {
            StatisticList = new ObservableCollection<Statistics>(_DataService.GetStatistics(_SelectedUser.PersonID));

            TestResultCaption = string.Format("Eddigi Tesztek Eredményei: {0}", _SelectedUser.TreeDisplayName);
          }
          catch (SessionExpiredException) {
            MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
          }
        }
        else {
          TestResultCaption = string.Empty;
          //ErrorMsg = "Kérlek, válassz Felhasználót, ne csoportot!";
        }
      }
    }

    private string _TestResultCaption;
    public string TestResultCaption {
      get { return _TestResultCaption; }
      set {
        _TestResultCaption = value;
        RaisePropertyChanged<string>(() => this.TestResultCaption);
      }
    }

    private List<PersonRole> _PersonTreeList;
    public List<PersonRole> PersonTreeList {
      get { return _PersonTreeList; }
      set {
        _PersonTreeList = value;
        RaisePropertyChanged<List<PersonRole>>(() => this.PersonTreeList);
      }
    }

    private RelayCommand<object> _UserTreeSelectionChangedCommand;
    public RelayCommand<object> UserTreeSelectionChangedCommand {
      get {
        if (_UserTreeSelectionChangedCommand == null) {
          _UserTreeSelectionChangedCommand = new RelayCommand<object>((selecteduser) => {
            // add implement in here:
            if (selecteduser != null && selecteduser.GetType() == typeof(PersonDetails)) {
              SelectedUser = selecteduser as PersonDetails;
            }
            else {
              SelectedUser = null;
              StatisticList = new ObservableCollection<Statistics>();
            }
          });
        }
        return _UserTreeSelectionChangedCommand;
      }
    }

    private RelayCommand _cmdTestDetails;
    public RelayCommand cmdTestDetails {
      get {
        if (_cmdTestDetails == null) {
          _cmdTestDetails = new RelayCommand(() => {
            // add implement in here:

            List<TestDetails> testdet = new List<TestDetails>();
            try {
              if (_SelectedUser == null)
                testdet = _DataService.GetTestDetails(_SelectedStatistics.TestID, null);
              else
                testdet = _DataService.GetTestDetails(_SelectedStatistics.TestID, _SelectedUser.PersonID);

            }
            catch (SessionExpiredException) {
              MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
            }

            ChildWindowMessage cwm = new ChildWindowMessage {
              Action = DialogAction.Open,
              ModelInstance = testdet
            };

            MessengerInstance.Send<ChildWindowMessage>(cwm);
          });
        }
        return _cmdTestDetails;
      }
    }

    private bool _IsNewTestExpanded;
    public bool IsNewTestExpanded {
      get { return _IsNewTestExpanded; }
      set {
        _IsNewTestExpanded = value;
        RaisePropertyChanged<bool>(() => this.IsNewTestExpanded);
      }
    }

    protected override void LoadData() {
      try {
        // show the New Test Start controls if we have error message from the Test Execution initialization
        // otherwise hide it
        IsNewTestExpanded = !string.IsNullOrEmpty(ErrorMsg);

        // has to save the ErrorMsg to temp variable
        // because if the StudentView is called back from TestExecution, but we had error there at NewTest
        // the Error from that VM has to show here
        var olderror = ErrorMsg;

        _LoggedInPerson = (PersonDetails)_DataService.LoggedInUser;

        if (_LoggedInPerson.RoleEnum == PersonRoleEnum.Student) {
          // this is the student

          TestResultCaption = string.Format("Eddigi Tesztek Eredményei: {0}", _LoggedInPerson.TreeDisplayName);

          ShowPersonList = false;
          StatisticList = new ObservableCollection<Statistics>(_DataService.GetStatistics(null));
        }
        else if (_LoggedInPerson.RoleEnum == PersonRoleEnum.Teacher) {
          // this is teacher
          ShowPersonList = true;
          // ask only for students
          PersonTreeList = _DataService.GetUsers((int)PersonRoleEnum.Student);
          SelectedUser = PersonTreeList.Find(p => p.RoleName == "Saját").Children[0];
        }
        else if (_LoggedInPerson.RoleEnum == PersonRoleEnum.Admin) {
          // this is admin
          ShowPersonList = true;
          // ask for everybody
          PersonTreeList = _DataService.GetUsers(null);
          SelectedUser = PersonTreeList.Find(p => p.RoleName == "Saját").Children[0];
        }

        CourseList = _DataService.GetCourses();
        ErrorMsg = olderror;
      }
      catch (SessionExpiredException) {
        MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
      }
    }

    public StudentViewModel(WSManager ds):base(ds) {

      NumberQuestion = 15;

      if (IsInDesignMode) {
        _LoggedInPerson = new PersonDetails() { PersonID = 1, FirstName = "jozsika" };

        TestResultCaption = string.Format("Eddigi Tesztek Eredményei: {0}", _LoggedInPerson.TreeDisplayName);

        ErrorMsg = "Ez meg az error";
        ShowPersonList = true;

        StatisticList = new ObservableCollection<Statistics>();
        StatisticList.Add(new Statistics {
          Generated = DateTime.Now,
          TestID = 2,
          Questions = 15,
          CorrectAnswer = 3,
          Percent = 3,
          TotalTime = TimeSpan.FromSeconds(69)
        });

        PersonTreeList = new List<PersonRole>();
        PersonTreeList.Add(new PersonRole { RoleName = "Saját", Children = new List<PersonDetails>() { _LoggedInPerson } });

        PersonTreeList.Add(new PersonRole { RoleName = "Diák", Children = new List<PersonDetails>() { new PersonDetails { FirstName = "vki", LastName = "vki" } } });
      }
      else {
      }

    }

  }
}
