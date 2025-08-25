using System.Windows.Controls;
using System.Windows.Input;

namespace library.Controls;

public partial class TopBar : UserControl
{
    private static readonly RoutedCommand FocusSearchCommand = new();

    public TopBar()
    {
        InitializeComponent();
        // Atajos para enfocar el buscador: Ctrl+/ y Ctrl+K
        InputBindings.Add(new KeyBinding(FocusSearchCommand, Key.Oem2, ModifierKeys.Control)); // "/"
        InputBindings.Add(new KeyBinding(FocusSearchCommand, Key.K,    ModifierKeys.Control));

        CommandBindings.Add(new CommandBinding(FocusSearchCommand, (_, __) => SearchBox.Focus()));
    }
    
    private void FocusSearch_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
        SearchBox.Focus();
    }
}