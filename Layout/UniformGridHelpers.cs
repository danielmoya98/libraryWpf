using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace library.Layout;

public static class UniformGridHelpers
{
    public static readonly DependencyProperty EnableAutoColumnsProperty =
        DependencyProperty.RegisterAttached("EnableAutoColumns", typeof(bool), typeof(UniformGridHelpers),
            new PropertyMetadata(false, OnChanged));
    public static void SetEnableAutoColumns(DependencyObject o, bool v) => o.SetValue(EnableAutoColumnsProperty, v);
    public static bool GetEnableAutoColumns(DependencyObject o) => (bool)o.GetValue(EnableAutoColumnsProperty);

    public static readonly DependencyProperty MinItemWidthProperty =
        DependencyProperty.RegisterAttached("MinItemWidth", typeof(double), typeof(UniformGridHelpers),
            new PropertyMetadata(240.0, OnChanged));
    public static void SetMinItemWidth(DependencyObject o, double v) => o.SetValue(MinItemWidthProperty, v);
    public static double GetMinItemWidth(DependencyObject o) => (double)o.GetValue(MinItemWidthProperty);

    public static readonly DependencyProperty GapProperty =
        DependencyProperty.RegisterAttached("Gap", typeof(double), typeof(UniformGridHelpers),
            new PropertyMetadata(16.0, OnGapChanged));
    public static void SetGap(DependencyObject o, double v) => o.SetValue(GapProperty, v);
    public static double GetGap(DependencyObject o) => (double)o.GetValue(GapProperty);

    private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UniformGrid ug)
        {
            ug.Loaded -= OnSizeChanged;
            ug.SizeChanged -= OnSizeChanged;
            if (GetEnableAutoColumns(ug))
            {
                ug.Loaded += OnSizeChanged;
                ug.SizeChanged += OnSizeChanged;
            }
        }
    }

    private static void OnGapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        // El “gap” lo implementamos dando margen a cada item desde el ItemsControl.
        // Aquí no hay nada que hacer para el panel como tal.
    }

    private static void OnSizeChanged(object sender, RoutedEventArgs e)
    {
        var ug = (UniformGrid)sender;
        if (!GetEnableAutoColumns(ug)) return;
        var w = ug.ActualWidth;
        var min = GetMinItemWidth(ug);
        var gap = GetGap(ug);

        if (w <= 0 || min <= 0) return;

        // columnas = piso((ancho + gap) / (minWidth + gap)), mínimo 1
        var cols = Math.Max(1, (int)Math.Floor((w + gap) / (min + gap)));
        ug.Columns = cols;
        ug.Rows = 0;
    }
}
