namespace ProductosApi.Entidades.Dtos;

// DTO DE LECTURA: lo que la API DEVUELVE al cliente.
// Solo los datos necesarios; la categoría viaja como texto (su nombre).
public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal Precio { get; set; }
    public string? Categoria { get; set; }
}
