using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
namespace Flexify.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public byte[]? Image { get; set; }
        public int UserId { get; set; }
    }
}
