using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;

namespace JolTudomE_Client.Helper {
  public class RoutedPropertyChangedEventArgsToObjectConverter : IEventArgsConverter {
    public object Convert(object value, object parameter) {
      var args = (RoutedPropertyChangedEventArgs<object>)value;
      var element = (TreeView)parameter;
      
      return args.NewValue;
    }
  }
}
