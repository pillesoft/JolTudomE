using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Command;
using JolTudomE_Client.Message;

namespace JolTudomE_Client.ViewModel {
  public class BaseCWViewModel<T> : BaseViewModel, IBaseCWViewModel<T> {


    private T _Instance;
    public T Instance {
      get { return _Instance; }
      private set {
        _Instance = value;
        RaisePropertyChanged<T>(() => this.Instance);
      }
    }

    private RelayCommand _AdmCloseCommand;
    public RelayCommand AdmCloseCommand {
      get {
        if (_AdmCloseCommand == null) {
          _AdmCloseCommand = new RelayCommand(() => {
            // add implement in here:
            ErrorMsg = string.Empty;

            MessengerInstance.Send<ChildWindowMessage>(new ChildWindowMessage { Action = DialogAction.Close });
          });
        }
        return _AdmCloseCommand;
      }
    }

    public BaseCWViewModel(WSManager ds):base(ds) {

    }

    public void SetInstance(T inst) {
      Instance = inst;
    }
  }
}
