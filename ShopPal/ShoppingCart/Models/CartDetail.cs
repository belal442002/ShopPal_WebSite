using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("ShoppingCart")]
        public int ShoppingCartId { get; set; }
        public virtual Shopping_Cart? ShoppingCart { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }
        public double Price{ get; set; }
        public double TotalPrice { get; set; }
    }
}
