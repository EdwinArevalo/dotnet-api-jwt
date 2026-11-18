using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Entidades.Dtos;

// DTO DE ENTRADA para registrarse. No incluye el rol: el rol lo decide el servidor,
// nunca el cliente (si no, cualquiera podría registrarse como Admin).
public class RegistroDto
{
    [Required, EmailAddress]
    public string Correo { get; set; } = null!;

    [Required, StringLength(50, MinimumLength = 6)]
    public string Clave { get; set; } = null!;
}
