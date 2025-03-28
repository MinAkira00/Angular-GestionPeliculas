using System.ComponentModel.DataAnnotations;
using backEnd.Validaciones;

namespace backEnd.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength:10)]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; }
        
        [Range(18,120)]
        public int Edad { get; set; }
        [CreditCard]
        public string TarjetaDeCredito { get; set; }
        [Url]
        public string URL { get; set; }

    }
}
