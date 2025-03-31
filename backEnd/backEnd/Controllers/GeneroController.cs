using System.Threading.Tasks;
using AutoMapper;
using backEnd.DTOs;
using backEnd.Entidades;
using backEnd.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly ILogger<GeneroController> logger;
        private readonly ApplicationDbContext context;
        IMapper mapper;
        public GeneroController(ILogger<GeneroController> logger, ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) {
           var queryable = context.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<GeneroDTO>> Get(int Id) {
           var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);

            if (genero == null) {
                return NotFound(); 
            }

            return mapper.Map<GeneroDTO>(genero);  
        
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO) {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int Id,[FromBody] GeneroCreacionDTO generoCreacionDTO){
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Id == Id);

            if (genero == null)
            {
                return NotFound();
            }
            genero = mapper.Map(generoCreacionDTO, genero);
            await context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == Id);

            if (!existe) {
                return NotFound();
            }

            context.Remove(new Genero() { Id = Id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
