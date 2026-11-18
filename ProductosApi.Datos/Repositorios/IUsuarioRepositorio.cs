using ProductosApi.Entidades;

namespace ProductosApi.Datos.Repositorios;

// Operaciones de datos sobre la tabla Usuarios (igual que IProductoRepositorio).
public interface IUsuarioRepositorio
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task AgregarAsync(Usuario usuario);
    Task GuardarAsync();
}
