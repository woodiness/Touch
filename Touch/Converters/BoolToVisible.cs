using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Touch.Converters
{
    public class BoolToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var b = (bool) value;
            return b ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}