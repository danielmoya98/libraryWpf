using System;
using System.ComponentModel.DataAnnotations;

namespace library.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string? ClUsuario { get; set; }
        
        [Required]
        public string UserName { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Genero { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Nacionalidad { get; set; }
        public string? Biografia { get; set; }
        public string? Foto { get; set; }
        public int? IdRol { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Navegación
        public Rol? Rol { get; set; }

        // Propiedades calculadas
        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
        public bool EsAdministrador => Rol?.Nombre == "Administrador";
        public bool EsCliente => Rol?.Nombre == "Cliente";
        
        // Para mostrar en UI
        public string DisplayName => !string.IsNullOrEmpty(NombreCompleto) ? NombreCompleto : UserName;
        public string RolDisplay => Rol?.Nombre ?? "Sin rol";
    }
}