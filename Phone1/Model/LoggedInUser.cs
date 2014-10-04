using System;
using System.Collections.Generic;
using System.Linq;

namespace Phone1.Models {
  public class LoggedInUser {
    public string UserName { get; set; }
    public int PersonID { get; set; }
    public int RoleID { get; set; }
    public string Token { get; set; }
    public string FullName { get; set; }

  }
}