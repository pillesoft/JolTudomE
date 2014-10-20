using JolTudomE_WP.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace JolTudomE_WP.ViewModel {
  public class LoggedInUserViewModel : BaseNotifyable, IViewModel {

    private bool _ShowProgressBar;

    public bool ShowProgressBar {
      get { return _ShowProgressBar; }
      set { SetProperty<bool>(ref _ShowProgressBar, value); }
    }


    private int _CurrentPivotItem;
    public int CurrentPivotItem {
      get { return _CurrentPivotItem; }
      set {
        SetProperty<int>(ref _CurrentPivotItem, value);
        IsProfilButtonsShown = _CurrentPivotItem == 1;
      }
    }

    private List<GroupedUser> _UserList;
    public List<GroupedUser> UserList {
      get { return _UserList; }
      set {
        SetProperty<List<GroupedUser>>(ref _UserList, value);

        var result = from user in _UserList group user by user.Role into grp orderby grp.Key.GroupingOrder select grp;
        UserListGrouped.Source = result;

      }
    }

    private ProfilViewModel _ProfilVM;
    public ProfilViewModel ProfilVM {
      get { return _ProfilVM; }
      set {
        SetProperty<ProfilViewModel>(ref _ProfilVM, value);  
      }
    }

    private bool _IsProfilButtonsShown;
    public bool IsProfilButtonsShown {
      get { return _IsProfilButtonsShown; }
      set { SetProperty<bool>(ref _IsProfilButtonsShown, value); }
    }

    private CollectionViewSource _UserListGrouped;
    public CollectionViewSource UserListGrouped {
      get { return _UserListGrouped; }
      set { SetProperty<CollectionViewSource>(ref _UserListGrouped, value); }
    }
    
    public LoggedInUserViewModel() {

      UserListGrouped = new CollectionViewSource();
      UserListGrouped.IsSourceGrouped = true;

      ProfilVM = new ProfilViewModel();
    }

    public async void LoadData(object customdata) {
      ShowProgressBar = true;

      UserList = await DataSource.GetUserList();
      ProfilVM.LoadData(null);

      ShowProgressBar = false;
    }
  }
}
