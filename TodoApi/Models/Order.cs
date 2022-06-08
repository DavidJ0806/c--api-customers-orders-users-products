using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.Attributes;

namespace TodoApi.Models
{
    /// <summary>
    /// Order class to represent orders
    /// </summary>
    public class Order
    {
        [Key, ForeignKey("OrderId")]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        [RegularExpression("^\\d{4}\\-(0[1-9]|1[012])\\-(0[1-9]|[12][0-9]|3[01])$")]
        public string? Date { get; set; }
        [Required]
        [Price]
        public decimal OrderTotal { get; set; }
        [Required]
        public OrderDetails? OrderDetails { get; set; }
    }
}