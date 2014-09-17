using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace JolTudomE_Client.Model
{
  public enum ModelState
  {
    None,
    New,
    Modified,
    Deleted
  }

  public class BaseModel : BaseNotifyableModel, IDataErrorInfo
  {
    private ModelState _ModelState;

    public ModelState ModelState
    {
      get { return _ModelState; }
      set { _ModelState = value; }
    }

    public bool IsValid() {
      ValidationContext validationContext = new ValidationContext(this, null, null);
      var valresult = new List<ValidationResult>();
      bool isvalid = Validator.TryValidateObject(this, validationContext, valresult, true);
      return isvalid;
    }

    #region Notification

    public override void OnPropertyChanged(string propname)
    {
      if (_ModelState != Model.ModelState.New)
      {
        ModelState = Model.ModelState.Modified;
      }

      base.OnPropertyChanged(propname);

      //if (PropertyChanged != null)
      //{
      //  PropertyChanged(this, new PropertyChangedEventArgs(propname));
      //}
    }

    //public override event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region IDataErrorInfo

    public string Error {
      get { return null; }
    }

    public string this[string columnName] {
      get {
        string errormsg = string.Empty;
        if (_ValidationErrors.ContainsKey(columnName)) {
          ICollection<ValidationResult> valres = _ValidationErrors[columnName];

          errormsg = string.Join("\n", valres);
        }
        return errormsg;
      }
    }

    #endregion

    #region Validation

    private Dictionary<string, ICollection<ValidationResult>> _ValidationErrors = new Dictionary<string, ICollection<ValidationResult>>();

    protected void ValidateProperty(object value, string propertyName) {
      ValidationContext validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
      var valresult = new List<ValidationResult>();
      bool isvalid = Validator.TryValidateProperty(value, validationContext, valresult);
      if (!isvalid) {
        _ValidationErrors[propertyName] = valresult;
      }
      else {
        _ValidationErrors.Remove(propertyName);
      }
    }

    #endregion
  }
}
