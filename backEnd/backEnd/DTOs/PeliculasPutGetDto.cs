namespace backEnd.DTOs
{
    public class PeliculasPutGetDto
    {
        public PeliculaDto Pelicula { get; set; }
        public List<GeneroDTO> GenerosSeleccionados { get; set; }
        public List<GeneroDTO> GenerosNoSeleccionados {get; set; }
        public List<CineDto> CinesSeleccionados {get; set; }
        public List<CineDto> CinesNoSeleccionados { get;set; }
        public List<PeliculaActorDto> Actores { get; set; }
    }
}
