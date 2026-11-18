namespace ProductosApi.Entidades;

// Entidad de la tabla Usuarios (JWT S14).
public partial class Usuario
{
    public int Id { get; set; }

    public string Correo { get; set; } = null!;

    // Nunca se guarda la contraseña: solo su hash (BCrypt)
    public string ClaveHash { get; set; } = null!;

    // Define los permisos: "Admin" o "Usuario"
    public string Rol { get; set; } = null!;
}
