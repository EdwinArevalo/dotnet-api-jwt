using ProductosApi.Entidades;

namespace ProductosApi.Datos.Repositorios;

// La INTERFAZ define qué operaciones de datos existen.
// Las capas superiores dependen de esta interfaz, no de EF Core.
// Trabaja con ENTIDADES: el repositorio no conoce DTOs ni HTTP.
public interface IProductoRepositorio
{
    Task<List<Producto>> ListarAsync();
    Task<Producto?> ObtenerAsync(int id);
    Task<bool> ExisteNombreAsync(string nombre);
    Task<bool> ExisteCategoriaAsync(int categoriaId);
    Task AgregarAsync(Producto producto);
    Task EliminarAsync(Producto producto);
    Task GuardarAsync();
}
