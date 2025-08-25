// Dialogs/AddCustomerDialogViewModel.cs
using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using library.Models;

namespace library.Dialogs;

public partial class AddCustomerDialogViewModel : ObservableObject
{
    [ObservableProperty] private string? photoPath;
    [ObservableProperty] private string firstName = "";
    [ObservableProperty] private string lastName  = "";
    [ObservableProperty] private string ci        = "";
    [ObservableProperty] private string email     = "";
    [ObservableProperty] private string phone     = "";
    [ObservableProperty] private string address   = "";
    [ObservableProperty] private string username  = "";
    [ObservableProperty] private string password  = "";
    [ObservableProperty] private Gender gender    = Gender.Other;

    public IReadOnlyList<Gender> Genders { get; } =
        Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(FirstName)
        && !string.IsNullOrWhiteSpace(LastName)
        && !string.IsNullOrWhiteSpace(Email);

    partial void OnFirstNameChanged(string value) => OnPropertyChanged(nameof(IsValid));
    partial void OnLastNameChanged(string value)  => OnPropertyChanged(nameof(IsValid));
    partial void OnEmailChanged(string value)     => OnPropertyChanged(nameof(IsValid));

    [RelayCommand]
    private void BrowseImage()
    {
        var dlg = new OpenFileDialog
        {
            Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.webp;*.bmp",
            Multiselect = false
        };
        if (dlg.ShowDialog() == true)
            PhotoPath = dlg.FileName;
    }
}