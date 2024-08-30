using TutorialsEUIdentity.Models;

namespace TutorialsEUIdentity.ViewModels
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; }

        public int CartItemTotal { get; set; }
    }
}
