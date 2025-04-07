using System.ComponentModel.DataAnnotations;
using backEnd.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace backEnd.DTOs
{
    public class PeliculaCreacionDto
    {
        [Required]
        [StringLength(maximumLength: 300)]
        public string? Titulo { get; set; }
        public string Resumen { get; set; }
        public string Trailer { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public IFormFile? Poster { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CinesIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaCreacionDto>>))]
        public List<ActorPeliculaCreacionDto> Actores { get; set; }
    }
}
