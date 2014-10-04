using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Phone1.ViewModel
{
  public class MainViewModel : ViewModelBase {


    private bool _IsLoginPopupShow;
    public bool IsLoginPopupShow {
      get { return _IsLoginPopupShow; }
      set {
        _IsLoginPopupShow = value;
        RaisePropertyChanged<bool>(() => this.IsLoginPopupShow);
      }
    }
        
      
    public MainViewModel() {
      if (IsInDesignMode) {
        // Code runs in Blend --> create design time data.
        IsLoginPopupShow = true;
      }
      else {
        // Code runs "for real"
        IsLoginPopupShow = true;

      }
    }
  }
}