﻿using System.ComponentModel.DataAnnotations;

namespace backEnd.DTOs
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 200)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public IFormFile? Foto { get; set; }
        public string Biografia { get; set; }
    }
}
