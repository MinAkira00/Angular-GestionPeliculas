using backEnd.Entidades;
using backEnd.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace backEnd.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        private readonly IRepositorio repositorio;
        private readonly WeatherForecastController weatherForecastController;
        //private readonly ILogger<GeneroController> logger;

        public GeneroController(IRepositorio repositorio,
            WeatherForecastController weatherForecastController
            )
        { //ILogger<GeneroController> logger
            this.repositorio = repositorio;
            this.weatherForecastController = weatherForecastController;
            //this.logger = logger;
        }
        [HttpGet("listado")]
        public ActionResult<List<Genero>>Get() {
            //logger.LogInformation("Vamos a mostar los generos");
            return repositorio.ObtenerTodosLosGeneros();
        }
        [HttpGet("guid")]
        public ActionResult<Guid> GetGUID()
        {
            return Ok(new
            {
                Guid_GenerosController = repositorio.ObtenerGuid(),
                Guid_WeatherForecastController = weatherForecastController.ObtenerGuidWeatherForecastController()
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int Id,[FromHeader] string Nombre) {

            //logger.LogDebug($"Obteniendo un genero por el id {Id}");
            var genero = await repositorio.ObtenerPorId(Id);

            if (genero == null) {
                //logger.LogWarning($"No pudimos encontrar el genero de id {Id}");
                return NotFound();
            }
            
            return genero;
        }
        
        [HttpPost]
        public ActionResult Post([FromBody] Genero genero) {
            repositorio.CrearGenero(genero);
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genero genero)
        {
            return NoContent();
        }
        [HttpDelete]
        public ActionResult Delete()
        {
            return NoContent();
        }
    }
}
