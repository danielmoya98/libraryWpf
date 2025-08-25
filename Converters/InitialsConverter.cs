using System;
using System.Globalization;
using System.Windows.Data;

namespace library.Converters  // 👈 debe coincidir EXACTO con el xmlns del XAML
{
    public class InitialsConverter : IValueConverter   // 👈 pública e IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value?.ToString()?.Trim();
            if (string.IsNullOrEmpty(s)) return "?";

            var parts = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0][0].ToString().ToUpper(culture);

            return (char.ToUpper(parts[0][0], culture)).ToString() +
                   (char.ToUpper(parts[1][0], culture)).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}