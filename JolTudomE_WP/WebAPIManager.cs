using JolTudomE_WP.Model;
using JolTudomE_WP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JolTudomE_WP {
  public class WebAPIManager {
    public string Token { get; private set; }
    public LoggedInUser LoggedInUser { get; private set; }

    private const string WEBAPIROOT = "http://localhost:1854";

    public bool IsAuthenticated() {
      return !string.IsNullOrEmpty(Token);
    }

    public async Task<bool> Login(string username, string password) {
      string fullurl = string.Format("{0}/{1}", WEBAPIROOT, "api/account/login");
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);

      request.Method = "POST";
      string basicauth = username + ":" + password;
      basicauth = Convert.ToBase64String(Encoding.UTF8.GetBytes(basicauth));
      request.Headers["Authorization"] = "Basic " + basicauth;

      //// Create POST data and convert it to a byte array.
      //string postData = "This is a test that posts this string to a Web server.";
      //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
      //// Set the ContentType property of the WebRequest.
      //request.ContentType = "application/x-www-form-urlencoded";
      //// Set the ContentLength property of the WebRequest.
      //request.ContentLength = byteArray.Length;
      //// Get the request stream.
      //Stream dataStream = await request.GetRequestStreamAsync();
      //// Write the data to the request stream.
      //dataStream.Write(byteArray, 0, byteArray.Length);
      //// Close the Stream object.
      //dataStream.Close();

      // Get the response.
      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          string cookie = httpresp.Headers["Set-Cookie"];
          Token = cookie.Split('=')[1];

          Stream dataStream = httpresp.GetResponseStream();
          // Open the stream using a StreamReader for easy access.
          StreamReader reader = new StreamReader(dataStream);
          // Read the content.
          string responseFromServer = reader.ReadToEnd();

          LoggedInUser = JsonConvert.DeserializeObject<LoggedInUser>(responseFromServer);

          // Clean up the streams.
          reader.Dispose();
          dataStream.Dispose();
          response.Dispose();

          return true;
        }
        else {
          Token = string.Empty;
          LoggedInUser = null;
          return false;
        }
      }
      catch (WebException) {
        Token = string.Empty;
        LoggedInUser = null;
        return false;
      }
    }

    public async Task<List<Statistic>> GetStatistics(int personid) {
      string fullurl = string.Format("{0}/{1}/{2}", WEBAPIROOT, "api/test/statistic", personid);

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      List<Statistic> result = new List<Statistic>();

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          Stream dataStream = httpresp.GetResponseStream();
          // Open the stream using a StreamReader for easy access.
          StreamReader reader = new StreamReader(dataStream);
          // Read the content.
          string responseFromServer = reader.ReadToEnd();
          // Display the content.
          result = JsonConvert.DeserializeObject<List<Statistic>>(responseFromServer);

          // Clean up the streams.
          reader.Dispose();
          dataStream.Dispose();
          response.Dispose();
        }
      }
      catch (WebException) {
        return result;
      }

      return result;
    }

    public async Task<List<TestDetail>> GetTestDetails(int testid, int personid) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}", WEBAPIROOT, "api/test/detail", testid, personid);

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      List<TestDetail> result = new List<TestDetail>();

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          Stream dataStream = httpresp.GetResponseStream();
          // Open the stream using a StreamReader for easy access.
          StreamReader reader = new StreamReader(dataStream);
          // Read the content.
          string responseFromServer = reader.ReadToEnd();
          // Display the content.
          result = JsonConvert.DeserializeObject<List<TestDetail>>(responseFromServer);

          // Clean up the streams.
          reader.Dispose();
          dataStream.Dispose();
          response.Dispose();
        }
      }
      catch (WebException) {
        return result;
      }

      return result;
    }

    public async Task<ObservableCollection<Course>> GetCourses() {
      string fullurl = string.Format("{0}/{1}", WEBAPIROOT, "api/course/courses");

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      ObservableCollection<Course> result = new ObservableCollection<Course>();

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          Stream dataStream = httpresp.GetResponseStream();
          // Open the stream using a StreamReader for easy access.
          StreamReader reader = new StreamReader(dataStream);
          // Read the content.
          string responseFromServer = reader.ReadToEnd();
          // Display the content.
          result = JsonConvert.DeserializeObject<ObservableCollection<Course>>(responseFromServer);

          // Clean up the streams.
          reader.Dispose();
          dataStream.Dispose();
          response.Dispose();
        }
      }
      catch (WebException) {
        return result;
      }

      return result;
    }

    public async Task<ObservableCollection<Topic>> GetTopics(int courseid) {
      string fullurl = string.Format("{0}/{1}/{2}", WEBAPIROOT, "api/course/topic", courseid);

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      ObservableCollection<Topic> result = new ObservableCollection<Topic>();

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          Stream dataStream = httpresp.GetResponseStream();
          // Open the stream using a StreamReader for easy access.
          StreamReader reader = new StreamReader(dataStream);
          // Read the content.
          string responseFromServer = reader.ReadToEnd();
          // Display the content.
          result = JsonConvert.DeserializeObject<ObservableCollection<Topic>>(responseFromServer);

          // Clean up the streams.
          reader.Dispose();
          dataStream.Dispose();
          response.Dispose();
        }
      }
      catch (WebException) {
        return result;
      }

      return result;
    }

    public async Task<NewTest> StartNewTest(int personid, int count, List<int> topicids) {

      string fullurl = string.Format("{0}/{1}/{2}/{3}/{4}", WEBAPIROOT, "api/test/start", personid, count, string.Join(",", topicids));

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      NewTest result = new NewTest();

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          Stream dataStream = httpresp.GetResponseStream();
          // Open the stream using a StreamReader for easy access.
          StreamReader reader = new StreamReader(dataStream);
          // Read the content.
          string responseFromServer = reader.ReadToEnd();
          // Display the content.
          result = JsonConvert.DeserializeObject<NewTest>(responseFromServer);

          // Clean up the streams.
          reader.Dispose();
          dataStream.Dispose();
          response.Dispose();
        }
      }
      catch (WebException) {
        return result;
      }

      return result;
    }

    public async void AnswerTest(int testid, int questionid, int answerid) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}/{4}", WEBAPIROOT, "api/test/answer", testid, questionid, answerid);

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          response.Dispose();
        }
      }
      catch (WebException) {
      }

    }

    public async void CompleteTest(int testid, int questionid, int answerid) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}/{4}", WEBAPIROOT, "api/test/complete", testid, questionid, answerid);

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          response.Dispose();
        }
      }
      catch (WebException) {
      }

    }

    public async void CancelTest(int testid, int personid) {
      string fullurl = string.Format("{0}/{1}/{2}/{3}", WEBAPIROOT, "api/test/cancel", testid, personid);

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullurl);
      request.Method = "GET";
      request.Accept = "application/json";

      Cookie c = new Cookie("JolTudomEToken", Token);
      request.CookieContainer = new CookieContainer();
      request.CookieContainer.Add(new Uri(WEBAPIROOT), c);

      try {
        WebResponse response = await request.GetResponseAsync();
        HttpWebResponse httpresp = (HttpWebResponse)response;

        if (httpresp.StatusCode == HttpStatusCode.OK) {
          response.Dispose();
        }
      }
      catch (WebException) {
      }

    }

  }
}
