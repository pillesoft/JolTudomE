using JolTudomE_WP.Helper;
using JolTudomE_WP.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JolTudomE_WP {

  class UnauthorizedException : Exception { }
  
  class ApiModelException : Exception {
    public ApiModelException(string apierror) : base(apierror) { }
  }

  class ApiModelError : Dictionary<string, List<string>> {
    public string FormattedError {
      get {
        StringBuilder err = new StringBuilder();
        foreach (var item in this) {
          err.AppendLine(string.Format("Field Name: {0}", item.Key));
          foreach (var value in item.Value) {
            err.AppendLine(string.Format("--->{0}", value));
          }
        }
        return err.ToString();
      }
    }
  }

  class WebAPIManager {
    private string _Token;

    private const string WEBAPIROOT = "http://localhost:1854";

    private string GetResponse(HttpWebResponse webresp) {
      using (Stream dataStream = webresp.GetResponseStream()) {
        using (StreamReader reader = new StreamReader(dataStream)) {
          string responseFromServer = reader.ReadToEnd();
          return responseFromServer;
        }
      }
    }

    private async Task<string> DoRequest(string url) {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", _Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      string responseFromServer = string.Empty;

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          responseFromServer = GetResponse(httpresp);
        }
        return responseFromServer;
      }
      catch (WebException wexc) {
        _Token = string.Empty;
        DataSource.LoggedInInfo = null;
        if (((HttpWebResponse)wexc.Response).StatusCode == HttpStatusCode.Unauthorized) {
          // that means the session is expired
          ((App)App.Current).SessionExpired();
          return null;
          // throw new UnauthorizedException();
        }
        else if (((HttpWebResponse)wexc.Response).StatusCode == HttpStatusCode.InternalServerError) {
          string modelerrors = GetResponse((HttpWebResponse)wexc.Response);
          var errdetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(modelerrors);
          throw new ApiModelException(ExceptionHandler.GetUserFriendlyErrorMessage(errdetails["ExceptionMessage"]));
        }
        else throw;
      }

    }

    internal async Task<string> Login(string username, string password) {
      string fullurl = string.Format("{0}/{1}", WEBAPIROOT, "api/account/login");
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);

      request.Method = "POST";
      string basicauth = username + ":" + password;
      basicauth = Convert.ToBase64String(Encoding.UTF8.GetBytes(basicauth));
      request.Headers["Authorization"] = "Basic " + basicauth;

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;
        string responseFromServer = string.Empty;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          string cookie = httpresp.Headers["Set-Cookie"];
          _Token = cookie.Split('=')[1];

          responseFromServer = GetResponse(httpresp);
          response.Dispose();

        }
        else {
          _Token = string.Empty;
        }

        return responseFromServer;
      }
      catch (WebException wexc) {
        _Token = string.Empty;
        if (((HttpWebResponse)wexc.Response).StatusCode == HttpStatusCode.Unauthorized) throw new UnauthorizedException();
        else throw;
      }
    }

    internal async Task<bool> Register(UserDetail newuser) {
      string fullurl = string.Format("{0}/{1}", WEBAPIROOT, "api/account/addstudent");
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);

      request.Method = "POST";
      request.ContentType = "application/json; charset=utf-8";
      request.Accept = "application/json";

      string postData = JsonConvert.SerializeObject(newuser);
      byte[] byteArray = Encoding.UTF8.GetBytes(postData);
      Stream dataStream = await request.GetRequestStreamAsync();
      dataStream.Write(byteArray, 0, byteArray.Length);
      dataStream.Dispose();

      // send it
      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;
        string responseFromServer = string.Empty;

        if (httpresp.StatusCode == HttpStatusCode.Created) {
          response.Dispose();
        }

        return true;
      }
      catch (WebException wexc) {
        if (((HttpWebResponse)wexc.Response).StatusCode == HttpStatusCode.BadRequest) {
          string modelerrors = GetResponse((HttpWebResponse)wexc.Response);
          var errdetails = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<Dictionary<string, string>>>>>(modelerrors);
          ApiModelError apierr = new ApiModelError();
          foreach (var field in errdetails) {
            var errlist = field.Value["_errors"].FindAll(e => e.ContainsKey("<ErrorMessage>k__BackingField"));
            List<string> errmesslist = new List<string>();
            foreach (var errdet in errlist) {
              string errmsg = string.Empty;
              if (errdet.TryGetValue("<ErrorMessage>k__BackingField", out errmsg)) errmesslist.Add(errmsg);
            }
            apierr.Add(field.Key, errmesslist);
          }
          throw new ApiModelException(apierr.FormattedError);
        }
        else if (((HttpWebResponse)wexc.Response).StatusCode == HttpStatusCode.InternalServerError) {
          string modelerrors = GetResponse((HttpWebResponse)wexc.Response);
          var errdetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(modelerrors);
          throw new ApiModelException(ExceptionHandler.GetUserFriendlyErrorMessage(errdetails["ExceptionMessage"]));
        }
        else throw;
      }

    }

    internal async Task<string> GetLoginDetail() {
      string fullurl = string.Format("{0}/{1}", WEBAPIROOT, "api/account/detail");
      string response = string.Empty;
      try {
        response = await DoRequest(fullurl);
      }
      catch {
        throw;
      }
      return response;
    }

    internal async Task<string> GetUserList(int? roletosearch) {
      string fullurl = string.Empty;
      if (roletosearch == null) {
        fullurl = string.Format("{0}/{1}", WEBAPIROOT, "api/account/searchbyrole");
      }
      else {
        fullurl = string.Format("{0}/{1}/{2}", WEBAPIROOT, "api/account/searchbyrole/roletosearch");
      }
      string response = string.Empty;
      try {
        response = await DoRequest(fullurl);
      }
      catch {
        throw;
      }
      return response;
    }

    internal async Task<string> GetStatistics(int personid) {
      string fullurl = string.Format("{0}/{1}/{2}", WEBAPIROOT, "api/test/statistic", personid);

      string response = string.Empty;
      try {
        response = await DoRequest(fullurl);
      }
      catch {
        throw;
      }
      return response;
    }

    internal async Task<string> GetTestDetails(int testid, int personid) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}", WEBAPIROOT, "api/test/detail", testid, personid);

      string response = string.Empty;
      try {
        response = await DoRequest(fullurl);
      }
      catch {
        throw;
      }
      return response;
    }

    internal async Task<string> GetCourses() {
      string fullurl = string.Format("{0}/{1}", WEBAPIROOT, "api/course/courses");

      string response = string.Empty;
      try {
        response = await DoRequest(fullurl);
      }
      catch {
        throw;
      }
      return response;
    }

    internal async Task<string> GetTopics(int courseid) {
      string fullurl = string.Format("{0}/{1}/{2}", WEBAPIROOT, "api/course/topic", courseid);

      string response = string.Empty;
      try {
        response = await DoRequest(fullurl);
      }
      catch {
        throw;
      }
      return response;
    }

    internal async Task<string> StartNewTest(int personid, int count, List<int> topicids) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}/{4}", WEBAPIROOT, "api/test/start", personid, count, string.Join(",", topicids));

      string response = string.Empty;
      try {
        response = await DoRequest(fullurl);
      }
      catch {
        throw;
      }
      return response;
    }

    internal async Task AnswerTest(int testid, int questionid, int answerid) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}/{4}", WEBAPIROOT, "api/test/answer", testid, questionid, answerid);

      try {
        await DoRequest(fullurl);
      }
      catch {
        throw;
      }
    }

    internal async Task CompleteTest(int testid, int questionid, int answerid) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}/{4}", WEBAPIROOT, "api/test/complete", testid, questionid, answerid);

      try {
        await DoRequest(fullurl);
      }
      catch {
        throw;
      }
    }

    internal async Task CancelTest(int testid, int personid) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}", WEBAPIROOT, "api/test/cancel", testid, personid);

      try {
        await DoRequest(fullurl);
      }
      catch {
        throw;
      }

    }

  }
}
