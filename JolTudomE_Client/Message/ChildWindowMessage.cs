using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JolTudomE_Client.Message {
  public enum DialogAction { 
    Open,
    Close
  }

  public class ChildWindowMessage {
    public DialogAction Action { get; set; }
    public object ModelInstance { get; set; }
    public object UserState { get; set; }

    public Action ExecuteAfterClose { get; set; }

    public ChildWindowMessage() {
      ExecuteAfterClose = () => { };
    }
  }
}
