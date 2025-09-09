using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace library.Models
{
    public class Rol
    {
        public int IdRol { get; set; }
        
        [Required]
        public string Nombre { get; set; } = string.Empty;
        
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        // Navegación
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();

        // Métodos de utilidad
        public bool EsAdministrador => Nombre == "Administrador";
        public bool EsCliente => Nombre == "Cliente";
    }
}