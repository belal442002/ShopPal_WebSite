using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String? UserId { get; set; }
        [Required]
        [ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }
        public virtual OrderStatus? OrderStatus { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
