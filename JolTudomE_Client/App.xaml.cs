using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using JolTudomE_Client.JolTudomEWSService;
using JolTudomE_Client.Message;
using JolTudomE_Client.Model;
using JolTudomE_Client.View;
using JolTudomE_Client.ViewModel;


namespace JolTudomE_Client
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    internal int? CurrentTestID { get; set; }
    private ChildWindowMessage _CurrentCWDialog;

    private void ShowDialog(ShowDialogMessage param) {
      DialogWindow dw = new DialogWindow(param);
      Nullable<bool> dw_return = dw.ShowDialog();

      if (dw_return != null)
        if((bool)dw_return) 
          param.ExecuteIfTrue();
        else
          param.ExecuteIfFalse();
    }

    private void ShowChildWindowDialog(ChildWindowMessage param) {
      switch (param.Action) {
        case DialogAction.Open:
          _CurrentCWDialog = param;
          if (param.ModelInstance.GetType() == typeof(ClientCourse)) {
            AdmCourseWindow dw = new AdmCourseWindow();
            ((IBaseCWViewModel<ClientCourse>)dw.DataContext).SetInstance(param.ModelInstance as ClientCourse);
            dw.ShowDialog();
          }
          else if (param.ModelInstance.GetType() == typeof(ClientTopic)) {
            AdmTopicWindow dw = new AdmTopicWindow();
            ((IBaseCWViewModel<ClientTopic>)dw.DataContext).SetInstance(param.ModelInstance as ClientTopic);
            dw.ShowDialog();
          }
          else if (param.ModelInstance.GetType() == typeof(List<TestDetails>)) {
            TestDetailsWindow dw = new TestDetailsWindow();
            ((IBaseCWViewModel<List<TestDetails>>)dw.DataContext).SetInstance(param.ModelInstance as List<TestDetails>);
            dw.ShowDialog();
          }
          break;
        case DialogAction.Close:
          if (App.Current.Windows.Count > 1) {
            App.Current.Windows[1].Close();
          }
          if (_CurrentCWDialog != null) {
            _CurrentCWDialog.ExecuteAfterClose();
            _CurrentCWDialog = null;
          }
          break;
        default:
          break;
      }
    }

    private void Application_Startup(object sender, StartupEventArgs e) {
      this.DispatcherUnhandledException += (s, unhandledexcargs) => {
        unhandledexcargs.Handled = true;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Format("New Error occured at {0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
        sb.AppendLine(unhandledexcargs.Exception.ToString());
        sb.AppendLine();
        File.AppendAllText("Errors.txt", sb.ToString());

        MessageBox.Show(unhandledexcargs.Exception.Message, "Ooops! Hiba történt", MessageBoxButton.OK, MessageBoxImage.Error);
      };

      Messenger.Default.Register<ShowDialogMessage>(this, ShowDialog);
      Messenger.Default.Register<ChildWindowMessage>(this, ShowChildWindowDialog);

    }

  }
}
