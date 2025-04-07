using AutoMapper;
using backEnd.DTOs;
using backEnd.Entidades;
using Microsoft.AspNetCore.Identity;

namespace backEnd.Utilidades
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<Cine, CineDto>().ReverseMap();
            CreateMap<CineCreacionDto, Cine>();

            CreateMap<PeliculaCreacionDto, Pelicula>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.PeliculasGeneros, options => options.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.PeliculasCines, options => options.MapFrom(MapearPeliculasCines))
                .ForMember(x => x.PeliculasActores, options => options.MapFrom(MapearPeliculasActores));
            CreateMap<Pelicula, PeliculaDto>()
                .ForMember(x => x.Generos, options => options.MapFrom(MapearPeliculasGeneros))
                .ForMember(x => x.Actores, options => options.MapFrom(MapearPeliculasActores))
                .ForMember(x => x.Cines, options => options.MapFrom(MapearPeliculasCines));

            CreateMap<IdentityUser, UsuarioDto>();
        }

        private List<CineDto> MapearPeliculasCines(Pelicula pelicula, PeliculaDto peliculaDto)
        {

            var resultado = new List<CineDto>();

            if (pelicula.PeliculasCines != null)
            {
                foreach (var peliculasCines in pelicula.PeliculasCines)
                {
                    resultado.Add(new CineDto()
                    {
                        Id = peliculasCines.CineId,
                        Nombre = peliculasCines.Cine.Nombre
                    });
                }
            }
            return resultado;
        }
        private List<PeliculaActorDto> MapearPeliculasActores(Pelicula pelicula, PeliculaDto peliculaDto)
        {

            var resultado = new List<PeliculaActorDto>();

            if (pelicula.PeliculasActores != null)
            {
                foreach (var actorPeliculas in pelicula.PeliculasActores)
                {
                    resultado.Add(new PeliculaActorDto() 
                    { 
                        Id = actorPeliculas.ActorId, 
                        Nombre = actorPeliculas.Actor.Nombre, 
                        Foto = actorPeliculas.Actor.Foto, 
                        Orden = actorPeliculas.Orden, 
                        Personaje = actorPeliculas.Personaje 
                    });
                }
            }
            return resultado;
        }


        private List<GeneroDTO> MapearPeliculasGeneros(Pelicula pelicula, PeliculaDto peliculaDto)
        {

            var resultado = new List<GeneroDTO>();

            if (pelicula.PeliculasGeneros != null) {
                foreach (var genero in pelicula.PeliculasGeneros) {
                    resultado.Add(new GeneroDTO { Id = genero.GeneroId, Nombre = genero.Genero.Nombre });
                }
            }
            return resultado;
        }

        private List<PeliculasActores> MapearPeliculasActores(PeliculaCreacionDto peliculaCreacionDto, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();

            if (peliculaCreacionDto.Actores == null)
            {
                return resultado;
            }

            foreach (var actor in peliculaCreacionDto.Actores)
            {
                resultado.Add(new PeliculasActores() { ActorId = actor.Id, Personaje = actor.Personaje});
            }

            return resultado;
        }

        private List<PeliculasGeneros> MapearPeliculasGeneros(PeliculaCreacionDto peliculaCreacionDto, Pelicula pelicula) {
            var resultado = new List<PeliculasGeneros>();

            if (peliculaCreacionDto.GenerosIds == null) {
                return resultado;
            }

            foreach (var id in peliculaCreacionDto.GenerosIds) {
                resultado.Add(new PeliculasGeneros() { GeneroId = id});
            }

            return resultado;
        }

        private List<PeliculasCines> MapearPeliculasCines(PeliculaCreacionDto peliculaCreacionDto, Pelicula pelicula)
        {
            var resultado = new List<PeliculasCines>();

            if (peliculaCreacionDto.CinesIds == null)
            {
                return resultado;
            }

            foreach (var id in peliculaCreacionDto.CinesIds)
            {
                resultado.Add(new PeliculasCines() { CineId = id });
            }

            return resultado;
        }

    }
}
