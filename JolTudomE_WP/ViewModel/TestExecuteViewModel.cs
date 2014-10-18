using JolTudomE_WP.Common;
using JolTudomE_WP.Model;
using System.Collections.Generic;

namespace JolTudomE_WP.ViewModel {
  public class TestExecuteViewModel : BaseNotifyable, IViewModel {

    private IEnumerator<NewTestQuestion> _TestQuestions;

    private NewTest _NewTest;
    public NewTest NewTest {
      get { return _NewTest; }
      set {
        SetProperty<NewTest>(ref _NewTest, value);
        _TestQuestions = _NewTest.Questions.GetEnumerator();
        
        _TestQuestions.MoveNext();
        ShowNewQuestion();
      }
    }

    private NewTestQuestion _CurrentQuestion;
    public NewTestQuestion CurrentQuestion {
      get { return _CurrentQuestion; }
      set {
        SetProperty<NewTestQuestion>(ref _CurrentQuestion, value);
      }
    }

    private string _TestQuestion;
    public string TestQuestion {
      get { return _TestQuestion; }
      set {
        SetProperty<string>(ref _TestQuestion, value);
      }
    }

    private string[] _Answers;

    public string[] Answers {
      get { return _Answers; }
      set { SetProperty<string[]>(ref _Answers, value); }
    }

    private int _CheckedAnswer;

    public int CheckedAnswer {
      get { return _CheckedAnswer; }
      set { SetProperty<int>(ref _CheckedAnswer, value); }
    }


    private RelayCommand _NextQuestionCommand;

    public RelayCommand NextQuestionCommand {
      get {
        return _NextQuestionCommand
      ?? (_NextQuestionCommand = new RelayCommand(
      async () => {
        await DataSource.AnswerTest(NewTest.TestID, CurrentQuestion.QuestionID, CurrentQuestion.Answers[CheckedAnswer - 1].AnswerID);
        _TestQuestions.MoveNext();
        ShowNewQuestion();
      },
      () => CanGoNext()));
      }
    }


    public TestExecuteViewModel() {
      
      /* DESIGN TIME DATA
      TestQuestion = "1./15. Ez az elso kerdes";
      Answers = new string[] { "Válasz 1", "Válasz 2", "Válasz 3", "Válasz 4" };
      */

      CheckedAnswer = 3;
    }

    private bool CanGoNext() {
      return NewTest != null && 
        NewTest.Questions.IndexOf(_TestQuestions.Current) + 1 != NewTest.Questions.Count;
    }

    private void ShowNewQuestion() {
      CurrentQuestion = _TestQuestions.Current;
      TestQuestion = string.Format("{0}./{1}. {2}", NewTest.Questions.IndexOf(CurrentQuestion) + 1, NewTest.Questions.Count, CurrentQuestion.QuestionText);
      Answers = new string[] { 
        CurrentQuestion.Answers[0].AnswerText, 
        CurrentQuestion.Answers[1].AnswerText, 
        CurrentQuestion.Answers[2].AnswerText, 
        CurrentQuestion.Answers[3].AnswerText 
      };

      CheckedAnswer = 0;
      NextQuestionCommand.RaiseCanExecuteChanged();
    }

    public async void LoadData(object customdata) {
      NewTestParam p = (NewTestParam)customdata;

      var result = await DataSource.GenerateTest(DataSource.LoggedInInfo.PersonID, p.NumberOfQuestions, p.TopicIDs);
      NewTest = result;

    }
  }
}
