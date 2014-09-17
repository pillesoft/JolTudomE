using System;
using System.Windows.Data;

namespace JolTudomE_Client.Helper
{
  public class RadioButtonConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      int input = int.Parse(value.ToString());
      int param = int.Parse(parameter.ToString());
      return input == param;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value == null || !(value is bool))
      {
        return string.Empty;
      }
      if (parameter == null || !(parameter is string))
      {
        return string.Empty;
      }
      if ((bool)value)
      {
        return parameter.ToString();
      }
      else
      {
        return string.Empty;
      }
    }
  }
}
