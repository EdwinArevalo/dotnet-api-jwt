using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductosApi.Datos.Repositorios;
using ProductosApi.Entidades;
using ProductosApi.Entidades.Dtos;

namespace ProductosApi.Negocio.Servicios;

// CAPA NEGOCIO: reglas de seguridad. No conoce HTTP: devuelve null o lanza una excepción
// y el controlador decide el código (401, 409...).
public class AuthServicio : IAuthServicio
{
    private readonly IUsuarioRepositorio _repo;
    private readonly JwtOpciones _jwt;

    public AuthServicio(IUsuarioRepositorio repo, JwtOpciones jwt)
    {
        _repo = repo;
        _jwt = jwt;
    }

    public async Task RegistrarAsync(RegistroDto dto)
    {
        // Regla de negocio: el correo debe ser único
        if (await _repo.ObtenerPorCorreoAsync(dto.Correo) != null)
            throw new ReglaNegocioException("Ya existe un usuario con ese correo.");

        var usuario = new Usuario
        {
            Correo = dto.Correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave),   // se guarda el hash, nunca la clave
            Rol = "Usuario"                                          // el rol lo decide el servidor
        };
        await _repo.AgregarAsync(usuario);
        await _repo.GuardarAsync();
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _repo.ObtenerPorCorreoAsync(dto.Correo);

        // Mismo resultado si el correo no existe o si la clave es incorrecta:
        // el cliente no debe saber cuál de las dos falló.
        if (usuario == null ||
            !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        // CLAIMS: los datos que viajan dentro del token (no pongas datos sensibles)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol)       // lo usarán [Authorize(Roles = ...)]
        };

        // La clave secreta firma el token: si alguien lo altera, la firma ya no coincide
        var clave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwt.Clave));
        var expira = DateTime.UtcNow.AddMinutes(_jwt.Minutos);

        var token = new JwtSecurityToken(
            issuer: _jwt.Emisor,
            audience: _jwt.Audiencia,
            claims: claims,
            expires: expira,
            signingCredentials: new SigningCredentials(
                clave, SecurityAlgorithms.HmacSha256));

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),   // texto: encabezado.claims.firma
            Expira = expira
        };
    }
}
