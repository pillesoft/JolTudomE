using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace JolTudomE_Client.Helper {
  /// <summary>
  /// used for ErrorMessage binding property
  /// if the value is null the textblock of the error message is collapsed
  /// otherwise visible
  /// </summary>
  public class ErrorMsgToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      if (string.IsNullOrEmpty((string)value)) 
        return Visibility.Collapsed;
      else
        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
