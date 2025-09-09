using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;

namespace library.Dialogs;

public partial class EditProfileDialogViewModel : ObservableObject
{
    // Datos básicos
    [ObservableProperty] private string? firstName;
    [ObservableProperty] private string? lastName;
    [ObservableProperty] private string? email;
    [ObservableProperty] private string? username;
    [ObservableProperty] private string? phone;
    [ObservableProperty] private string? address;
    [ObservableProperty] private string? notes;

    // Preferencias
    [ObservableProperty] private bool isDarkTheme;
    [ObservableProperty] private bool emailNotifications;

    // Avatar
    [ObservableProperty] private string? photoPath;

    public string FullName => $"{FirstName} {LastName}".Trim();

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(FirstName) &&
        !string.IsNullOrWhiteSpace(LastName) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Username);

    // Notificar que IsValid/FullName cambian cuando cambian campos base
    partial void OnFirstNameChanged(string? _) { OnPropertyChanged(nameof(IsValid)); OnPropertyChanged(nameof(FullName)); }
    partial void OnLastNameChanged(string? _)  { OnPropertyChanged(nameof(IsValid)); OnPropertyChanged(nameof(FullName)); }
    partial void OnEmailChanged(string? _)     { OnPropertyChanged(nameof(IsValid)); }
    partial void OnUsernameChanged(string? _)  { OnPropertyChanged(nameof(IsValid)); }

    [RelayCommand]
    private void BrowseImage()
    {
        var dlg = new OpenFileDialog
        {
            Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.webp;*.bmp",
            CheckFileExists = true
        };
        if (dlg.ShowDialog() == true && File.Exists(dlg.FileName))
        {
            PhotoPath = dlg.FileName;
        }
    }

    [RelayCommand]
    private void RemoveImage() => PhotoPath = null;
}
