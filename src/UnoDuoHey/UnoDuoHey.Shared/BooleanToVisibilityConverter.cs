using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UnoDuoHey
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b)
            {
                return b switch
                {
                    true => Visibility.Visible,
                    false => Visibility.Collapsed
                };
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
