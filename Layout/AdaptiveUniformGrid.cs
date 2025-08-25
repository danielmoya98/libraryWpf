using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace library.Layout
{
    /// <summary>
    /// Ajusta UniformGrid.Columns según breakpoints de ancho.
    /// Formato: "1400=4;1000=3;700=2;0=1" (orden no importa; se ordenan desc).
    /// </summary>
    public static class AdaptiveUniformGrid
    {
        public static readonly DependencyProperty BreakpointsProperty =
            DependencyProperty.RegisterAttached(
                "Breakpoints",
                typeof(string),
                typeof(AdaptiveUniformGrid),
                new PropertyMetadata("1400=4;1000=3;700=2;0=1", OnChanged));

        public static void SetBreakpoints(DependencyObject d, string value) => d.SetValue(BreakpointsProperty, value);
        public static string GetBreakpoints(DependencyObject d) => (string)d.GetValue(BreakpointsProperty);

        public static readonly DependencyProperty HorizontalPaddingProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalPadding",
                typeof(double),
                typeof(AdaptiveUniformGrid),
                new PropertyMetadata(0d, OnChanged));
        public static void SetHorizontalPadding(DependencyObject d, double value) => d.SetValue(HorizontalPaddingProperty, value);
        public static double GetHorizontalPadding(DependencyObject d) => (double)d.GetValue(HorizontalPaddingProperty);

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not UniformGrid ug) return;
            ug.Loaded -= UgOnLoadedOrSizeChanged;
            ug.SizeChanged -= UgOnLoadedOrSizeChanged;
            ug.Loaded += UgOnLoadedOrSizeChanged;
            ug.SizeChanged += UgOnLoadedOrSizeChanged;
            UpdateColumns(ug);
        }

        private static void UgOnLoadedOrSizeChanged(object? sender, RoutedEventArgs e)
        {
            if (sender is UniformGrid ug) UpdateColumns(ug);
        }

        private static void UpdateColumns(UniformGrid ug)
        {
            var w = ug.ActualWidth - GetHorizontalPadding(ug);
            if (w <= 0) return;

            // Parse breakpoints: "width=cols;..."
            var rules = GetBreakpoints(ug)
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    var parts = s.Split('=');
                    if (parts.Length != 2) return (Width: 0d, Cols: 1);
                    _ = double.TryParse(parts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var ww);
                    _ = int.TryParse(parts[1], out var cc);
                    return (Width: ww, Cols: Math.Max(1, cc));
                })
                .OrderByDescending(t => t.Width)
                .ToList();

            var cols = rules.FirstOrDefault(r => w >= r.Width).Cols;
            ug.Columns = Math.Max(1, cols);
            ug.Rows = 0;
        }
    }
}
