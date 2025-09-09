using library.Models;
using System;

namespace library.Services
{
    public class UserSessionService : IUserSessionService
    {
        private Usuario? _currentUser;

        public Usuario? CurrentUser
        {
            get => _currentUser;
            private set
            {
                _currentUser = value;
                System.Diagnostics.Debug.WriteLine($"Usuario actual actualizado: {_currentUser?.DisplayName ?? "null"}");
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public event Action<Usuario>? UserLoggedIn;
        public event Action? UserLoggedOut;

        public void Login(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            CurrentUser = usuario;
            System.Diagnostics.Debug.WriteLine($"Usuario logueado: {usuario.DisplayName} (ID: {usuario.IdUsuario}, Rol: {usuario.RolDisplay})");
            
            UserLoggedIn?.Invoke(usuario);
        }

        public void Logout()
        {
            var previousUser = CurrentUser;
            CurrentUser = null;
            
            System.Diagnostics.Debug.WriteLine($"Usuario deslogueado: {previousUser?.DisplayName ?? "unknown"}");
            
            UserLoggedOut?.Invoke();
        }

        public int? GetCurrentUserId()
        {
            return CurrentUser?.IdUsuario;
        }

        public bool HasRole(string roleName)
        {
            return CurrentUser?.Rol?.Nombre?.Equals(roleName, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}