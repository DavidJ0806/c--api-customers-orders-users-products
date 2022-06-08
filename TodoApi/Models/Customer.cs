using TodoApi.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TodoApi.Models
{
    /// <summary>
    /// Customer class to represent customers
    /// </summary>
    public class Customer
    {
        [Key, ForeignKey("CustomerId")]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public CustomerAddress? CustomerAddress { get; set; }
    }
}