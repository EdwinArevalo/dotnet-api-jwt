namespace ProductosApi.Entidades.Dtos;

// DTO DE LECTURA: lo que la API devuelve al iniciar sesión.
public class TokenDto
{
    public string Token { get; set; } = null!;
    public DateTime Expira { get; set; }
}
