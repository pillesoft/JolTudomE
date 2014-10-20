using JolTudomE_WP.Common;
using JolTudomE_WP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JolTudomE_WP.ViewModel {
  public class ProfilViewModel : BaseNotifyable, IViewModel {

    private UserDetail _UserProfil;

    public UserDetail UserProfil {
      get { return _UserProfil; }
      set {
        SetProperty<UserDetail>(ref _UserProfil, value);
      }
    }

    private bool _IsProfilInEditMode;

    public bool IsProfilInEditMode {
      get { return _IsProfilInEditMode; }
      set { SetProperty<bool>(ref _IsProfilInEditMode, value); }
    }

    private RelayCommand _EditCommand;

    public RelayCommand EditCommand {
      get {
        if (_EditCommand == null) {
          _EditCommand = new RelayCommand(
              () => IsProfilInEditMode = true,
              () => true);
        }

        return _EditCommand; 
      }
      set { _EditCommand = value; }
    }

    private RelayCommand _SaveCommand;

    public RelayCommand SaveCommand {
      get {
        if (_SaveCommand == null) {
          _SaveCommand = new RelayCommand(
              () => IsProfilInEditMode = false,
              () => true);
        }

        return _SaveCommand;
      }
      set { _SaveCommand = value; }
    }

    private RelayCommand _CancelCommand;

    public RelayCommand CancelCommand {
      get {
        if (_CancelCommand == null) {
          _CancelCommand = new RelayCommand(
              () => IsProfilInEditMode = false,
              () => true);
        }

        return _CancelCommand;
      }
      set { _CancelCommand = value; }
    }

    public ProfilViewModel() {
      IsProfilInEditMode = false;

    }

    public async void LoadData(object customdata) {
      UserProfil = await DataSource.GetLoginDetail();
    }
  }
}
