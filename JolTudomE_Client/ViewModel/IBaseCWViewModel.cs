using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JolTudomE_Client.ViewModel {
  public interface IBaseCWViewModel<T> {
    void SetInstance(T inst);
  }
}
