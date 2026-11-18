using AutoMapper;
using ProductosApi.Entidades;
using ProductosApi.Entidades.Dtos;

namespace ProductosApi.Negocio;

// PERFIL DE AUTOMAPPER: aquí se declaran TODOS los mapeos, cada uno con su sentido.
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ENTIDAD -> DTO DE LECTURA (sentido de SALIDA: base de datos -> cliente).
        // Producto.Categoria es un objeto y ProductoDto.Categoria es un texto:
        // los nombres coinciden pero los tipos no, así que se configura a mano.
        CreateMap<Producto, ProductoDto>()
            .ForMember(d => d.Categoria,
                o => o.MapFrom(s => s.Categoria!.Nombre));

        // DTO DE ENTRADA -> ENTIDAD (sentido de ENTRADA: cliente -> base de datos).
        // Los nombres coinciden; no hace falta configurar nada.
        // No se declara ReverseMap(): la entidad nunca se convierte en DTO de entrada.
        CreateMap<ProductoCrearDto, Producto>();
    }
}
