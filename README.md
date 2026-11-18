# ProductosApi (S14): API en capas con autenticación JWT

Continúa la demo de la S13 (capas, DTOs y AutoMapper) y agrega seguridad con JWT. Misma base de datos `ProductosDB`.

## Qué se agregó en cada capa

| Capa | Piezas nuevas |
|---|---|
| `ProductosApi.Entidades` | `Usuario`, `LoginDto`, `RegistroDto`, `TokenDto` |
| `ProductosApi.Datos` | `IUsuarioRepositorio` y `UsuarioRepositorio` (el `ProductosContext` suma `DbSet<Usuario>`) |
| `ProductosApi.Negocio` | `AuthServicio` (hash de la contraseña y generación del token) y `JwtOpciones` |
| `ProductosApi.Api` | `AuthController`, JwtBearer y Swagger con Authorize en `Program.cs`, y `[Authorize]` en `ProductosController` |

## Cómo ejecutarlo

1. Ejecuta `ProductosDB.sql` (S12) y luego `Usuarios.sql` en tu SQL Server.
2. En `ProductosApi.Api/appsettings.json` pon tu clave de `sa` en la cadena de conexión.
3. Desde esta carpeta: `dotnet run --project ProductosApi.Api`
4. Abre `/swagger` con el puerto que indique la consola (por defecto `http://localhost:5055`).

Usuario administrador de prueba: `admin@productos.com` / `Admin123!`. Los usuarios que se registran por la API tienen rol `Usuario`.

## Permisos

| Endpoint | Acceso |
|---|---|
| `POST /api/auth/registro`, `POST /api/auth/login` | Público |
| `GET /api/productos`, `GET /api/productos/{id}` | Público |
| `POST` y `PUT /api/productos` | Cualquier usuario autenticado |
| `DELETE /api/productos/{id}` | Solo rol `Admin` |

Sin token responde 401; con un token válido pero sin el rol requerido, 403.

## Probar con Swagger

1. `POST /api/auth/login` y copia el valor de `token`.
2. Botón **Authorize**: pega el token (sin la palabra Bearer).
3. Ejecuta los endpoints protegidos.

También puedes usar `ProductosApi.Api/ProductosApi.Api.http` o Postman (pestaña Authorization, tipo Bearer Token).

## Notas

- La clave del token (`Jwt:Clave`) está en `appsettings.json` solo para la demo. En un proyecto real va en variables de entorno o user-secrets.
- En .NET el claim del rol viaja con el nombre largo `http://schemas.microsoft.com/ws/2008/06/identity/claims/role`.
- AutoMapper 16 muestra en el log una advertencia de licencia: es normal en desarrollo.
- Los paquetes de EF Core y JwtBearer están fijados en la versión 10 (`10.*`).
