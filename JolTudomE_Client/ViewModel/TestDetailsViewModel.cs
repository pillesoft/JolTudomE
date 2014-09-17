using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JolTudomE_Client.JolTudomEWSService;

namespace JolTudomE_Client.ViewModel {
  public class TestDetailsViewModel : BaseCWViewModel<List<TestDetails>> {

    public TestDetailsViewModel(WSManager ds):base(ds) {
      if (IsInDesignMode) {
        SetInstance(new List<TestDetails>());
        Instance.Add(new TestDetails { QuestionText = "kerdes", ChekedAnswer = "jo valasz", CorrectAnswer = "jo valasz" });
        Instance.Add(new TestDetails { QuestionText = "kerdes", ChekedAnswer = "valasz", CorrectAnswer = "rossz valasz" });
      }
    }

  }
}
