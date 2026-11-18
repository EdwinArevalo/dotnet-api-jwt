namespace ProductosApi.Negocio;

// El servicio no conoce HTTP: cuando se rompe una regla lanza esta excepción
// y el controlador decide qué respuesta dar (aquí, 409 Conflict).
public class ReglaNegocioException(string mensaje) : Exception(mensaje);
