using GalaSoft.MvvmLight.Command;
using JolTudomE_Client.Message;
using JolTudomE_Client.Model;

namespace JolTudomE_Client.ViewModel {

  public class AdmCourseCWViewModel : BaseCWViewModel<ClientCourse> {

    private RelayCommand _cmdSaveCourse;
    public RelayCommand cmdSaveCourse {
      get {
        if (_cmdSaveCourse == null) {
          _cmdSaveCourse = new RelayCommand(() => {
            ErrorMsg = string.Empty;
            // add implement in here:
            try {
              if (Instance.ModelState == ModelState.New) {
                _DataService.NewCourse(Instance.Name, Instance.Description);
              }
              else {
                _DataService.EditCourse(Instance.ID, Instance.Name, Instance.Description);
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
        return _cmdSaveCourse;
      }
    }

    public AdmCourseCWViewModel(WSManager ds)
      : base(ds) {

        ErrorMsg = string.Empty;

      if (IsInDesignMode) {
      }
      else {
      }
    }

  }
    
}
