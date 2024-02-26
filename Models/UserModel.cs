using System.ComponentModel.DataAnnotations;
namespace Flexify.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Please enter your First Name.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your Last Name.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your email address.")]
        public string email { get; set; }
        [Required(ErrorMessage = "Please enter your password.")]
        public string password { get; set; }
    }
}
