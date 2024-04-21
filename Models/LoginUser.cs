using System.ComponentModel.DataAnnotations;
namespace Flexify.Models
{
    public class LoginUser
    {

        [Required(ErrorMessage = "Please enter your email address.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Please enter your password.")]
        public string? Password { get; set; }


    }
}
