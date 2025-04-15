using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoActions2.Converters;

public class BooleanInverterConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool boolValue ? !boolValue : value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is bool boolValue ? !boolValue : value;
}