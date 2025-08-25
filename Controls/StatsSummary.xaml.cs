using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace library.Controls;

public partial class StatsSummary : UserControl
{
    public StatsSummary() => InitializeComponent();

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(StatsSummary),
            new PropertyMetadata(null));
}