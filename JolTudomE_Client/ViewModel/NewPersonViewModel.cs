using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using JolTudomE_Client.JolTudomEWSService;
using JolTudomE_Client.Message;
using JolTudomE_Client.Model;

namespace JolTudomE_Client.ViewModel {

  public class NewPersonViewModel : BaseViewModel {

    private NewPerson _NewPersonInst;
    public NewPerson NewPersonInst {
      get { return _NewPersonInst; }
      set {
        _NewPersonInst = value;
        RaisePropertyChanged<NewPerson>(() => this.NewPersonInst);
      }
    }


    private ObservableCollection<string> _RoleList;
    public ObservableCollection<string> RoleList {
      get { return _RoleList; }
      set {
        _RoleList = value;
        RaisePropertyChanged<ObservableCollection<string>>(() => this.RoleList);
      }
    }


    private RelayCommand _RegisterCommand;
    public RelayCommand RegisterCommand {
      get {
        if (_RegisterCommand == null) {
          _RegisterCommand = new RelayCommand(() => {
            // add implement in here:
            try {
              InfoMsg = string.Empty;
              ErrorMsg = string.Empty;

              _DataService.RegisterPerson(NewPersonInst);
              InfoMsg = "A regisztráció sikerült";
              IscmdNewRegisterVisible = true;
            }
            catch (SessionExpiredException) {
              MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
            }
            catch (TranslatedException texc) {
              ErrorMsg = texc.Message;
            }
          },
          () => {
            return NewPersonInst != null && NewPersonInst.IsValid();
          });
        }
        return _RegisterCommand;
      }
    }


    private RelayCommand _CancelCommand;
    public RelayCommand CancelCommand {
      get {
        if (_CancelCommand == null) {
          _CancelCommand = new RelayCommand(() => {
            // add implement in here:
            var lip = _DataService.LoggedInUser as PersonDetails;

            if (lip == null)
              MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login });
          });
        }
        return _CancelCommand;
      }
    }


    private bool _IsCancelCommandVisible;
    public bool IsCancelCommandVisible {
      get { return _IsCancelCommandVisible; }
      set {
        _IsCancelCommandVisible = value;
        RaisePropertyChanged<bool>(() => this.IsCancelCommandVisible);
      }
    }


    private RelayCommand _cmdNewRegister;
    public RelayCommand cmdNewRegister {
      get {
        if (_cmdNewRegister == null) {
          _cmdNewRegister = new RelayCommand(() => {
            // add implement in here:
            LoadData();
          });
        }
        return _cmdNewRegister;
      }
    }
      

    private bool _IscmdNewRegisterVisible;
    public bool IscmdNewRegisterVisible {
      get { return _IscmdNewRegisterVisible; }
      set {
        _IscmdNewRegisterVisible = value;
        RaisePropertyChanged<bool>(() => this.IscmdNewRegisterVisible);
      }
    }
        

    private string _InfoMsg;
    public string InfoMsg {
      get { return _InfoMsg; }
      set {
        _InfoMsg = value;
        RaisePropertyChanged<string>(() => this.InfoMsg);
      }
    }
        

    protected override void LoadData() {
      try {
        ErrorMsg = string.Empty;
        InfoMsg = string.Empty;
        IscmdNewRegisterVisible = false;

        RoleList = new ObservableCollection<string>();
        var lip = _DataService.LoggedInUser as PersonDetails;

        if (lip == null) {
          RoleList.Add("Diák");
          IsCancelCommandVisible = true;
        }
        else {
          RoleList.Add("Diák");
          RoleList.Add("Tanár");
          RoleList.Add("Admin");

          IsCancelCommandVisible = false;
        }
        NewPersonInst = NewPerson.Create();
        RegisterCommand.RaiseCanExecuteChanged();

      }
      catch (SessionExpiredException) {
        MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
      }
    }

    public NewPersonViewModel(WSManager ds)
      : base(ds) {

      if (IsInDesignMode) {
      }
      else {
      }
    }
  }
}
