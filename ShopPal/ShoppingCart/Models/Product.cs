using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20,ErrorMessage ="Max length is 20 letters")]
        [MinLength(2,ErrorMessage ="Min length is 2 letters")]
        public String? Name { get; set; }
        [MaxLength(250,ErrorMessage ="Max length is 250 letters")]
        public String? Description { get; set; }
        [Required]
        public double Price { get; set; }
        public String? Image { get; set; }
        [ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<CartDetail>? CartDetails { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        [NotMapped]
        public String? CategoryName { get; set; }
    }
}
