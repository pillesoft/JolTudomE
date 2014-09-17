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

  public class AdmCourseViewModel : BaseViewModel {

    private ObservableCollection<ClientCourse> _CourseList;
    public ObservableCollection<ClientCourse> CourseList {
      get { return _CourseList; }
      set {
        _CourseList = value;
        RaisePropertyChanged<ObservableCollection<ClientCourse>>(() => this.CourseList);
      }
    }

    private ClientCourse _SelectedCourse;
    public ClientCourse SelectedCourse {
      get { return _SelectedCourse; }
      set {
        _SelectedCourse = value;
        RaisePropertyChanged<ClientCourse>(() => this.SelectedCourse);
      }
    }
        
    private RelayCommand _cmdNewCourse;
    public RelayCommand cmdNewCourse {
      get {
        if (_cmdNewCourse == null) {
          _cmdNewCourse = new RelayCommand(() => {
            // add implement in here:
            SelectedCourse = ClientCourse.NewCourse();
            ChildWindowMessage adm = new ChildWindowMessage {
              Action = DialogAction.Open,
              ModelInstance = SelectedCourse,
              ExecuteAfterClose = LoadData
            };

            MessengerInstance.Send<ChildWindowMessage>(adm);
          }/*,
          () => SelectedCourse != null && SelectedCourse.ModelState != ModelState.New*/);
        }
        return _cmdNewCourse;
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
              ModelInstance = SelectedCourse,
              ExecuteAfterClose = LoadData
            };

            MessengerInstance.Send<ChildWindowMessage>(adm);
          });
        }
        return _cmdModify;
      }
    }

    protected override void LoadData() {
      try {
        var _oldcourse = SelectedCourse;
        var cc = _DataService.GetCourses();

        CourseList = new ObservableCollection<ClientCourse>();
        if (cc != null) {
          foreach (var item in cc) {
            CourseList.Add(ClientCourse.GetClientCourseFromCourse(item));
          }
          if (_oldcourse == null) {
            SelectedCourse = CourseList.FirstOrDefault();
          }
          else if (_oldcourse.ModelState == ModelState.New) {
            SelectedCourse = CourseList.FirstOrDefault(c => c.Name == _oldcourse.Name);
          }
          else {
            SelectedCourse = CourseList.FirstOrDefault(c => c.ID == _oldcourse.ID);
          }
        }
      }
      catch (SessionExpiredException) {
        MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
      }
    }

    public AdmCourseViewModel(WSManager ds)
      : base(ds) {

      if (IsInDesignMode) {
      }
      else {
      }
    }

  }
    
}
