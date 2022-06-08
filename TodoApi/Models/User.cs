using System.ComponentModel;
using TodoApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    /// <summary>
    /// User class to represent users
    /// </summary>
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        [Roles]
        public string? Roles { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
    }
}