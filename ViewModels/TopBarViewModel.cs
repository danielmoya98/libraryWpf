using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using library.Dialogs;
using library.Models;
using library.Services;
using MaterialDesignThemes.Wpf;

namespace library.ViewModels
{
    public partial class TopBarViewModel : ObservableObject
    {
        private readonly IUserSessionService? _userSessionService;

        [ObservableProperty] private string? searchText;
        [ObservableProperty] private int notificationsCount = 4;
        [ObservableProperty] private bool isDarkTheme;

        // Propiedades del usuario actual
        [ObservableProperty] private string userName = "Usuario";
        [ObservableProperty] private string userRole = "Rol";
        [ObservableProperty] private string userInitials = "U";
        [ObservableProperty] private bool isAdmin = false;

        // Usuario actual (del sistema de autenticación)
        public Usuario? CurrentUser { get; private set; }

        public IAsyncRelayCommand<Customer?> EditUserCommand { get; }

        public bool HasNotifications => NotificationsCount > 0;
        partial void OnNotificationsCountChanged(int value) => OnPropertyChanged(nameof(HasNotifications));

        public TopBarViewModel()
        {
            EditUserCommand = new AsyncRelayCommand<Customer?>(OpenEditProfileAsync);
            IsDarkTheme = ThemeService.IsDark;
        }

        public TopBarViewModel(IUserSessionService userSessionService) : this()
        {
            _userSessionService = userSessionService;
        }

        public void UpdateUserInfo(Usuario usuario)
        {
            CurrentUser = usuario;
            UserName = usuario.DisplayName;
            UserRole = usuario.RolDisplay;
            IsAdmin = usuario.EsAdministrador;

            // Generar iniciales
            var names = usuario.DisplayName.Split(' ');
            UserInitials = names.Length >= 2 
                ? $"{names[0][0]}{names[1][0]}".ToUpper()
                : usuario.DisplayName.Length > 0 
                    ? usuario.DisplayName.Substring(0, Math.Min(2, usuario.DisplayName.Length)).ToUpper()
                    : "U";
        }

        private async Task OpenEditProfileAsync(Customer? customer)
        {
            if (CurrentUser == null)
            {
                MessageBox.Show("⚠️ No hay usuario logueado para editar.");
                return;
            }

            // Convertir Usuario a Customer para el diálogo (adaptación temporal)
            var userAsCustomer = new Customer
            {
                FirstName = CurrentUser.Nombre ?? "Sin nombre",
                LastName = CurrentUser.Apellido ?? "Sin apellido",
                Email = CurrentUser.Email ?? "Sin email",
                Phone = CurrentUser.Telefono ?? "Sin teléfono",
                Address = CurrentUser.Direccion ?? "Sin dirección",
                PhotoPath = CurrentUser.Foto
            };

            var target = customer ?? userAsCustomer;

            var vm = new EditProfileDialogViewModel
            {
                FirstName = target.FirstName,
                LastName = target.LastName,
                Email = target.Email,
                Phone = target.Phone,
                Address = target.Address,
                PhotoPath = target.PhotoPath,
            };

            var view = new EditProfileDialog { DataContext = vm };

            try
            {
                var result = await DialogHost.Show(view, "RootDialog");

                if (result is EditProfileDialogViewModel saved)
                {
                    MessageBox.Show($"Perfil actualizado: {saved.FirstName} {saved.LastName}\n\n" +
                                    $"ID Usuario: {CurrentUser.IdUsuario}\n" +
                                    $"Rol: {CurrentUser.RolDisplay}");

                    // TODO: Aquí deberías actualizar el usuario en la base de datos
                    // usando IAuthService.UpdateUsuarioAsync()
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"🔥 Error en DialogHost.Show: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task OpenNotifications()
        {
            var vm = new NotificationsViewModel();
            var view = new NotificationsDialog { DataContext = vm };

            await DialogHost.Show(view, "RootDialog");
        }

        [RelayCommand]
        private void ToggleTheme()
        {
            ThemeService.Toggle();
            IsDarkTheme = ThemeService.IsDark;
        }

        [RelayCommand]
        private void ClearSearch()
        {
            var shell = Application.Current?.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive)?
                .DataContext as ShellViewModel;

            if (shell != null)
                shell.GlobalSearch = string.Empty;
        }

        [RelayCommand]
        private void ShowProfile()
        {
            var userId = _userSessionService?.GetCurrentUserId();
            MessageBox.Show(
                $"📋 Perfil de usuario\n\n" +
                $"👤 Nombre: {UserName}\n" +
                $"🏷️ Rol: {UserRole}\n" +
                $"🆔 ID: {userId}\n" +
                $"🔑 Es Admin: {(IsAdmin ? "Sí" : "No")}\n" +
                $"📧 Email: {CurrentUser?.Email ?? "No especificado"}",
                "Mi Perfil - Biblioverso",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        [RelayCommand]
        private void Logout()
        {
            var result = MessageBox.Show(
                "¿Estás seguro de que deseas cerrar sesión?",
                "Cerrar Sesión - Biblioverso",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _userSessionService?.Logout();
            }
        }

        [RelayCommand]
        private void ChangeLanguage(string lang)
        {
            // TODO: cambiar idioma
        }
    }
}