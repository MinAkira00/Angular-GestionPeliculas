using System.Threading.Tasks;
using backEnd.Entidades;
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
        public GeneroController(ILogger<GeneroController> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Genero>>>Get() {
           return await context.Generos.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int Id) {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Genero genero) {
            context.Add(genero);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genero genero)
        {
            throw new NotImplementedException();
        }
        [HttpDelete]
        public ActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
