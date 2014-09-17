using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using JolTudomE_Client.Helper;
using JolTudomE_Client.Message;
using JolTudomE_Client.Model;

namespace JolTudomE_Client.ViewModel {

  public class AdministrationViewModel : BaseViewModel {

    private RelayCommand _cmdBack;
    public RelayCommand cmdBack {
      get {
        if (_cmdBack == null) {
          _cmdBack = new RelayCommand(() => {
            // add implement in here:
            MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Student });
          });
        }
        return _cmdBack;
      }
    }

    private RelayCommand _cmdCourse;
    public RelayCommand cmdCourse {
      get {
        if (_cmdCourse == null) {
          _cmdCourse = new RelayCommand(() => {
            // add implement in here:
            MessengerInstance.Send<AdmNavigationMessage>(new AdmNavigationMessage { View = AdminViewEnum.Course });
          },
          () => AdmContent != null && AdmContent.GetType().Name != typeof(AdmCourseViewModel).Name);
        }
        return _cmdCourse;
      }
    }

    private RelayCommand _cmdTopic;
    public RelayCommand cmdTopic {
      get {
        if (_cmdTopic == null) {
          _cmdTopic = new RelayCommand(() => {
            // add implement in here:
            MessengerInstance.Send<AdmNavigationMessage>(new AdmNavigationMessage { View = AdminViewEnum.Topic });
          },
          () => AdmContent != null && AdmContent.GetType().Name != typeof(AdmTopicViewModel).Name);
        }
        return _cmdTopic;
      }
    }

    private RelayCommand _cmdQuestions;
    public RelayCommand cmdQuestions {
      get {
        if (_cmdQuestions == null) {
          _cmdQuestions = new RelayCommand(() => {
            // add implement in here:
          });
        }
        return _cmdQuestions;
      }
    }

    private RelayCommand _cmdNewUser;
    public RelayCommand cmdNewUser {
      get {
        if (_cmdNewUser == null) {
          _cmdNewUser = new RelayCommand(() => {
            // add implement in here:
            MessengerInstance.Send<AdmNavigationMessage>(new AdmNavigationMessage { View = AdminViewEnum.NewPerson });
          },
          () => AdmContent != null && AdmContent.GetType().Name != typeof(NewPersonViewModel).Name);
        }
        return _cmdNewUser;
      }
    }
      
    private BaseViewModel _AdmContent;
    public BaseViewModel AdmContent {
      get { return _AdmContent; }
      set {
        _AdmContent = value;
        RaisePropertyChanged<BaseViewModel>(() => this.AdmContent);
      }
    }

    private bool _IsAdminUser;
    public bool IsAdminUser {
      get { return _IsAdminUser; }
      set {
        _IsAdminUser = value;
        RaisePropertyChanged<bool>(() => this.IsAdminUser);
      }
    }

    protected override void LoadData() {
      MessengerInstance.Send<AdmNavigationMessage>(new AdmNavigationMessage { View = AdminViewEnum.Course });

      IsAdminUser = _DataService.LoggedInUser != null && _DataService.LoggedInUser.RoleEnum == PersonRoleEnum.Admin;
    }

    public AdministrationViewModel(WSManager ds):base(ds) {

      MessengerInstance.Register<AdmNavigationMessage>(this, (m) => {
        switch (m.View) {
          case AdminViewEnum.Course:
            AdmContent = SimpleIoc.Default.GetInstance<AdmCourseViewModel>();
            break;
          case AdminViewEnum.Topic:
            AdmContent = SimpleIoc.Default.GetInstance<AdmTopicViewModel>();
            break;
          case AdminViewEnum.Question:
            break;
          case AdminViewEnum.NewPerson:
            AdmContent = SimpleIoc.Default.GetInstance<NewPersonViewModel>();
            break;
          default:
            break;
        }

      });


      if (IsInDesignMode) {
      }
      else {
      }
    }
  }
}
