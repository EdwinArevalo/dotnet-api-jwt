using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Entidades.Dtos;

// DTO DE ENTRADA: lo que la API RECIBE del cliente (POST y PUT).
// Solo los campos permitidos (sin Id) y con sus validaciones:
// si no se cumplen, [ApiController] responde 400 automáticamente.
public class ProductoCrearDto
{
    [Required, StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Range(0.01, 100000)]
    public decimal Precio { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    public int CategoriaId { get; set; }
}
