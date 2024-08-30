using Microsoft.EntityFrameworkCore;
using TutorialsEUIdentity.Data;
using TutorialsEUIdentity.Models;

namespace TutorialsEUIdentity.Services
{
    public partial class CartItemActions
    {
        // partial class
        // split the definition of a class into multiple files
        ApplicationDbContext _context;

        Guid SCartId { get; set; }

        IHttpContextAccessor _httpContextAccessor;

        public const string SessionKey = "CartId";

        public CartItemActions(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public static CartItemActions GetCart(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            var cart = new CartItemActions(context, httpContextAccessor);
            cart.SCartId = Guid.Parse(cart.GetCartId());
            return cart;
        }

        public void AddToCart(ProductModel productModel)
        {
            var cartItem = _context.CartItems.SingleOrDefault(
                c => c.CartId == SCartId
                && c.ProductID == productModel.Id
                );
            if( cartItem == null )
            {
                cartItem = new CartItemModel
                {
                    ProductID = productModel.Id,
                    CartId = SCartId,
                    Quantity = 1,
                    DateCreated = DateTime.Now,
                    Product = productModel,
                };
                _context.CartItems.Add( cartItem );
            }
            else
            {
                cartItem.Quantity++;
            }
            _context.SaveChanges();
        }

        public string GetCartId()
        {
            var context = _httpContextAccessor.HttpContext;
            var session = context.Session;

            var cartId = session.GetString(SessionKey);
            
            if(string.IsNullOrEmpty(cartId))
            {
                cartId = Guid.NewGuid().ToString();
                session.SetString(SessionKey, cartId);
            }
            return context.Session.GetString(SessionKey).ToString();
        }

        public int GetItemCount()
        {
            var count = _context.CartItems
                .Where(cartItems => cartItems.CartId == SCartId)
                .Sum(cartItems => (int?)cartItems.Quantity);
            return count ?? 0;
        }

        public List<CartItemModel> GetCartItems()
        {
            return _context.CartItems
                .Include(cartItems => cartItems.Product)
                .Where(cartItems => cartItems.CartId == SCartId)
                .ToList();
        }

        public int GetCartTotal()
        {
            int? total = (from cartItems in _context.CartItems
                          where cartItems.CartId == SCartId
                          select (int?)cartItems.Quantity *
                          cartItems.Product.Price).Sum();
            return total ?? int.MaxValue;
        }
        public void ClearCart()
        {
            var cartItems = _context.CartItems.Where(
                cart => cart.CartId == SCartId);

            foreach(var cartItem in cartItems)
            {
                _context.CartItems.Remove(cartItem);
            }
            _context.SaveChanges();
        }

        public int RemoveItemFromCart(int id)
        {
            var cartItem = _context.CartItems.Single(
                cart => cart.CartId == SCartId
                && cart.CartItemId == id);

            int count = 0;

            if(cartItem != null)
            {
                if(cartItem.Quantity > 1)
                {
                    cartItem.Quantity --;
                    count = cartItem.Quantity;
                }
                else
                {
                    _context.Remove(cartItem);
                }
                _context.SaveChanges();
            }
            return count;
        }

        /*
         start order
          */
        
    }
}
