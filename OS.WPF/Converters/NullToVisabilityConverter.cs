using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace OS.WPF.Converters
{
    /// <summary>
    /// Returns text for a combo box that has no selection (i.e 'select')
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
