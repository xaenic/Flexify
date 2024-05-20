using System.ComponentModel.DataAnnotations.Schema;

namespace Flexify.Models
{
    public class Theme
    {
        public int Id { get; set; }


        public string? SelectedTheme { get; set; }


        public int user_Id { get; set; }

    }
}
