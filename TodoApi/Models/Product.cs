using System.ComponentModel.DataAnnotations;
using TodoApi.Attributes;
namespace TodoApi.Models
{
    /// <summary>
    /// Product class to represent products
    /// </summary>
    public class Product
    {
        public long Id { get; set; }
        [Required]
        public string? Sku { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Manufacturer { get; set; }
        [Required]
        [Price]
        public decimal Price { get; set; }
    }
}