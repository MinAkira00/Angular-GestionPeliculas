using System.ComponentModel.DataAnnotations;
using backEnd.Validaciones;

namespace backEnd.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength:50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public List<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}
