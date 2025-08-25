using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace library.Converters
{
    // Oculta textos cuando el ancho del sidebar es menor al umbral (compacto).
    public class CollapseByWidthToVisibilityConverter : IValueConverter
    {
        public double Threshold { get; set; } = 120.0;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var t = Threshold;
            if (parameter is string s && double.TryParse(s, out var p)) t = p;
            if (value is double w) return w < t ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }

    // Convierte bool (IsExpanded) -> ancho (expanded;collapsed).
    // Usa ConverterParameter con el formato "expanded;collapsed", por defecto "240;64".
    public class ExpandToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double expanded = 240, collapsed = 64;
            if (parameter is string p)
            {
                var parts = p.Split(';');
                if (parts.Length > 0 && double.TryParse(parts[0], out var e)) expanded = e;
                if (parts.Length > 1 && double.TryParse(parts[1], out var c)) collapsed = c;
            }
            var b = value is bool v && v;
            return b ? expanded : collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
    }
}