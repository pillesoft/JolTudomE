using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JolTudomE_Client.Message {
  public enum AdminViewEnum {
    Course,
    Topic,
    Question,
    NewPerson
  }

  public class AdmNavigationMessage {
    public AdminViewEnum View { get; set; }
    /// <summary>
    /// could be any type of data, should be passed to the navigation stuff
    /// </summary>
    public object UserState { get; set; }

  }
}
