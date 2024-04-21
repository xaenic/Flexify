using System.ComponentModel.DataAnnotations;
namespace Flexify.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your First Name.")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your Last Name.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Please enter username")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Please enter your email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter your password.")]
        public string? Password { get; set; }
    }
}
