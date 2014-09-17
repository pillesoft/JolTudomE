using System.Linq;
using System.Windows;
using System.Windows.Controls;
using JolTudomE_Client.Message;

namespace JolTudomE_Client.View {
  /// <summary>
  /// Interaction logic for DialogWindow.xaml
  /// </summary>
  public partial class DialogWindow : Window {
    private ShowDialogMessage _MsgContents;

    public DialogWindow(ShowDialogMessage contents) {

      InitializeComponent();

      _MsgContents = contents;

      this.Title = contents.Title;
      this.txtMessage.Text = contents.Text;

      this.cmdDefault.Content = contents.Buttons.First(b => b.IsDefault).Title;
      this.cmdDefault.IsDefault = contents.Buttons.First(b => b.IsDefault).IsDefault;
      this.cmdCancel.Content = contents.Buttons.First(b => !b.IsDefault).Title;
      this.cmdCancel.IsCancel = contents.Buttons.First(b => !b.IsDefault).IsDefault;
    }

    private void cmdDefault_Click(object sender, RoutedEventArgs e) {
      this.DialogResult = _MsgContents.Buttons.First(b => b.IsDefault).ReturnValue;
    }

    private void cmdCancel_Click(object sender, RoutedEventArgs e) {
      this.DialogResult = _MsgContents.Buttons.First(b => !b.IsDefault).ReturnValue;
    }
  }
}
