using System.ComponentModel.DataAnnotations;

namespace backEnd.DTOs
{
    public class CredencialesUsuaio
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
