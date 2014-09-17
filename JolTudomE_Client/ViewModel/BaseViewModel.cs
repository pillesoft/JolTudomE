using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JolTudomE_Client.Message;

namespace JolTudomE_Client.ViewModel {
  public class BaseViewModel : ViewModelBase {
    protected WSManager _DataService { get; private set; }

    private RelayCommand _LoadedEventCommand;
    public RelayCommand LoadedEventCommand {
      get {
        if (_LoadedEventCommand == null) {
          _LoadedEventCommand = new RelayCommand(() => {
            // add implement in here:
            LoadData();
          });
        }
        return _LoadedEventCommand;
      }
    }

    private string _ErrorMsg;
    public string ErrorMsg {
      get {
        return _ErrorMsg;
      }
      set {
        _ErrorMsg = value;
        RaisePropertyChanged<string>(() => this.ErrorMsg);
      }
    }

    protected virtual void LoadData() {
      throw new NotImplementedException();
    }
      
    public virtual bool CanCloseWindowConfirmation() {

      bool canclose = false;

      ShowDialogMessage sdm = new ShowDialogMessage();
      sdm.Title = "Kilépés a Rendszerből";
      sdm.Text = "Biztosan kilép?";

      sdm.Buttons = new DialogButton[] { 
          new DialogButton{Title="Kilépek", IsDefault=false, ReturnValue=true},
          new DialogButton{Title="Maradok", IsDefault=true, ReturnValue=false},
        };
      sdm.ExecuteIfTrue = () => {
        _DataService.ClearSession();

        canclose = true;
      };
      sdm.ExecuteIfFalse = () => {
        canclose = false;
      };

      MessengerInstance.Send<ShowDialogMessage>(sdm);

      return canclose;

    }

    public BaseViewModel(WSManager ds) {
      _DataService = ds;
    }

  }
}
