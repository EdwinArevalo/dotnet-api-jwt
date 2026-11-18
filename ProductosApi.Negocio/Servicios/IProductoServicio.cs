using ProductosApi.Entidades.Dtos;

namespace ProductosApi.Negocio.Servicios;

// Contrato de la CAPA NEGOCIO. Recibe y devuelve DTOs, nunca entidades.
public interface IProductoServicio
{
    Task<List<ProductoDto>> ListarAsync();
    Task<ProductoDto?> ObtenerAsync(int id);
    Task<ProductoDto> CrearAsync(ProductoCrearDto dto);
    Task<bool> ActualizarAsync(int id, ProductoCrearDto dto);
    Task<bool> EliminarAsync(int id);
}
