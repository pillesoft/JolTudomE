using JolTudomE_WP.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JolTudomE_WP.Models {
  public class LoggedInUser:BaseNotifyable {
    private string _UserName;

    public string UserName {
      get { return _UserName; }
      set { SetProperty<string>(ref _UserName, value); }
    }
    private int _PersonID;

    public int PersonID {
      get { return _PersonID; }
      set { SetProperty<int>(ref _PersonID, value); }
    }
    private int _RoleID;

    public int RoleID {
      get { return _RoleID; }
      set { SetProperty<int>(ref _RoleID, value); }
    }
    private string _Token;

    public string Token {
      get { return _Token; }
      set { SetProperty<string>(ref _Token, value); }
    }
    private string _FullName;

    public string FullName {
      get { return _FullName; }
      set { SetProperty<string>(ref _FullName, value); }
    }
  }
}