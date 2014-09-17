using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JolTudomE_Client.Model {

  public class NewPerson : BaseModel {

    #region properties

    private string _UserName;
    [StringLength(8, MinimumLength=5, ErrorMessage="Felhasználó neve 5 és 8 karakter között kell legyen")]
    public string UserName {
      get { return _UserName; }
      set {
        _UserName = value;
        ValidateProperty(_UserName, "UserName");
        OnPropertyChanged("UserName");
      }
    }

    private string _Prefix;
    public string Prefix {
      get { return _Prefix; }
      set {
        _Prefix = value;
        OnPropertyChanged("Prefix");
      }
    }

    private string _FirstName;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Ki kell tölteni a Keresztnevet")]
    public string FirstName {
      get { return _FirstName; }
      set {
        _FirstName = value;
        ValidateProperty(_FirstName, "FirstName");
        OnPropertyChanged("FirstName");
      }
    }

    private string _MiddleName;
    public string MiddleName {
      get { return _MiddleName; }
      set {
        _MiddleName = value;
        OnPropertyChanged("MiddleName");
      }
    }

    private string _LastName;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Ki kell tölteni a Család nevet")]
    public string LastName {
      get { return _LastName; }
      set {
        _LastName = value;
        ValidateProperty(_LastName, "LastName");
        OnPropertyChanged("LastName");
      }
    }

    private string _Password;
    [StringLength(50, MinimumLength = 8, ErrorMessage = "Jelszó minimum 8 karakter kell legyen")]
    public string Password {
      get { return _Password; }
      set {
        _Password = value;
        ValidateProperty(_Password, "Password");
        OnPropertyChanged("Password");
      }
    }

    private string _Role;
    [Required(AllowEmptyStrings = false, ErrorMessage = "Csoportot kell választani")]
    public string Role {
      get { return _Role; }
      set {
        _Role = value;
        ValidateProperty(_Role, "Role");
        OnPropertyChanged("Role");
      }
    }

    public int RoleId {
      get {
        switch (Role) {
          case "Diák":
            return 1;
          case "Tanár":
            return 2;
          case "Admin":
            return 3;
          default:
            return 0;
        }
      }
    }

    #endregion

    // force using the static Create method
    private NewPerson() { }
    
    public static NewPerson Create() {
      return new NewPerson { UserName = "", Password = "", FirstName = "", LastName = "", Role = "Diák" };
    }

  }
}
