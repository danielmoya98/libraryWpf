using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;                         // Binding.DoNothing
using MahApps.Metro.IconPacks;                // PackIconMaterialKind (mah:PackIconMaterial)

namespace library.Converters
{
    /// <summary>
    /// Convierte bool -> ícono. Usa el ConverterParameter con el formato "IconoSiTrue|IconoSiFalse".
    /// Por defecto: "WeatherNight|WhiteBalanceSunny".
    /// Soporta tanto mah:PackIconMaterial como materialDesign:PackIcon (detecta el tipo destino).
    /// </summary>
    public sealed class BoolToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value as bool? ?? false;

            var param = (parameter as string) ?? "WeatherNight|WhiteBalanceSunny";
            var parts = param.Split('|');
            var whenTrue  = parts[0];
            var whenFalse = parts.Length > 1 ? parts[1] : parts[0];

            // Caso 1: MahApps (mah:PackIconMaterial Kind)
            if (targetType == typeof(PackIconMaterialKind))
            {
                return Enum.Parse(typeof(PackIconMaterialKind), flag ? whenTrue : whenFalse);
            }

            // Caso 2: MaterialDesign (materialDesign:PackIcon Kind)
            var mdType = Type.GetType("MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf");
            if (mdType != null && targetType == mdType)
            {
                return Enum.Parse(mdType, flag ? whenTrue : whenFalse);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }

    /// <summary>
    /// Convierte bool -> texto. Usa ConverterParameter "TextoSiTrue|TextoSiFalse".
    /// </summary>
    public sealed class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value as bool? ?? false;
            var param = (parameter as string) ?? "Modo oscuro|Modo claro";
            var parts = param.Split('|');
            var whenTrue  = parts[0];
            var whenFalse = parts.Length > 1 ? parts[1] : parts[0];
            return flag ? whenTrue : whenFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
