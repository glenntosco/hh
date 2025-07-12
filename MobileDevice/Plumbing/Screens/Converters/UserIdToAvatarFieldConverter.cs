using System;
using System.Globalization;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Plumbing.Screens.Converters
{
    public class UserIdToAvatarFieldConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
                return null;
            return Singleton<Context>.Instance.UserAvatars.TryGetValue((Guid)value, out var result) ? result : null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}