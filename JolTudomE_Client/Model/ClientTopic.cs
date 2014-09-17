using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using JolTudomE_Client.JolTudomEWSService;

namespace JolTudomE_Client.Model {
  public class ClientTopic : BaseModel {
    private int _ID;

    public int ID {
      get { return _ID; }
      set {
        _ID = value;
        OnPropertyChanged("ID");
      }
    }

    private string _Name;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Témakör neve kötelező")]
    [StringLength(50, ErrorMessage = "Maximum 50 karakter lehet a Témakör neve")]
    public string Name {
      get { return _Name; }
      set {
        _Name = value;
        ValidateProperty(Name, "Name");
        OnPropertyChanged("Name");
      }
    }

    private string _Description;

    [StringLength(255, ErrorMessage = "Maximum 255 karakter lehet a Témakör leírása")]
    public string Description {
      get { return _Description; }
      set {
        _Description = value;
        ValidateProperty(_Description, "Description");
        OnPropertyChanged("Description");
      }
    }

    private int _CourseID;

    public int CourseID {
      get { return _CourseID; }
      set { _CourseID = value; }
    }

    private ClientTopic() { }

    public static ClientTopic NewTopic(int courseid) {
      return new ClientTopic { Name = "", Description = "", CourseID = courseid, ModelState = ModelState.New };
    }

    public static ClientTopic GetClientTopicFromTopic(int coursid, Topics topic) {
      return new ClientTopic { ID = topic.TopicID, Name = topic.TopicName, Description = topic.TopicDescription, CourseID = coursid };
    }

  }
}
