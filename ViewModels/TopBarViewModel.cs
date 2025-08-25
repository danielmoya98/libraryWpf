using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace library.ViewModels;

public partial class TopBarViewModel : ObservableObject
{
    [ObservableProperty] private string? searchText;
    [ObservableProperty] private int notificationsCount = 4; // demo

    public bool HasNotifications => NotificationsCount > 0;
    partial void OnNotificationsCountChanged(int value) => OnPropertyChanged(nameof(HasNotifications));

    [RelayCommand] private void ClearSearch() => SearchText = string.Empty;
    [RelayCommand] private void OpenNotifications() { /* TODO */ }
    [RelayCommand] private void ChangeLanguage(string lang) { /* TODO */ }
}