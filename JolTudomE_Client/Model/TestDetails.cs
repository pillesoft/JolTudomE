using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JolTudomE_Client.JolTudomEWSService {
  public partial class TestDetails {
    public bool IsCorrect {
      get {
        return ChekedAnswer == CorrectAnswer;
      }
    }
  }
}
