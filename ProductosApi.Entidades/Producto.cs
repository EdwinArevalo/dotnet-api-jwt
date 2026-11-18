namespace ProductosApi.Entidades;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public int CategoriaId { get; set; }

    // Navegación hacia la categoría. Esta entidad NUNCA sale por la API:
    // por eso ya no hace falta [JsonIgnore] ni hay ciclo en el JSON.
    public virtual Categoria? Categoria { get; set; }
}
