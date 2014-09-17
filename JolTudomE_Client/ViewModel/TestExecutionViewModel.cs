using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JolTudomE_Client.JolTudomEWSService;
using JolTudomE_Client.Message;
using JolTudomE_Client.Model;

namespace JolTudomE_Client.ViewModel {

  public class TestExecutionViewModel : BaseViewModel {

    public int NumberOfQuestions { get; set; }

    private IEnumerator<NewTestQuestion> _TestQuestions;
    private TestExecutionInit _InitData;

    private NewTest _NewTest;
    public NewTest NewTest {
      get { return _NewTest; }
      set {
        _NewTest = value;
        RaisePropertyChanged<NewTest>(() => this.NewTest);
        _TestQuestions = _NewTest.Questions.GetEnumerator();

        _TestQuestions.MoveNext();
        ShowNewQuestion();
      }
    }

    private NewTestQuestion _CurrentQuestion;
    public NewTestQuestion CurrentQuestion {
      get { return _CurrentQuestion; }
      set {
        _CurrentQuestion = value;
        RaisePropertyChanged<NewTestQuestion>(() => this.CurrentQuestion);
      }
    }

    private string _TestQuestion;
    public string TestQuestion {
      get { return _TestQuestion; }
      set {
        _TestQuestion = value;
        RaisePropertyChanged<string>(() => this.TestQuestion);
      }
    }

    private string _Answer1;
    public string Answer1 {
      get { return _Answer1; }
      set {
        _Answer1 = value;
        RaisePropertyChanged<string>(() => this.Answer1);
      }
    }


    private string _Answer2;
    public string Answer2 {
      get { return _Answer2; }
      set {
        _Answer2 = value;
        RaisePropertyChanged<string>(() => this.Answer2);
      }
    }


    private string _Answer3;
    public string Answer3 {
      get { return _Answer3; }
      set {
        _Answer3 = value;
        RaisePropertyChanged<string>(() => this.Answer3);
      }
    }


    private string _Answer4;
    public string Answer4 {
      get { return _Answer4; }
      set {
        _Answer4 = value;
        RaisePropertyChanged<string>(() => this.Answer4);
      }
    }


    private int _CheckedAnswer;
    public int CheckedAnswer {
      get { return _CheckedAnswer; }
      set {
        _CheckedAnswer = value;
        RaisePropertyChanged<int>(() => this.CheckedAnswer);
      }
    }


    private RelayCommand _NextQuestionCommand;
    public RelayCommand NextQuestionCommand {
      get {
        if (_NextQuestionCommand == null) {
          _NextQuestionCommand = new RelayCommand(() => {
            // add implement in here:

            if (CheckedAnswer != 0) {
              int answerid = _TestQuestions.Current.Answers[CheckedAnswer - 1].AnswerID;
              try {
                _DataService.CheckAnswer(_DataService.LoggedInUser.PersonID, NewTest.TestID, _TestQuestions.Current.QuestionID, answerid, false);
              }
              catch (SessionExpiredException) {
                MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
              }
            }

            _TestQuestions.MoveNext();
            ShowNewQuestion();
          }
          , () => {
            // check here if the current question is the last
            return _TestQuestions != null && _TestQuestions.Current != null
              && _TestQuestions.Current.QuestionID != _NewTest.Questions.Last().QuestionID;
          });
        }
        return _NextQuestionCommand;
      }
    }


    private RelayCommand _CancelTestCommand;
    public RelayCommand CancelTestCommand {
      get {
        if (_CancelTestCommand == null) {
          _CancelTestCommand = new RelayCommand(() => {
            // add implement in here:
            var dwmess = new ShowDialogMessage();
            dwmess.Title = "Teszt Megszakítás";
            dwmess.Text = "Ha megszakítod a tesztet, elvesznek az eddigi válaszok. \nBiztosan megszakítod a tesztet?";
            dwmess.Buttons = new DialogButton[] { 
              new DialogButton{IsDefault = true, Title = "Nem", ReturnValue=false},
              new DialogButton{Title = "Igen", ReturnValue=true}
            };

            dwmess.ExecuteIfTrue = () => {
              try {
                _DataService.CancelTest(NewTest.TestID, _DataService.LoggedInUser.PersonID);
              }
              catch (SessionExpiredException) {
                MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
              }
            };

            MessengerInstance.Send<ShowDialogMessage>(dwmess);
          });
        }
        return _CancelTestCommand;
      }
    }


    private RelayCommand _FinishTestCommand;
    public RelayCommand FinishTestCommand {
      get {
        if (_FinishTestCommand == null) {
          _FinishTestCommand = new RelayCommand(() => {
            // add implement in here:
            int? answerid = null;
            if (CheckedAnswer != 0) {
              answerid = _TestQuestions.Current.Answers[CheckedAnswer - 1].AnswerID;
            }
            try {
              _DataService.CheckAnswer(_DataService.LoggedInUser.PersonID, NewTest.TestID, _TestQuestions.Current.QuestionID, answerid, true);
            }
            catch (SessionExpiredException) {
              MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
            }
          });
        }
        return _FinishTestCommand;
      }
    }
      

    private void ShowNewQuestion() {
      NewTestQuestion currquest = _TestQuestions.Current;
      TestQuestion = string.Format("{0}./{1}. {2}", NewTest.Questions.IndexOf(currquest) + 1, NewTest.Questions.Count, currquest.QuestionText);
      Answer1 = currquest.Answers[0].AnswerText;
      Answer2 = currquest.Answers[1].AnswerText;
      Answer3 = currquest.Answers[2].AnswerText;
      Answer4 = currquest.Answers[3].AnswerText;

      CheckedAnswer = 0;
    }

    public override bool CanCloseWindowConfirmation() {
      bool canclose = false;

      ShowDialogMessage sdm = new ShowDialogMessage();
      sdm.Title = "Kilépés a Rendszerből";
      sdm.Text = "Amennyiben kilép a rendszerből a jelenlegi teszt végrehajtás megszakad.\nBiztosan kilép?";
      sdm.Buttons = new DialogButton[] { 
          new DialogButton{Title="Kilépek", IsDefault=false, ReturnValue=true},
          new DialogButton{Title="Maradok", IsDefault=true, ReturnValue=false},
        };
      sdm.ExecuteIfTrue = () => {
        try {
          _DataService.CancelTest((int)App.Current.Properties["CurrentTestID"], _DataService.LoggedInUser.PersonID);
        }
        catch (SessionExpiredException) {
          // hm... what to do?
        }
        _DataService.ClearSession();
        canclose = true;
      };
      sdm.ExecuteIfFalse = () => {
        canclose = false;
      };

      MessengerInstance.Send<ShowDialogMessage>(sdm);

      return canclose;
    }

    protected override void LoadData() {
      try {
        NewTest = _DataService.GetNewTest(_InitData.NumberOfQuestions, _InitData.TopicIDs);
        NextQuestionCommand.RaiseCanExecuteChanged();
        MessengerInstance.Send<TestExecutionRunningMessage>(new TestExecutionRunningMessage { IsTestRunning = true });
      }
      catch (SessionExpiredException) {
        MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Login, UserState = "SessionExpired" });
      }
      catch (TranslatedException texc) {
        MessengerInstance.Send<NavigationMessage>(new NavigationMessage { View = ViewEnum.Student, UserState = new string[] { "TestCreateFailed", texc.Message } });
      }
    }

    public TestExecutionViewModel(WSManager ds, TestExecutionInit testinitdata)
      : base(ds) {

      if (IsInDesignMode) {
        TestQuestion = "Ez egy jo kis kerdes?";

        Answer1 = "Valasz 1";
        Answer2 = "Valasz 2";
        Answer3 = "Valasz 3";
        Answer4 = "Valasz 4";

        CheckedAnswer = 0;
      }
      else {
        _InitData = testinitdata;
      }
    }

  }

}
