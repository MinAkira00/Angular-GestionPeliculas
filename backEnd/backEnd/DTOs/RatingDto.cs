using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backEnd.DTOs
{
    public class RatingDto
    {
        public int PeliculaId { get; set; }
        [Range(1,5)]

        [JsonPropertyName("puntuacion")]
        public int Puntuacion { get; set; }
    }
}
