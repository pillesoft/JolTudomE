using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using JolTudomE_Client.Message;

namespace JolTudomE_Client
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    public MainWindow()
    {
      InitializeComponent();

      Messenger.Default.Register<CloseApplicationMessage>(this, m => Close());
    }

  }
}
