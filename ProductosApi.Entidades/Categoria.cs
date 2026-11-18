using System.Text.Json.Serialization;

namespace ProductosApi.Entidades;

// CAPA ENTIDADES: clases que representan las tablas de la base de datos.
// No dependen de nadie: todas las demás capas las usan.
public partial class Categoria
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    // Navegación 1:N: una categoría tiene muchos productos
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
