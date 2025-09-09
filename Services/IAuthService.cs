using library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace library.Services
{
    public interface IAuthService
    {
        Task<Usuario?> LoginAsync(string username, string password);
        Task<Usuario?> GetUsuarioByIdAsync(int id);
        Task<Usuario?> GetUsuarioByUsernameAsync(string username);
        Task<Usuario?> GetUsuarioByEmailAsync(string email);
        Task<List<Usuario>> GetAllUsuariosAsync();
        Task<List<Rol>> GetAllRolesAsync();
        Task<bool> ValidatePasswordAsync(string password, string hashedPassword);
        Task<Usuario?> CreateUsuarioAsync(Usuario usuario);
        Task<bool> UpdateUsuarioAsync(Usuario usuario);
        Task<bool> DeleteUsuarioAsync(int id);
    }
}