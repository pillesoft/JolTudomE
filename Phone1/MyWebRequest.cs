using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phone1 {
  class MyWebRequest:WebRequest {
    public override WebHeaderCollection Headers {
      get {
        return base.Headers;
      }
      set {
        base.Headers = value;
      }
    }
  }
}
