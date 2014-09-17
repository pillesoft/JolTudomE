using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace JolTudomE_Client.Model {
  public class BaseNotifyableModel : INotifyPropertyChanged {
    #region Notification

    public virtual void OnPropertyChanged(string propname) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propname));
      }
    }

    public virtual event PropertyChangedEventHandler PropertyChanged;

    #endregion
  }
}
