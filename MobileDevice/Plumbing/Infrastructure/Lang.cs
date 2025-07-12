using System;
using System.Globalization;
using Pro4Soft.DataTransferObjects;
using Xamarin.Forms;
// ReSharper disable CoVariantArrayConversion

namespace Pro4Soft.MobileDevice.Plumbing.Infrastructure
{
    public class Lang : IValueConverter
    {
        public static string Translate(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return key;
            if (Singleton<Context>.Instance.Translations == null)
                return key;
            
            var (transKey, replace) = Utils.PrepareForTranslation(key);
            if (Singleton<Context>.Instance.Translations.TryGetValue(transKey, out var result))
                return string.Format(result ?? transKey, replace);
            
            Singleton<Context>.Instance.DirtyTranslations[transKey] = null;
            return string.Format(transKey, replace);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Translate(parameter?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Localization cannot be converted back");
        }
    }
}
