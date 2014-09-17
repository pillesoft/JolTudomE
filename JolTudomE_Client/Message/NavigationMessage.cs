using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JolTudomE_Client.ViewModel;

namespace JolTudomE_Client.Message {
  public enum ViewEnum {
    Login,
    Student,
    NewPerson,
    TestExecution,
    Admin
  }

  public class NavigationMessage {
    public ViewEnum View { get; set; }
    /// <summary>
    /// could be any type of data, should be passed to the navigation stuff
    /// </summary>
    public object UserState { get; set; }
  }
}
