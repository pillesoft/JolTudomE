using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JolTudomE_WS.Security;

namespace JolTudomE_WS.DAL {
  public partial class Session {

    public JolTudomERoles UserRole {
      get { return (JolTudomERoles)RoleID; }
    }

  }
}