using JolTudomE_WP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JolTudomE_WP.ViewModel {
  public class SelectedUserViewModel : BaseNotifyable, IViewModel {

    private bool _ShowProgressBar;
    public bool ShowProgressBar {
      get { return _ShowProgressBar; }
      set { SetProperty<bool>(ref _ShowProgressBar, value); }
    }


    private int _CurrentPivotItem;
    public int CurrentPivotItem {
      get { return _CurrentPivotItem; }
      set {
        SetProperty<int>(ref _CurrentPivotItem, value);
        IsProfilButtonsShown = _CurrentPivotItem == 2;
      }
    }

    private ProfilViewModel _ProfilVM;
    public ProfilViewModel ProfilVM {
      get { return _ProfilVM; }
      set {
        SetProperty<ProfilViewModel>(ref _ProfilVM, value);  
      }
    }

    private List<Statistic> _StatisticList;
    public List<Statistic> StatisticList {
      get { return _StatisticList; }
      set {
        SetProperty<List<Statistic>>(ref _StatisticList, value);
        IsStatisticEmpty = _StatisticList != null && _StatisticList.Count == 0;
      }
    }

    private bool _IsStatisticEmpty;
    public bool IsStatisticEmpty {
      get { return _IsStatisticEmpty; }
      set { SetProperty<bool>(ref _IsStatisticEmpty, value); }
    }

    private ObservableCollection<Course> _CourseList;
    public ObservableCollection<Course> CourseList {
      get { return _CourseList; }
      set { 
        SetProperty<ObservableCollection<Course>>(ref _CourseList, value);
        if (_CourseList != null) SelectedCourse = _CourseList.First();
      }
    }

    private ObservableCollection<Topic> _TopicList;
    public ObservableCollection<Topic> TopicList {
      get { return _TopicList; }
      set { SetProperty<ObservableCollection<Topic>>(ref _TopicList, value); }
    }

    private Course _SelectedCourse;
    public Course SelectedCourse {
      get { return _SelectedCourse; }
      set {
        SetProperty<Course>(ref _SelectedCourse, value);
        GetTopicList();
      }
    }

    private int _NumberQuestion;
    public int NumberQuestion {
      get { return _NumberQuestion; }
      set { SetProperty<int>(ref _NumberQuestion, value); }
    }

    private List<int> _SelectedTopics;
    public List<int> SelectedTopics {
      get { return _SelectedTopics; }
      set { _SelectedTopics = value; }
    }

    private bool _IsTopicErrorShown;
    public bool IsTopicErrorShown {
      get { return _IsTopicErrorShown; }
      set { SetProperty<bool>(ref _IsTopicErrorShown, value); }
    }

    private bool _IsProfilButtonsShown;
    public bool IsProfilButtonsShown {
      get { return _IsProfilButtonsShown; }
      set { SetProperty<bool>(ref _IsProfilButtonsShown, value); }
    }

    private string _SelectedUser;
    public string SelectedUser {
      get { return _SelectedUser; }
      set { SetProperty<string>(ref _SelectedUser, value); }
    }

    //private bool _ShowNewTestPivot;
    //public bool ShowNewTestPivot {
    //  get { return _ShowNewTestPivot; }
    //  set { SetProperty<bool>(ref _ShowNewTestPivot, value); }
    //}

    //private bool _ShowProfilPivot;
    //public bool ShowProfilPivot {
    //  get { return _ShowProfilPivot; }
    //  set { SetProperty<bool>(ref _ShowProfilPivot, value); }
    //}

    public SelectedUserViewModel() {

      NumberQuestion = 15;
      IsTopicErrorShown = false;
      SelectedUser = "Belépett Felhasználó";
      ProfilVM = new ProfilViewModel();

    }

    private async void GetTopicList() {
      TopicList = await DataSource.GetTopics(_SelectedCourse.CourseID);
      SelectedTopics = new List<int>();
      IsTopicErrorShown = false;
    }


    public async void LoadData(object customdata) {
      ShowProgressBar = true;
      
      ProfilVM.LoadData(null);
      SelectedUser = DataSource.SelectedUserInfo.DisplayName;

      //ShowNewTestPivot = DataSource.SelectedUserInfo.PersonID == DataSource.LoggedInInfo.PersonID;
      //ShowProfilPivot = DataSource.SelectedUserInfo.RoleID == DataSource.GetRoleStudent().RoleID;

      StatisticList = await DataSource.GetStatistic();
      CourseList = await DataSource.GetCourses();

      ShowProgressBar = false;
    }
  }
}
