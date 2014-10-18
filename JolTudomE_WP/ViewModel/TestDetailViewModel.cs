using JolTudomE_WP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JolTudomE_WP.ViewModel {
  public class TestDetailViewModel : BaseNotifyable, IViewModel {

    private List<TestDetail> _TestDetList;

    public List<TestDetail> TestDetList {
      get { return _TestDetList; }
      set { SetProperty<List<TestDetail>>(ref _TestDetList, value); }
    }

    public TestDetailViewModel() {
      TestDetail td = new TestDetail {
        ChekedAnswer="Válasz", 
        CorrectAnswer = "Jó válasz",
        QuestionText = "Ez meg volt a kerdes?"
      };
      TestDetail td1 = new TestDetail {
        ChekedAnswer = "Jó válasz",
        CorrectAnswer = "Jó válasz",
        QuestionText = "Meg egy másik kerdes?"
      };
      TestDetList = new List<TestDetail>();
      TestDetList.Add(td);
      TestDetList.Add(td1);
        
      
    }

    public async void LoadData(object customdata) {
      var param = customdata as TestDetailParam;

      var result = await DataSource.GetTestDetail(param.TestID, param.PersonID);
      TestDetList = result;

    }
  }
}
