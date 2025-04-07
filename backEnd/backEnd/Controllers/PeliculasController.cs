using AutoMapper;
using backEnd.DTOs;
using backEnd.Entidades;
using backEnd.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backEnd.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly UserManager<IdentityUser> userManager;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageDto>> Get() {

            var top = 6;
            var hoy = DateTime.Today;

            var proximosEstrenos = await context.Peliculas
                .Where(x => x.FechaLanzamiento > hoy)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var enCines = await context.Peliculas
                .Where(x => x.EnCines)
                .OrderBy(x => x.FechaLanzamiento)
                .Take(top)
                .ToListAsync();

            var resultado = new LandingPageDto();
            resultado.ProximosEstrenos = mapper.Map<List<PeliculaDto>>(proximosEstrenos);
            resultado.EnCines = mapper.Map<List<PeliculaDto>>(enCines);

            return resultado;
        }


        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaDto>> Get(int id) {
            var pelicula = await context.Peliculas
                    .Include(x => x.PeliculasGeneros).ThenInclude(x => x.Genero)
                    .Include(x => x.PeliculasActores).ThenInclude(x => x.Actor)
                    .Include(x => x.PeliculasCines).ThenInclude(x => x.Cine)
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null) { return NotFound(); }
            var promedioVoto = 0.0;
            var usuarioVoto = 0;
            if (await context.Ratings.AnyAsync(x => x.PeliculaId == id)) {
                promedioVoto = await context.Ratings.Where(x => x.PeliculaId == id)
                     .AverageAsync(x => x.Puntuacion);
                
                if (HttpContext.User.Identity.IsAuthenticated) {
                    var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
                    var usuario = await userManager.FindByEmailAsync(email);
                    var usuarioId = usuario.Id;
                    var ratingDB = await context.Ratings.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.PeliculaId == id);

                    if (ratingDB != null) {
                        usuarioVoto = ratingDB.Puntuacion;
                    }
                }
            }

            var dto = mapper.Map<PeliculaDto>(pelicula);
            dto.VotoUsuario = usuarioVoto;
            dto.PromedioVoto = promedioVoto;
            dto.Actores = dto.Actores.OrderBy(x => x.Orden).ToList();
            return dto;
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaDto>>> Filtrar([FromQuery] PeliculasFiltrarDto peliculasFiltrarDto) {
            var peliculasQueryable = context.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(peliculasFiltrarDto.Titulo)) {
                peliculasQueryable = peliculasQueryable.Where(x => x.Titulo.Contains(peliculasFiltrarDto.Titulo));
            }

            if (peliculasFiltrarDto.EnCines) {
                peliculasQueryable = peliculasQueryable.Where(x => x.EnCines);
            }

            if (peliculasFiltrarDto.ProximosEstrenos) {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(x => x.FechaLanzamiento > hoy);

            }
            if (peliculasFiltrarDto.GeneroId != 0) {
                peliculasQueryable = peliculasQueryable
                    .Where(x => x.PeliculasGeneros.Select(y => y.GeneroId)
                    .Contains(peliculasFiltrarDto.GeneroId));
            }
            await HttpContext.InsertarParametrosPaginacionEnCabecera(peliculasQueryable);
            var peliculas = await peliculasQueryable.Paginar(peliculasFiltrarDto.Paginacion).ToListAsync();

            return mapper.Map<List<PeliculaDto>>(peliculas);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] PeliculaCreacionDto peliculaCreacionDto) {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDto);

            if (peliculaCreacionDto.Poster != null)
            {
                pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenedor, peliculaCreacionDto.Poster);
            }

            EscribirOrdenActores(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();

            return pelicula.Id;

        }

        [HttpGet("postget")]
        public async Task<ActionResult<PeliculasPostGetDto>> PostGet() {
            var cines = await context.Cines.ToListAsync();
            var generos = await context.Generos.ToListAsync();

            var cinesDTO = mapper.Map<List<CineDto>>(cines);
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);

            return new PeliculasPostGetDto() { Cines = cinesDTO, Generos = generosDTO };

        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<PeliculasPutGetDto>> PutGet(int id) {
            var peliculaActionResult = await Get(id);

            if (peliculaActionResult.Result is NotFoundResult) { return NotFound(); }

            var pelicula = peliculaActionResult.Value;
            var generosSeleccionadosIds = pelicula.Generos.Select(x => x.Id).ToList();
            var generosNoSeleccionados = await context.Generos
                .Where(x => !generosSeleccionadosIds.Contains(x.Id))
                .ToListAsync();

            var cinesSeleccionadosIds = pelicula.Cines.Select(x => x.Id).ToList();
            var cinesNoSeleccionados = await context.Cines
                .Where(x => !cinesSeleccionadosIds.Contains(x.Id))
                .ToListAsync();

            var generosNoSeleccionadosDto = mapper.Map<List<GeneroDTO>>(generosNoSeleccionados);
            var cinesNoSeleccionadosDto = mapper.Map<List<CineDto>>(cinesNoSeleccionados);

            var respuesta = new PeliculasPutGetDto();
            respuesta.Pelicula = pelicula;
            respuesta.GenerosSeleccionados = pelicula.Generos;
            respuesta.GenerosNoSeleccionados = generosNoSeleccionadosDto;
            respuesta.CinesSeleccionados = pelicula.Cines;
            respuesta.CinesNoSeleccionados = cinesNoSeleccionadosDto;
            respuesta.Actores = pelicula.Actores;

            return respuesta;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDto peliculaCreacionDto) {
            var pelicula = await context.Peliculas
                .Include(x => x.PeliculasActores)
                .Include(x => x.PeliculasCines)
                .Include(x => x.PeliculasGeneros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null) {
                return NotFound();
            }
            pelicula = mapper.Map(peliculaCreacionDto, pelicula);
            if (peliculaCreacionDto.Poster != null && peliculaCreacionDto.Poster.Length > 0)
            {
                pelicula.Poster = await almacenadorArchivos.EditarArchivo(contenedor, peliculaCreacionDto.Poster, pelicula.Poster);
            }
            EscribirOrdenActores(pelicula);
            await context.SaveChangesAsync();
            return NoContent();

        }

        private void EscribirOrdenActores(Pelicula pelicula) {
            if (pelicula.PeliculasActores != null) {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++) {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {

            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            context.Remove(pelicula);
            await context.SaveChangesAsync();
            await almacenadorArchivos.BorrarArchivos(pelicula.Poster, contenedor);
            return NoContent();
        }
    }
}
