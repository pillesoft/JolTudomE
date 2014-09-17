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

  public class AdmTopicCWViewModel : BaseCWViewModel<ClientTopic> {

    private RelayCommand _cmdSaveTopic;
    public RelayCommand cmdSaveTopic {
      get {
        if (_cmdSaveTopic == null) {
          _cmdSaveTopic = new RelayCommand(() => {
            ErrorMsg = string.Empty;
            // add implement in here:
            try {
              if (Instance.ModelState == ModelState.New) {
                _DataService.NewTopic(Instance.CourseID, Instance.Name, Instance.Description);
              }
              else {
                _DataService.EditTopic(Instance.CourseID, Instance.ID, Instance.Name, Instance.Description);
              }
              ErrorMsg = string.Empty;

              MessengerInstance.Send<ChildWindowMessage>(new ChildWindowMessage { Action = DialogAction.Close });
            }
            catch (TranslatedException exc) {
              ErrorMsg = exc.Message;
            }
            
          },
          () => Instance != null && Instance.IsValid());
        }
        return _cmdSaveTopic;
      }
    }



    public AdmTopicCWViewModel(WSManager ds)
      : base(ds) {

        ErrorMsg = string.Empty;

      if (IsInDesignMode) {
      }
      else {
      }
    }

  }
    
}
