using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TodoApi.Models
{
    /// <summary>
    /// Order Details class to represent the details for an Order
    /// </summary>
    public class OrderDetails
    {
        [Key, ForeignKey("OrderDetailsId")]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public int Quantity { get; set; }
        [Required]
        public int OrderId { get; set; }
    }
}
