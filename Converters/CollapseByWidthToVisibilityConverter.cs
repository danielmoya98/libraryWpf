using System.Globalization;
using System.Windows;
using System.Windows.Data;

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