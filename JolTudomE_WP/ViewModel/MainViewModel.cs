using JolTudomE_WP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JolTudomE_WP.ViewModel {
  public class MainViewModel : BaseNotifyable, IViewModel {

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

    ProfilViewModel ProfilVM {
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
        IsStatisticEmpty = _StatisticList.Count == 0;
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
        SelectedCourse = _CourseList.First();
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

    public MainViewModel() {

      NumberQuestion = 15;
      IsTopicErrorShown = false;
      ProfilVM = new ProfilViewModel();

      /* DESIGN TIME DATA
      Statistic st = new Statistic {
        CorrectAnswer = 12,
        Generated = DateTime.Now,
        Percent = (decimal)0.67,
        Questions = 15,
        TestID = 1,
        TotalTime = new TimeSpan(0, 12, 23)
      };
      Statistic st1 = new Statistic {
        CorrectAnswer = 8,
        Generated = DateTime.Now,
        Percent = (decimal)0.12,
        Questions = 15,
        TestID = 2,
        TotalTime = new TimeSpan(0, 10, 43)
      };
      StatisticList = new List<Statistic>();
      StatisticList.Add(st);
      StatisticList.Add(st1);

      TopicList = new ObservableCollection<Topic>();
      TopicList.Add(new Topic { TopicID = 1, TopicName = "topic name 1", TopicDescription = "topic 1 description" });
      TopicList.Add(new Topic { TopicID = 2, TopicName = "topic name 2", TopicDescription = "topic 2 description" });
      TopicList.Add(new Topic { TopicID = 1, TopicName = "topic name 1", TopicDescription = "topic 1 description" });
      TopicList.Add(new Topic { TopicID = 2, TopicName = "topic name 2", TopicDescription = "topic 2 description" });
      TopicList.Add(new Topic { TopicID = 1, TopicName = "topic name 1", TopicDescription = "topic 1 description" });
      TopicList.Add(new Topic { TopicID = 2, TopicName = "topic name 2", TopicDescription = "topic 2 description" });
      */
    }

    private async void GetTopicList() {
      TopicList = await DataSource.GetTopics(_SelectedCourse.CourseID);
      SelectedTopics = new List<int>();
      IsTopicErrorShown = false;
    }


    public async void LoadData(object customdata) {
      ShowProgressBar = true;
      
      ProfilVM.LoadData(null);

      StatisticList = await DataSource.GetStatistic((int)customdata);
      CourseList = await DataSource.GetCourses();

      ShowProgressBar = false;
    }
  }
}
