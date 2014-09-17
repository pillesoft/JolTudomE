using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JolTudomE_Client.Model;

namespace JolTudomE_Client.JolTudomEWSService {
  public partial class PersonDetails {

    public string TreeDisplayName {
      get {
        return string.Format("{0}, {1}", LastName, FirstName);
      }
    }

    public PersonRoleEnum RoleEnum {
      get {
        return (PersonRoleEnum)RoleID;
      }
    }

    private bool _IsSelected;
    public bool IsSelected {
      get { return _IsSelected; }
      set {
        _IsSelected = value;
        RaisePropertyChanged("IsSelected");
      }
    }

    private bool _IsExpanded;
    public bool IsExpanded {
      get { return _IsExpanded; }
      set {
        _IsExpanded = value;
        RaisePropertyChanged("IsExpanded");
      }
    }

  }
}
