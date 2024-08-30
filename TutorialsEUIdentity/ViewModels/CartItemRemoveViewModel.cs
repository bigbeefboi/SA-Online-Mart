using TutorialsEUIdentity.Models;

namespace TutorialsEUIdentity.ViewModels
{
    public class CartItemRemoveViewModel
    {
        public string Message { get; set; }
        public int CartItemTotal { get; set; }
        public int CartCount { get; set; }
        public int CartItemCount { get; set; }
        public int DeleteID { get; set; }

    }
}
