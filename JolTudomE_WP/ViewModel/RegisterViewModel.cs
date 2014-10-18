using JolTudomE_WP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JolTudomE_WP.ViewModel {
  class RegisterViewModel : BaseNotifyable, IViewModel {

    private UserDetail _NewUser;

    public UserDetail NewUser {
      get { return _NewUser; }
      set { SetProperty<UserDetail>(ref _NewUser, value); }
    }

    public void LoadData(object customdata) {
      NewUser = (UserDetail)customdata;
    }
  }
}
