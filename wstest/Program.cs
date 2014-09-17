using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wstest.ServiceReference1;

namespace wstest
{
  class Program
  {
    static void Main(string[] args)
    {

      ServiceReference1.JolTudomEWSClient client = new ServiceReference1.JolTudomEWSClient();


      //client.NewUser(null, "ivanadm", null, "Horvath", null, "Ivan", "qqqqqqqq", 5);

      LoggedInUser liu;
      string token = client.Login(out liu, "Student1", "12345678");

      //client.NewUser(token, "tanar2", null, "Pista", null, "Tanar 2", "aaaaaaaa", 2);

      var testlist = client.StartTest(token, 10, new int[] { 1,2 });

      //client.GetStatistics(token);

      client.Close();
    }
  }
}
