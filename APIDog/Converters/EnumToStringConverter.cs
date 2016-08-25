using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace APIDog.Converters
{
    /// <summary>
    /// Converter
    /// Enum to String
    /// </summary>
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.IsDefined(value.GetType(), value) == false || value == null)
                return DependencyProperty.UnsetValue;

            return Enum.GetName(value.GetType(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.Parse(targetType, (string)value);
        }
    }
}