using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Entidades.Dtos;

// DTO DE ENTRADA para iniciar sesión: lo que el cliente envía.
public class LoginDto
{
    [Required, EmailAddress]
    public string Correo { get; set; } = null!;

    [Required]
    public string Clave { get; set; } = null!;
}
