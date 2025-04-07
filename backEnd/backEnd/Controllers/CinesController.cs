using AutoMapper;
using backEnd.DTOs;
using backEnd.Entidades;
using backEnd.Migrations;
using backEnd.Utilidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Controllers
{
    
    [Route("api/cines")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class CinesController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        public readonly IMapper mapper;

        public CinesController(ApplicationDbContext context, IMapper mapper) {
        
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CineDto>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = context.Cines.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var cines = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<CineDto>>(cines);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<CineDto>> Get(int Id)
        {
            var cine = await context.Cines.FirstOrDefaultAsync(x => x.Id == Id);

            if (cine == null)
            {
                return NotFound();
            }

            return mapper.Map<CineDto>(cine);

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreacionDto cineCreacionDTO)
        {
            var cine = mapper.Map<Cine>(cineCreacionDTO);
            context.Add(cine);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int Id, [FromBody] CineCreacionDto cineCreacionDTO)
        {
            var cine = await context.Cines.FirstOrDefaultAsync(x => x.Id == Id);

            if (cine == null)
            {
                return NotFound();
            }
            cine = mapper.Map(cineCreacionDTO, cine);
            await context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var cine = await context.Cines.FirstOrDefaultAsync(x => x.Id == Id);

            if (cine == null)
            {
                return NotFound();
            }

            context.Remove(cine);
            await context.SaveChangesAsync();

            //var sql ="DBCC CHECKIDENT ('Cines', RESEED, 0);";
            //await.context.Database.ExecuteSqlRawAsync(sql);
            return NoContent();
        }
    }
}
