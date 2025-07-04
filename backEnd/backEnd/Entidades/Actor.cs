﻿using System.ComponentModel.DataAnnotations;

namespace backEnd.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:200)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }
        public string Biografia { get; set; }

        public List<PeliculasActores> PeliculasActores { get; set; }

    }   
}
