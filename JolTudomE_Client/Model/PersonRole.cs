using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using JolTudomE_Client.JolTudomEWSService;

namespace JolTudomE_Client.Model {
  public class PersonRole :BaseModel {
    public string RoleName { get; set; }
    public List<PersonDetails> Children { get; set; }

    private bool _IsSelected;
    //[CustomValidation(typeof(PersonRoleSelectedValidator), "IsRoleSelected")]
    public bool IsSelected {
      get { return _IsSelected; }
      set {
        _IsSelected = value;
        OnPropertyChanged("IsSelected");
        //ValidateProperty(_IsExpanded, "IsSelected");
      }
    }

    private bool _IsExpanded;
    public bool IsExpanded {
      get { return _IsExpanded; }
      set {
        _IsExpanded = value;
        OnPropertyChanged("IsExpanded");
      }
    }
    
  }

  //public static class PersonRoleSelectedValidator {
  //  public static ValidationResult IsRoleSelected(bool newvalue, ValidationContext ctx) {
  //    if (newvalue) {
  //      return new ValidationResult("Kérlek, válassz Felhasználót, ne csoportot!");
  //    }
  //    else {
  //      return ValidationResult.Success;
  //    }
  //  }
  //}
}
