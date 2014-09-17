using System;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using JolTudomE_Client.Helper;
using JolTudomE_Client.Message;
using JolTudomE_Client.Model;

namespace JolTudomE_Client.ViewModel {
  public class MainViewModel : BaseViewModel {

    private string _WelcomeString;
    public string WelcomeString {
      get { return _WelcomeString; }
      set {
        _WelcomeString = value;
        RaisePropertyChanged<string>(() => this.WelcomeString);
      }
    }

    private BaseViewModel _CurrentViewModel;
    public BaseViewModel CurrentViewModel {
      get { return _CurrentViewModel; }
      set {
        _CurrentViewModel = value;
        RaisePropertyChanged<BaseViewModel>(() => this.CurrentViewModel);
      }
    }


    private RelayCommand _CloseCommand;
    public RelayCommand CloseCommand {
      get {
        if (_CloseCommand == null) {
          _CloseCommand = new RelayCommand(() => {
            // add implement in here:
          },
            // display here the confirmation dialog
          () => CurrentViewModel.CanCloseWindowConfirmation()
          );
        }
        return _CloseCommand;
      }
    }


    private bool _IsAuthenticated;
    public bool IsAuthenticated {
      get { return _IsAuthenticated; }
      set {
        _IsAuthenticated = value;
        RaisePropertyChanged<bool>(() => this.IsAuthenticated);
      }
    }

    private bool _IsAdminVisible;
    public bool IsAdminVisible {
      get { return _IsAdminVisible; }
      set {
        _IsAdminVisible = value;
        RaisePropertyChanged<bool>(() => this.IsAdminVisible);
      }
    }

    private RelayCommand _cmdAdminCommand;
    public RelayCommand cmdAdminCommand {
      get {
        if (_cmdAdminCommand == null) {
          _cmdAdminCommand = new RelayCommand(() => {
            // add implement in here:
            MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Admin });
          },
          () => CurrentViewModel.GetType().Name != typeof(AdministrationViewModel).Name);
        }
        return _cmdAdminCommand;
      }
    }


    private bool _IscmdLogoutVisible;
    public bool IscmdLogoutVisible {
      get { return _IscmdLogoutVisible; }
      set {
        _IscmdLogoutVisible = value;
        RaisePropertyChanged<bool>(() => this.IscmdLogoutVisible);
      }
    }

    private RelayCommand _cmdLogoutCommand;
    public RelayCommand cmdLogoutCommand {
      get {
        if (_cmdLogoutCommand == null) {
          _cmdLogoutCommand = new RelayCommand(() => {
            // add implement in here:
            _DataService.Logout();
          });
        }
        return _cmdLogoutCommand;
      }
    }


    //private RelayCommand _CloseFailCommand;
    //public RelayCommand CloseFailCommand {
    //  get {
    //    if (_CloseFailCommand == null) {
    //      _CloseFailCommand = new RelayCommand(() => {
    //        // add implement in here:

    //        string sa = "sdfg";
    //      });
    //    }
    //    return _CloseFailCommand;
    //  }
    //}


    private void AfterLogin(AfterLoginLogoutMessage m) {

      IsAuthenticated = _DataService.LoggedInUser != null;

      if (IsAuthenticated) {
        IsAdminVisible = _DataService.LoggedInUser.RoleEnum == PersonRoleEnum.Teacher
          || _DataService.LoggedInUser.RoleEnum == PersonRoleEnum.Admin;
        IscmdLogoutVisible = true;

        WelcomeString = string.Format("Isten hozott {0}!", _DataService.LoggedInUser.TreeDisplayName);
      }
      else {
        IsAdminVisible = false;
      }
    }

    public MainViewModel(WSManager ds)
      : base(ds) {

      MessengerInstance.Register<AfterLoginLogoutMessage>(this, AfterLogin);

      MessengerInstance.Register<TestExecutionRunningMessage>(this, (m) => {
        if (m.IsTestRunning) {
          IsAdminVisible = false;
          IscmdLogoutVisible = false;
        }
        else {
          IsAdminVisible = _DataService.LoggedInUser.RoleEnum == PersonRoleEnum.Teacher
            || _DataService.LoggedInUser.RoleEnum == PersonRoleEnum.Admin;
          IscmdLogoutVisible = true;
        }
      });

      MessengerInstance.Register<NavigationMessage>(this, (m) => {
        switch (m.View) {
          case ViewEnum.Login:
            CurrentViewModel = SimpleIoc.Default.GetInstance<LoginViewModel>();
            if (m.UserState != null && m.UserState.ToString() == "SessionExpired") {
              IscmdLogoutVisible = false;
              IsAdminVisible = false;
              WelcomeString = "Lejárt a Session. Kérlek, jelentkezz be újra.";
            }
            break;
          case ViewEnum.Student:
            CurrentViewModel = SimpleIoc.Default.GetInstance<StudentViewModel>();
            CurrentViewModel.ErrorMsg = string.Empty;
            if (m.UserState != null && ((string[])m.UserState)[0] == "TestCreateFailed") {
              CurrentViewModel.ErrorMsg = ((string[])m.UserState)[1];
            }
            break;
          case ViewEnum.NewPerson:
            CurrentViewModel = SimpleIoc.Default.GetInstance<NewPersonViewModel>();
            break;
          case ViewEnum.TestExecution:
            // force viewmodel initialization always in order to handle the right test execution
            if (SimpleIoc.Default.IsRegistered<TestExecutionViewModel>())
              SimpleIoc.Default.Unregister<TestExecutionViewModel>();

            // register TestExec Init data - if it is not yet registered
            if (!SimpleIoc.Default.IsRegistered<TestExecutionInit>()) {
              SimpleIoc.Default.Register<TestExecutionInit>();
            }

            var init = SimpleIoc.Default.GetInstance<TestExecutionInit>();
            init.NumberOfQuestions = ((TestExecutionInit)m.UserState).NumberOfQuestions;
            init.TopicIDs = ((TestExecutionInit)m.UserState).TopicIDs;

            SimpleIoc.Default.Register<TestExecutionViewModel>();
            CurrentViewModel = SimpleIoc.Default.GetInstance<TestExecutionViewModel>();
            break;
          case ViewEnum.Admin:
            CurrentViewModel = SimpleIoc.Default.GetInstance<AdministrationViewModel>();
            break;
          default:
            break;
        }

      });

      if (this.IsInDesignMode) {
      }
      else {
        MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login });
      }
    }
  }
}