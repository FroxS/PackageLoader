using System;
using System.Globalization;
using System.Windows;

namespace PackageLoader.Conventers
{
    public class NullToVisibleConventer : BaseValueConventer<NullToVisibleConventer>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}