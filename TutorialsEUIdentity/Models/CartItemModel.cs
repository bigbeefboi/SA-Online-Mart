using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorialsEUIdentity.Models
{
    public class CartItemModel
    {
        //https://learn.microsoft.com/en-us/aspnet/web-forms/overview/getting-started/getting-started-with-aspnet-45-web-forms/shopping-cart
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }

        public Guid CartId { get; set; }

        public int Quantity { get; set; }

        public DateTime DateCreated { get; set; }

        //fk for product model
        public int ProductID {  get; set; }
        public virtual ProductModel Product { get; set; }
    }
}
