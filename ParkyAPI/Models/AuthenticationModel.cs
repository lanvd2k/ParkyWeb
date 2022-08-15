using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models
{
    public class AuthenticationModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
