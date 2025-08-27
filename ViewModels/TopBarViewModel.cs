using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using library.Services;

namespace library.ViewModels;

public partial class TopBarViewModel : ObservableObject
{
    [ObservableProperty] private string? searchText;
    [ObservableProperty] private int notificationsCount = 4; // demo
    [ObservableProperty] private bool isDarkTheme;
    public bool HasNotifications => NotificationsCount > 0;
    partial void OnNotificationsCountChanged(int value) => OnPropertyChanged(nameof(HasNotifications));

    public TopBarViewModel()
    {
        // Sincroniza con el estado actual del servicio
        IsDarkTheme = ThemeService.IsDark;
    }
    
    // [RelayCommand]
    // private void ClearSearch() => SearchText = string.Empty;
    [RelayCommand]
    private void ToggleTheme()
    {
        ThemeService.Toggle();
        IsDarkTheme = ThemeService.IsDark; // refleja el cambio para el icono/texto
    }

    [RelayCommand]
    private void ClearSearch()
    {
        // Limpia el GlobalSearch del ShellViewModel si es tu DataContext principal
        var shell = Application.Current?.Windows
            ?.OfType<Window>()
            ?.FirstOrDefault(w => w.IsActive)?.DataContext as ShellViewModel;

        if (shell != null)
            shell.GlobalSearch = string.Empty;
    }

    [RelayCommand]
    private void OpenNotifications()
    {
        /* TODO */
    }

    [RelayCommand]
    private void ChangeLanguage(string lang)
    {
        /* TODO */
    }
}