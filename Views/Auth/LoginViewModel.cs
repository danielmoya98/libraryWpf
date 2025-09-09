using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using library.Models;
using library.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace library.ViewModels
{
    public partial class DatabaseLoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string username = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private bool rememberMe = false;
        [ObservableProperty] private bool isBusy = false;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private bool isPasswordVisible = false;

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public bool CanLogin => !IsBusy && !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        public event Action<Usuario>? LoginSuccess;

        public DatabaseLoginViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        // Partial methods
        partial void OnErrorMessageChanged(string value) => OnPropertyChanged(nameof(HasError));
        partial void OnUsernameChanged(string value) => OnPropertyChanged(nameof(CanLogin));
        partial void OnPasswordChanged(string value) => OnPropertyChanged(nameof(CanLogin));
        partial void OnIsBusyChanged(bool value) => OnPropertyChanged(nameof(CanLogin));

        [RelayCommand]
        private void TogglePassword()
        {
            IsPasswordVisible = !IsPasswordVisible;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (!CanLogin) return;

            ErrorMessage = string.Empty;
            IsBusy = true;

            try
            {
                var usuario = await _authService.LoginAsync(Username.Trim(), Password);

                if (usuario != null)
                {
                    // Éxito en el login
                    System.Diagnostics.Debug.WriteLine($"Login exitoso: {usuario.DisplayName} ({usuario.RolDisplay})");
                    LoginSuccess?.Invoke(usuario);
                }
                else
                {
                    ErrorMessage = "Usuario o contraseña incorrectos. Verifica tus credenciales.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error de conexión con Neon: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Login error: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CheckDatabaseUsers()
        {
            try
            {
                IsBusy = true;
                var usuarios = await _authService.GetAllUsuariosAsync();
                var roles = await _authService.GetAllRolesAsync();

                var userInfo = "📊 Usuarios en Neon Database (biblioverso):\n";
                userInfo += "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n";

                if (usuarios.Any())
                {
                    foreach (var user in usuarios.Take(5)) // Solo mostrar los primeros 5
                    {
                        var rolName = user.Rol?.Nombre ?? "Sin rol";
                        userInfo += $"👤 Usuario: {user.UserName}\n";
                        userInfo += $"   Nombre: {user.DisplayName}\n";
                        userInfo += $"   Email: {user.Email ?? "No especificado"}\n";
                        userInfo += $"   Rol: {rolName}\n";
                        userInfo += $"   ID: {user.IdUsuario}\n\n";
                    }

                    if (usuarios.Count > 5)
                    {
                        userInfo += $"... y {usuarios.Count - 5} usuarios más.\n\n";
                    }

                    userInfo += $"📈 Total de usuarios: {usuarios.Count}\n";
                    userInfo += $"🎭 Roles disponibles: {string.Join(", ", roles.Select(r => r.Nombre))}\n\n";
                    userInfo += "💡 Base de datos: PostgreSQL en Neon\n";
                    userInfo += "🔐 Autenticación: BCrypt + Texto plano (migración)";
                }
                else
                {
                    userInfo += "❌ No se encontraron usuarios en la base de datos.\n\n";
                    userInfo += "Verifica que:\n";
                    userInfo += "• La conexión a Neon esté funcionando\n";
                    userInfo += "• Las tablas existan en la base de datos\n";
                    userInfo += "• Los usuarios hayan sido insertados correctamente";
                }

                System.Windows.MessageBox.Show(
                    userInfo,
                    "Usuarios en Neon Database",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error al consultar usuarios en Neon:\n\n{ex.Message}\n\nVerifica tu conexión a la base de datos.",
                    "Error de Conexión",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void ForgotPassword()
        {
            System.Windows.MessageBox.Show(
                "🔐 Para recuperar tu contraseña:\n\n" +
                "1. Contacta al administrador del sistema\n" +
                "2. Proporciona tu nombre de usuario o email\n" +
                "3. El administrador podrá restablecer tu contraseña\n\n" +
                "👤 Usuario admin disponible: admin\n" +
                "📧 Contacto: admin@biblioteca.com\n" +
                "🌐 Base de datos: Neon PostgreSQL",
                "Recuperar Contraseña - Biblioverso",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        [RelayCommand]
        private void ShowHelp()
        {
            System.Windows.MessageBox.Show(
                "💡 Ayuda para iniciar sesión en Biblioverso:\n\n" +
                "• Usa tu nombre de usuario o email para iniciar sesión\n" +
                "• Las contraseñas son sensibles a mayúsculas y minúsculas\n" +
                "• Usuario predeterminado: admin\n" +
                "• El sistema soporta roles: Administrador y Cliente\n\n" +
                "🔍 Para ver usuarios existentes, haz clic en 'Ver Usuarios BD'\n" +
                "🗄️ Base de datos: PostgreSQL 17 en Neon\n" +
                "❓ Si tienes problemas, verifica la conexión de red",
                "Ayuda - Sistema Biblioverso",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Question);
        }
    }
}