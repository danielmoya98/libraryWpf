using library.Models;
using System;

namespace library.Services
{
    public interface IUserSessionService
    {
        /// <summary>
        /// Usuario actualmente logueado
        /// </summary>
        Usuario? CurrentUser { get; }

        /// <summary>
        /// Indica si hay un usuario logueado
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Evento que se dispara cuando el usuario hace login
        /// </summary>
        event Action<Usuario>? UserLoggedIn;

        /// <summary>
        /// Evento que se dispara cuando el usuario hace logout
        /// </summary>
        event Action? UserLoggedOut;

        /// <summary>
        /// Inicia sesión con un usuario
        /// </summary>
        void Login(Usuario usuario);

        /// <summary>
        /// Cierra la sesión actual
        /// </summary>
        void Logout();

        /// <summary>
        /// Obtiene el ID del usuario actual
        /// </summary>
        int? GetCurrentUserId();

        /// <summary>
        /// Verifica si el usuario actual tiene un rol específico
        /// </summary>
        bool HasRole(string roleName);
    }
}