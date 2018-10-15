using System;
using System.Windows.Data;

namespace OS.WPF.Converters
{
    /// <summary>
    /// Returns the opposite for boolean, used in dependency property route binding in views
    /// </summary>
    public class BoolInvertConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        #endregion
    }
}
