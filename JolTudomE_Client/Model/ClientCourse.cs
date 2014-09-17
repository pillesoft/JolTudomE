using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using JolTudomE_Client.JolTudomEWSService;

namespace JolTudomE_Client.Model {
  public class ClientCourse : BaseModel {

    private int _ID;

    public int ID {
      get { return _ID; }
      set {
        _ID = value;
        OnPropertyChanged("ID");
      }
    }

    private string _Name;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Kurzus neve kötelező")]
    [StringLength(30, ErrorMessage = "Maximum 30 karakter lehet a Kurzus neve")]
    public string Name {
      get { return _Name; }
      set {
        _Name = value;
        ValidateProperty(Name, "Name");
        OnPropertyChanged("Name");
      }
    }

    private string _Description;

    [StringLength(255, ErrorMessage = "Maximum 255 karakter lehet a Kurzus leírása")]
    public string Description {
      get { return _Description; }
      set {
        _Description = value;
        ValidateProperty(_Description, "Description");
        OnPropertyChanged("Description");
      }
    }

    private ClientCourse() { }

    public static ClientCourse NewCourse() {
      return new ClientCourse { Name = "", Description = "", ModelState = ModelState.New };
    }

    public static ClientCourse GetClientCourseFromCourse(Courses course) {
      return new ClientCourse { ID = course.CourseID, Name = course.CourseName, Description = course.CourseDescription };
    }
  }
}
