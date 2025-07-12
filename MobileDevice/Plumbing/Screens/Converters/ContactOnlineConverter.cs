using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Plumbing.Screens.Converters
{
    public class ContactOnlineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Color.FromHex("0ABB87") : Color.FromHex("FFB822");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}