using System.Windows;
using System.Windows.Controls;

namespace library.Controls;

public partial class ChartCard : UserControl
{
    public ChartCard() => InitializeComponent();

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(ChartCard), new PropertyMetadata(""));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // ✅ Nueva propiedad para evitar el bucle con Content
    public static readonly DependencyProperty ChartContentProperty =
        DependencyProperty.Register(nameof(ChartContent), typeof(object), typeof(ChartCard), new PropertyMetadata(null));

    public object ChartContent
    {
        get => GetValue(ChartContentProperty);
        set => SetValue(ChartContentProperty, value);
    }
}