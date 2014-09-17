using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Command;
using JolTudomE_Client.JolTudomEWSService;
using JolTudomE_Client.Message;
using JolTudomE_Client.Model;

namespace JolTudomE_Client.ViewModel {

  public class AdmTopicViewModel : BaseViewModel {
    
    private ObservableCollection<Courses> _CourseList;
    public ObservableCollection<Courses> CourseList {
      get { return _CourseList; }
      set {
        _CourseList = value;
        RaisePropertyChanged<ObservableCollection<Courses>>(() => this.CourseList);
      }
    }


    private ObservableCollection<ClientTopic> _TopicList;
    public ObservableCollection<ClientTopic> TopicList {
      get { return _TopicList; }
      set {
        _TopicList = value;
        RaisePropertyChanged<ObservableCollection<ClientTopic>>(() => this.TopicList);
      }
    }
        

    private Courses _SelectedCourse;
    public Courses SelectedCourse {
      get { return _SelectedCourse; }
      set {
        _SelectedCourse = value;
        RaisePropertyChanged<Courses>(() => this.SelectedCourse);
        
        ReloadTopic();
        SelectedTopic = TopicList.FirstOrDefault();
      }
    }


    private ClientTopic _SelectedTopic;
    public ClientTopic SelectedTopic {
      get { return _SelectedTopic; }
      set {
        _SelectedTopic = value;
        RaisePropertyChanged<ClientTopic>(() => this.SelectedTopic);
      }
    }
        

    private RelayCommand _cmdNewTopic;
    public RelayCommand cmdNewTopic {
      get {
        if (_cmdNewTopic == null) {
          _cmdNewTopic = new RelayCommand(() => {
            // add implement in here:
            SelectedTopic = ClientTopic.NewTopic(SelectedCourse.CourseID);
            ChildWindowMessage adm = new ChildWindowMessage {
              Action = DialogAction.Open,
              ModelInstance = SelectedTopic,
              ExecuteAfterClose = ReloadTopic
            };

            MessengerInstance.Send<ChildWindowMessage>(adm);

          });
        }
        return _cmdNewTopic;
      }
    }


    private RelayCommand _cmdModify;
    public RelayCommand cmdModify {
      get {
        if (_cmdModify == null) {
          _cmdModify = new RelayCommand(() => {
            // add implement in here:
            ChildWindowMessage adm = new ChildWindowMessage {
              Action = DialogAction.Open,
              ModelInstance = SelectedTopic,
              ExecuteAfterClose = ReloadTopic
            };

            MessengerInstance.Send<ChildWindowMessage>(adm);

          });
        }
        return _cmdModify;
      }
    }

    protected override void LoadData() {
      try {
        CourseList = new ObservableCollection<Courses>(_DataService.GetCourses());
        if (SelectedCourse == null) {
          SelectedCourse = CourseList.FirstOrDefault();
        }

        if (SelectedCourse == null) {
          // this means there is still no course
          ReloadTopic();
        }
      }
      catch (SessionExpiredException) {
        MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
      }
    }

    private void ReloadTopic() {

      try {
        var oldtopic = SelectedTopic;

        var ct = _DataService.GetTopics(SelectedCourse.CourseID);

        TopicList = new ObservableCollection<ClientTopic>();

        foreach (var item in ct) {
          TopicList.Add(ClientTopic.GetClientTopicFromTopic(SelectedCourse.CourseID, item));
        }

        if (oldtopic == null) {
          SelectedTopic = TopicList.FirstOrDefault();
        }
        else if (oldtopic.ModelState == ModelState.New) {
          SelectedTopic = TopicList.FirstOrDefault(c => c.Name == oldtopic.Name);
        }
        else {
          SelectedTopic = TopicList.FirstOrDefault(c => c.ID == oldtopic.ID);
        }

      }
      catch (SessionExpiredException) {
        MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
      }
    }

    public AdmTopicViewModel(WSManager ds) : base(ds) {
      if (IsInDesignMode) {
      }
      else {
      }
    }

  }

}
