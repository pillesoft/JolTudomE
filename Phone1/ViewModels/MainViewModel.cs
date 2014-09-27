using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Phone1.Resources;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace Phone1.ViewModels {
  public class MainViewModel : INotifyPropertyChanged {
    public MainViewModel() {
      this.Items = new ObservableCollection<ItemViewModel>();
    }

    /// <summary>
    /// A collection for ItemViewModel objects.
    /// </summary>
    public ObservableCollection<ItemViewModel> Items { get; private set; }

    private string _sampleProperty = "Sample Runtime Property Value";
    /// <summary>
    /// Sample ViewModel property; this property is used in the view to display its value using a Binding
    /// </summary>
    /// <returns></returns>
    public string SampleProperty {
      get {
        return _sampleProperty;
      }
      set {
        if (value != _sampleProperty) {
          _sampleProperty = value;
          NotifyPropertyChanged("SampleProperty");
        }
      }
    }

    /// <summary>
    /// Sample property that returns a localized string
    /// </summary>
    public string LocalizedSampleProperty {
      get {
        return AppResources.SampleProperty;
      }
    }

    public bool IsDataLoaded {
      get;
      private set;
    }

    /// <summary>
    /// Creates and adds a few ItemViewModel objects into the Items collection.
    /// </summary>
    public async void LoadData() {

      HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:52323/RestService.svc/Auth");
      // Set the Method property of the request to POST.
      request.Method = "POST";
      // Create POST data and convert it to a byte array.
      string postData = "This is a test that posts this string to a Web server.";
      byte[] byteArray = Encoding.UTF8.GetBytes(postData);
      // Set the ContentType property of the WebRequest.
      request.ContentType = "application/x-www-form-urlencoded";
      // Set the ContentLength property of the WebRequest.
      request.ContentLength = byteArray.Length;
      // Get the request stream.
      Stream dataStream = await request.GetRequestStreamAsync();
      // Write the data to the request stream.
      dataStream.Write(byteArray, 0, byteArray.Length);
      // Close the Stream object.
      dataStream.Close();
      // Get the response.
      WebResponse response = await request.GetResponseAsync();
      // Display the status.
      Console.WriteLine(((HttpWebResponse)response).StatusDescription);
      // Get the stream containing content returned by the server.
      dataStream = response.GetResponseStream();
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



      /*
      HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(new Uri("http://localhost:52323/RestService.svc/Login/Ivanka/12345678"));
      wr.Method = "GET";
      wr.Accept = "Accept=application/json";

      WebResponse response = await wr.GetResponseAsync();
      String jsonResponse = string.Empty;

      using (StreamReader sr = new StreamReader(response.GetResponseStream())) {

        jsonResponse = sr.ReadToEnd();
        string sa = "sdf";
      }
      */
      //WebClient wc = new WebClient();
      //wc.Headers["Accept"] = "application/json";
      //wc.DownloadStringAsync(new Uri("http://localhost:52323/RestService.svc/Login/Ivanka/12345678"));
      //wc.DownloadStringCompleted += (s, e) => {
      //  string sa = "Sfdg";
      //  this.Items.Add(new ItemViewModel() { ID = "0", LineOne = "result", LineTwo = e.Result });
      //  this.Items.Add(new ItemViewModel() { ID = "1", LineOne = "result1", LineTwo = e.Result });
      //};
      // Sample data; replace with real data
      //this.Items.Add(new ItemViewModel() { ID = "0", LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
      //this.Items.Add(new ItemViewModel() { ID = "1", LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
      //this.Items.Add(new ItemViewModel() { ID = "2", LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
      //this.Items.Add(new ItemViewModel() { ID = "3", LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
      //this.Items.Add(new ItemViewModel() { ID = "4", LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
      //this.Items.Add(new ItemViewModel() { ID = "5", LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
      //this.Items.Add(new ItemViewModel() { ID = "6", LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
      //this.Items.Add(new ItemViewModel() { ID = "7", LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
      //this.Items.Add(new ItemViewModel() { ID = "8", LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
      //this.Items.Add(new ItemViewModel() { ID = "9", LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
      //this.Items.Add(new ItemViewModel() { ID = "10", LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
      //this.Items.Add(new ItemViewModel() { ID = "11", LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
      //this.Items.Add(new ItemViewModel() { ID = "12", LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
      //this.Items.Add(new ItemViewModel() { ID = "13", LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
      //this.Items.Add(new ItemViewModel() { ID = "14", LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
      //this.Items.Add(new ItemViewModel() { ID = "15", LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

      this.IsDataLoaded = true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(String propertyName) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (null != handler) {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}