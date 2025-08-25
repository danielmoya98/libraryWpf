using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace library.Controls;

public partial class ChartSummary : UserControl
{
    public ChartSummary() => InitializeComponent();

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ChartSummary),
            new PropertyMetadata(null));

    // ✅ NUEVO: breakpoints para el grid (valor por defecto: 2 columnas >=1200px, si no 1)
    public string Breakpoints
    {
        get => (string)GetValue(BreakpointsProperty);
        set => SetValue(BreakpointsProperty, value);
    }

    public static readonly DependencyProperty BreakpointsProperty =
        DependencyProperty.Register(nameof(Breakpoints), typeof(string), typeof(ChartSummary),
            new PropertyMetadata("1200=2;0=1"));
}