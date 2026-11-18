using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ProductosApi.Datos;
using ProductosApi.Datos.Repositorios;
using ProductosApi.Negocio;
using ProductosApi.Negocio.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// OpenAPI con el esquema "Bearer": habilita el botón Authorize de Swagger
builder.Services.AddOpenApi(options =>
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes =
            new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            }
        };
        document.Security = [new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        }];
        return Task.CompletedTask;
    }));

// Inyección de dependencias (capas de la S13 + piezas de seguridad de la S14)
builder.Services.AddDbContext<ProductosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Productos")));
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();   // S14
builder.Services.AddScoped<IAuthServicio, AuthServicio>();               // S14
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// JWT: se leen las opciones de appsettings.json y se valida cada token que llega
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOpciones>()!;
builder.Services.AddSingleton(jwt);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true, ValidIssuer = jwt.Emisor,
            ValidateAudience = true, ValidAudience = jwt.Audiencia,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.Clave))
        });
builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger: /swagger (con el botón Authorize para pegar el token)
app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));

app.UseHttpsRedirection();
app.UseAuthentication();    // 1.º: lee y valida el token ("¿quién eres?")
app.UseAuthorization();     // 2.º: decide con [Authorize] y roles ("¿puedes hacerlo?")
app.MapControllers();

app.Run();
