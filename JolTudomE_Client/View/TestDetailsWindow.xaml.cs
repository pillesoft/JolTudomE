using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JolTudomE_Client.View {
  /// <summary>
  /// Interaction logic for TestDetailsWindow.xaml
  /// </summary>
  public partial class TestDetailsWindow : Window {
    public TestDetailsWindow() {
      InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e) {
      this.Close();
    }
  }
}
