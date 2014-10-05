using System;

namespace JolTudomE_WP.Model {
  public class Statistic : BaseNotifyable {

    private int _CorrectAnswer;
    private DateTime _Generated;
    private Nullable<decimal> _Percent;
    private Nullable<int> _Questions;
    private int _TestID;
    private Nullable<TimeSpan> _TotalTime;

    public int CorrectAnswer {
      get { return this._CorrectAnswer; }
      set { _CorrectAnswer = value; }
    }
    public DateTime Generated {
      get { return this._Generated; }
      set { _Generated = value; }
    }
    public Nullable<decimal> Percent {
      get { return _Percent; }
      set { _Percent = value; }
    }
    public Nullable<int> Questions {
      get { return _Questions; }
      set { _Questions = value; }
    }
    public int TestID {
      get { return _TestID; }
      set { _TestID = value; }
    }
    public Nullable<TimeSpan> TotalTime {
      get { return _TotalTime; }
      set { _TotalTime = value; }
    }

    public string NumberOfQuestAnsw {
      get { return string.Format("{0}/{1}", CorrectAnswer, Questions); }
    }

    public string PercentString {
      get { return string.Format("{0:P}", Percent); }
    }
  }

}

