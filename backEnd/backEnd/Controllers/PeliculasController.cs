using AutoMapper;
using backEnd.DTOs;
using backEnd.Entidades;
using backEnd.Utilidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backEnd.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDto>> Get(int id) {
            var pelicula = await context.Peliculas
                    .Include(x => x.PeliculasGeneros).ThenInclude(x => x.Genero)
                    .Include(x => x.PeliculasActores).ThenInclude(x => x.Actor)
                    .Include(x => x.PeliculasCines).ThenInclude(x => x.Cine)
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null) { return NotFound(); }

            var dto = mapper.Map<PeliculaDto>(pelicula);
            dto.Actores = dto.Actores.OrderBy(x => x.Orden).ToList();
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDto peliculaCreacionDto) {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDto);

            if (peliculaCreacionDto.Poster != null)
            {
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDto.Poster);
            }

            EscribirOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpGet("postget")]
        public async Task<ActionResult<PeliculasPostGetDto>> PostGet() {
            var cines = await context.Cines.ToListAsync();
            var generos = await context.Generos.ToListAsync();

            var cinesDTO = mapper.Map<List<CineDto>>(cines);
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);

            return new PeliculasPostGetDto() { Cines = cinesDTO, Generos = generosDTO };
            
        }

        private void EscribirOrdenActores(Pelicula pelicula) {
            if (pelicula.PeliculasActores != null) {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++) {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }
    }
}
