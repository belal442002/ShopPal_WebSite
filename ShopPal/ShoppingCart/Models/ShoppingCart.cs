using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    [Table("ShoppingCart")]
    public class Shopping_Cart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String? UserId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
        public virtual ICollection<CartDetail>? CartDetails { get; set; }
    }
}
