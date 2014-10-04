using Phone1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phone1 {
  public class WepAPIManager {
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
          // Display the content.
          string sa = "as";
          // Clean up the streams.
          reader.Close();
          dataStream.Close();
          response.Close();

          return true;
        }
        else {
          Token = string.Empty;
          return false;
        }
      }
      catch (WebException) {
        Token = string.Empty;
        return false;
      }
    }

    public async void GetStatistics() {
      string fullurl = string.Format("{0}/{1}", WEBAPIROOT, "api/test/statistic");
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
          Stream dataStream = httpresp.GetResponseStream();
          // Open the stream using a StreamReader for easy access.
          StreamReader reader = new StreamReader(dataStream);
          // Read the content.
          string responseFromServer = reader.ReadToEnd();
          // Display the content.
          string sa = "as";
          // Clean up the streams.
          reader.Close();
          dataStream.Close();
          response.Close();
        }
        else {
        }
      }
      catch (WebException) {
        string sa = "SDFg";
      }


    }

  }
}
