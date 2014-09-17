using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JolTudomE_Client.Message {
  public class DialogButton {
    public string Title { get; set; }
    public bool IsDefault { get; set; }
    public bool ReturnValue { get; set; }
  }

  public class ShowDialogMessage {
    public string Title { get; set; }
    public string Text { get; set; }
    public DialogButton[] Buttons { get; set; }

    public Action ExecuteIfTrue { get; set; }
    public Action ExecuteIfFalse { get; set; }

    public ShowDialogMessage() {
      // dummy empty Action methods - if they're not set
      ExecuteIfFalse = () => { };
      ExecuteIfTrue = () => { };
    }
  }
}
