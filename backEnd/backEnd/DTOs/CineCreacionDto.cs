using System.ComponentModel.DataAnnotations;

namespace backEnd.DTOs
{
    public class CineCreacionDto
    {
        [Required]
        [StringLength(maximumLength: 75)]
        public string Nombre { get; set; }
    }
}
