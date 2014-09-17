/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:JolTudomE_Client"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace JolTudomE_Client.ViewModel {
  /// <summary>
  /// This class contains static references to all the view models in the
  /// application and provides an entry point for the bindings.
  /// </summary>
  public class ViewModelLocator {
    /// <summary>
    /// Initializes a new instance of the ViewModelLocator class.
    /// </summary>
    public ViewModelLocator() {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      if (ViewModelBase.IsInDesignModeStatic) {
        // Create design time view services and models
        //SimpleIoc.Default.Register<IDataService, DesignDataService>();
      }
      else {
        // Create run time view services and models
        SimpleIoc.Default.Register<WSManager>();
      }

      SimpleIoc.Default.Register<MainViewModel>();
      SimpleIoc.Default.Register<LoginViewModel>();
      SimpleIoc.Default.Register<StudentViewModel>();
      SimpleIoc.Default.Register<NewPersonViewModel>();
      SimpleIoc.Default.Register<TestExecutionViewModel>();
      SimpleIoc.Default.Register<AdministrationViewModel>();

      SimpleIoc.Default.Register<AdmCourseViewModel>();
      SimpleIoc.Default.Register<AdmTopicViewModel>();

      SimpleIoc.Default.Register<AdmCourseCWViewModel>();
      SimpleIoc.Default.Register<AdmTopicCWViewModel>();

      SimpleIoc.Default.Register<TestDetailsViewModel>();
      
    }

    public MainViewModel Main {
      get {
        return ServiceLocator.Current.GetInstance<MainViewModel>();
      }
    }

    public LoginViewModel Login {
      get {
        return ServiceLocator.Current.GetInstance<LoginViewModel>();
      }
    }

    public StudentViewModel Student {
      get {
        return ServiceLocator.Current.GetInstance<StudentViewModel>();
      }
    }

    public NewPersonViewModel NewPerson {
      get {
        return ServiceLocator.Current.GetInstance<NewPersonViewModel>();
      }
    }

    public TestExecutionViewModel TestExecution {
      get {
        return ServiceLocator.Current.GetInstance<TestExecutionViewModel>();
      }
    }

    public AdministrationViewModel Admin {
      get {
        return ServiceLocator.Current.GetInstance<AdministrationViewModel>();
      }
    }

    public AdmCourseViewModel AdmCourse {
      get {
        return ServiceLocator.Current.GetInstance<AdmCourseViewModel>();
      }
    }

    public AdmCourseCWViewModel AdmCourseCW {
      get {
        return ServiceLocator.Current.GetInstance<AdmCourseCWViewModel>();
      }
    }

    public AdmTopicViewModel AdmTopic {
      get {
        return ServiceLocator.Current.GetInstance<AdmTopicViewModel>();
      }
    }

    public AdmTopicCWViewModel AdmTopicCW {
      get {
        return ServiceLocator.Current.GetInstance<AdmTopicCWViewModel>();
      }
    }

    public TestDetailsViewModel TestDetail {
      get {
        return ServiceLocator.Current.GetInstance<TestDetailsViewModel>();
      }
    }

    public static void Cleanup() {
      // TODO Clear the ViewModels
    }
  }
}