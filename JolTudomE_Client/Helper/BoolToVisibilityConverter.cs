using System;
using System.Windows;
using System.Windows.Data;

namespace JolTudomE_Client.Helper
{
  public class BoolToVisibilityConverter : IValueConverter
  {

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (parameter == null) return (bool)value ? Visibility.Visible : Visibility.Collapsed;
      else return (bool)value ? Visibility.Collapsed : Visibility.Visible;      
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
