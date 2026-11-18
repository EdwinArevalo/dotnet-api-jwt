namespace ProductosApi.Negocio;

// Valores de la sección "Jwt" de appsettings.json.
// Se usan en dos lugares: para GENERAR el token (AuthServicio) y para VALIDARLO (Program.cs).
public class JwtOpciones
{
    public string Clave { get; set; } = null!;      // clave secreta con la que se firma
    public string Emisor { get; set; } = null!;     // quién genera el token
    public string Audiencia { get; set; } = null!;  // a quién va dirigido
    public int Minutos { get; set; }                // vida del token
}
