using backEnd.DTOs;
using backEnd.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingsController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;

        public RatingsController(UserManager<IdentityUser> userManager, ApplicationDbContext context) {
            this.userManager = userManager;
            this.context =context;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] RatingDto ratingDto) {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email").Value;
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;

            var ratingActual = await context.Ratings
                .FirstOrDefaultAsync(x => x.PeliculaId == ratingDto.PeliculaId
                && x.UsuarioId == usuarioId);

            if (ratingActual == null)
            {
                var rating = new Rating();
                rating.PeliculaId = ratingDto.PeliculaId;
                rating.Puntuacion = ratingDto.Puntuacion;
                rating.UsuarioId = usuarioId;
                context.Add(rating);
            }
            else {
                ratingActual.Puntuacion = ratingDto.Puntuacion;
            }
            await context.SaveChangesAsync();
            return NoContent();

        }
    }
}
