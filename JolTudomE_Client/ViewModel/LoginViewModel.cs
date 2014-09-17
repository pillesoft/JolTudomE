using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JolTudomE_Client.Helper;
using JolTudomE_Client.Message;

namespace JolTudomE_Client.ViewModel
{
  public class LoginViewModel : BaseViewModel
  {

    private string _UserName;
    public string UserName {
      get { return _UserName; }
      set {
        _UserName = value;
        RaisePropertyChanged<string>(() => this.UserName);
      }
    }

    private string _Password;
    public string Password {
      get { return _Password; }
      set {
        _Password = value;
        RaisePropertyChanged<string>(() => this.Password);
      }
    }

    private RelayCommand _LoginCommand;
    public RelayCommand LoginCommand {
      get {
        if (_LoginCommand == null) {
          _LoginCommand = new RelayCommand(() => {
            // add implement in here:
            ErrorMsg = string.Empty;

            string result;
            if (!_DataService.Login(UserName, Password, out result)) {
              ErrorMsg = result;
            }
          });
        }
        return _LoginCommand;
      }
    }

    private RelayCommand _CancelCommand;
    public RelayCommand CancelCommand {
      get {
        if (_CancelCommand == null) {
          _CancelCommand = new RelayCommand(() => {
            // add implement in here:
            MessengerInstance.Send<CloseApplicationMessage>(new CloseApplicationMessage());
          });
        }
        return _CancelCommand;
      }
    }
      
    private RelayCommand _RegisterViewCommand;
    public RelayCommand RegisterViewCommand {
      get {
        if (_RegisterViewCommand == null) {
          _RegisterViewCommand = new RelayCommand(() => {
            // add implement in here:
            MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.NewPerson });
          });
        }
        return _RegisterViewCommand;
      }
    }

    protected override void LoadData() {
      ErrorMsg = string.Empty;
      UserName = string.Empty;
      Password = string.Empty;
    }

    public LoginViewModel(WSManager ds):base(ds) {

      if (IsInDesignMode) {
        ErrorMsg = "Error message";
        UserName = "Student1";
        Password = "12345678";
      }
      else {
      }
    }

  }
}
