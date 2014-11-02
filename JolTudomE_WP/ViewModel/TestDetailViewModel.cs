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

    public TestDetailViewModel() { }

    public async void LoadData(object customdata) {
      try {
        var result = await DataSource.GetTestDetail((int)customdata);
        TestDetList = result;
      }
      catch { }
    }
  }
}
