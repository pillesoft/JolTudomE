using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phone1.ViewModel {

  public class LoginViewModel : ViewModelBase {

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


    private string _LoginErrorText;
    public string LoginErrorText {
      get { return _LoginErrorText; }
      set {
        _LoginErrorText = value;
        RaisePropertyChanged<string>(() => this.LoginErrorText);
      }
    }

    private RelayCommand _cmdBejelentkezes;
    public RelayCommand cmdBejelentkezes {
      get {
        if (_cmdBejelentkezes == null) {
          _cmdBejelentkezes = new RelayCommand(() => {
            // add implement in here:
            LoginErrorText = string.Empty;

            JolTudomEService.JolTudomEWSClient client = new JolTudomEService.JolTudomEWSClient();
            client.LoginAsync(UserName, Password);
            client.LoginCompleted += client_LoginCompleted;
            //WepAPIManager wam = new WepAPIManager();
            //bool isloginok = await wam.Login(UserName, Password);
            //if (isloginok) {
            //  IsLoginPopupShow = false;
            //  wam.GetStatistics();
            //}
            //else {
            //  LoginErrorText = "Rossz felhasznalo, jelszo!";
            //}
          });
        }
        return _cmdBejelentkezes;
      }
    }

    void client_LoginCompleted(object sender, JolTudomEService.LoginCompletedEventArgs e) {
      string sa = "asdf";
    }

    public LoginViewModel() {
      if (IsInDesignMode) {
        // Code runs in Blend --> create design time data.
        UserName = "Ivanka";
        Password = "12345678";
        LoginErrorText = "Rossz felhasznalo, jelszo!";
      }
      else {
        // Code runs "for real"

        // comment these lines when deploy
        UserName = "Ivanka";
        Password = "12345678";

      }

    }

  }
}
