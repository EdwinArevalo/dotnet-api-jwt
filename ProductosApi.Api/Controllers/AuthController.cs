using Microsoft.AspNetCore.Mvc;
using ProductosApi.Entidades.Dtos;
using ProductosApi.Negocio;
using ProductosApi.Negocio.Servicios;

namespace ProductosApi.Api.Controllers;

// CAPA API: controlador delgado de seguridad. Sin [Authorize]: es la puerta de entrada
// (nadie tiene token todavía cuando se registra o inicia sesión).
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthServicio _auth;

    public AuthController(IAuthServicio auth) => _auth = auth;

    // POST api/auth/registro  -> 201 Created, 400 (validación) o 409 (correo repetido)
    [HttpPost("registro")]
    public async Task<IActionResult> Registro(RegistroDto dto)
    {
        try
        {
            await _auth.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message });
        }
    }

    // POST api/auth/login  -> 200 con el token, o 401 si las credenciales no son válidas
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await _auth.LoginAsync(dto);
        if (token == null) return Unauthorized();
        return Ok(token);
    }
}
