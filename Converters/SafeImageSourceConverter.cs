using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace library.Converters
{
    /// Devuelve null si la ruta viene null / vacía / inválida.
    public sealed class SafeImageSourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string s || string.IsNullOrWhiteSpace(s)) return null;

            try
            {
                if (Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out var uri))
                    return new BitmapImage(uri);
            }
            catch { /* swallow y devolver null */ }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}