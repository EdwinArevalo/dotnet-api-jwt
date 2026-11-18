using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductosApi.Entidades.Dtos;
using ProductosApi.Negocio;
using ProductosApi.Negocio.Servicios;

namespace ProductosApi.Api.Controllers;

// CAPA API: controlador "delgado". Solo recibe la solicitud, llama al servicio
// y elige la respuesta (ActionResult). No usa el DbContext ni las entidades.
[ApiController]
[Route("api/[controller]")]
[Authorize]     // S14: por defecto, TODOS los endpoints exigen un token válido
public class ProductosController : ControllerBase
{
    private readonly IProductoServicio _servicio;

    public ProductosController(IProductoServicio servicio) => _servicio = servicio;

    // GET api/productos
    [AllowAnonymous]    // público: no necesita token
    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> Get() =>
        Ok(await _servicio.ListarAsync());

    // GET api/productos/1
    [AllowAnonymous]    // público: no necesita token
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoDto>> Get(int id)
    {
        var producto = await _servicio.ObtenerAsync(id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    // POST api/productos  (requiere token: 401 sin él)
    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Post(ProductoCrearDto dto)
    {
        try
        {
            var creado = await _servicio.CrearAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message });
        }
    }

    // PUT api/productos/1  (requiere token)
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ProductoCrearDto dto)
    {
        try
        {
            if (!await _servicio.ActualizarAsync(id, dto)) return NotFound();
            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new ProblemDetails { Title = "Regla de negocio", Detail = ex.Message });
        }
    }

    // DELETE api/productos/1
    [Authorize(Roles = "Admin")]   // solo el rol Admin; con otro rol responde 403
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await _servicio.EliminarAsync(id)) return NotFound();
        return NoContent();
    }
}
