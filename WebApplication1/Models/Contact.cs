using System.ComponentModel.DataAnnotations;
using WebApplication1.ValidationAttributes;

namespace WebApplication1.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Email]
        public string Email { get; set; }
    }
}
