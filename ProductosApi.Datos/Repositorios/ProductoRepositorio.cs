using Microsoft.EntityFrameworkCore;
using ProductosApi.Entidades;

namespace ProductosApi.Datos.Repositorios;

// IMPLEMENTACIÓN con EF Core y LINQ. El DbContext llega por inyección de dependencias.
public class ProductoRepositorio : IProductoRepositorio
{
    private readonly ProductosContext _context;

    public ProductoRepositorio(ProductosContext context) => _context = context;

    public Task<List<Producto>> ListarAsync() =>
        _context.Productos
            .Include(p => p.Categoria)      // para poder mostrar el nombre de la categoría
            .AsNoTracking()                 // solo lectura
            .OrderBy(p => p.Nombre)
            .ToListAsync();

    public Task<Producto?> ObtenerAsync(int id) =>
        _context.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

    public Task<bool> ExisteNombreAsync(string nombre) =>
        _context.Productos.AnyAsync(p => p.Nombre == nombre);

    public Task<bool> ExisteCategoriaAsync(int categoriaId) =>
        _context.Categorias.AnyAsync(c => c.Id == categoriaId);

    public async Task AgregarAsync(Producto producto) =>
        await _context.Productos.AddAsync(producto);

    public Task EliminarAsync(Producto producto)
    {
        _context.Productos.Remove(producto);
        return Task.CompletedTask;
    }

    // Nada se guarda hasta llamar a este método
    public Task GuardarAsync() => _context.SaveChangesAsync();
}
