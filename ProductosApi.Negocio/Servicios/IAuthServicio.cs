using ProductosApi.Entidades.Dtos;

namespace ProductosApi.Negocio.Servicios;

// Contrato de la capa Negocio para la seguridad. Recibe y devuelve DTOs.
public interface IAuthServicio
{
    Task RegistrarAsync(RegistroDto dto);
    Task<TokenDto?> LoginAsync(LoginDto dto);   // null = credenciales inválidas
}
