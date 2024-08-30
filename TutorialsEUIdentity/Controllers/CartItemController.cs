using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorialsEUIdentity.Data;
using TutorialsEUIdentity.Models;
using TutorialsEUIdentity.Services;
using TutorialsEUIdentity.ViewModels;

namespace TutorialsEUIdentity.Controllers
{
    public class CartItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        // used for components that are statically rendered on the server
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartItemController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: CartItem
        public ActionResult Index()
        {
            var cart = CartItemActions.GetCart(_context, _httpContextAccessor);

            var viewModel = new CartItemViewModel
            {
                CartItems = cart.GetCartItems(),
                CartItemTotal = cart.GetCartTotal()
            };

            ViewData["ItemQuantity"] = cart.GetCartItems().Count();
            return View(viewModel);
          
        }

        public ActionResult CartItemSummary()
        {
            var cart = CartItemActions.GetCart(_context, _httpContextAccessor);
            ViewData["ItemQuantity"] =cart.GetItemCount();
            return PartialView("ItemOverview");
        }

        public IActionResult AddItemToCart(int pId)
        {
           
            var addedProduct = _context.Product.SingleOrDefault(product => product.Id == pId);

           
            if (addedProduct == null)
            {
              
                return NotFound(); 
            }

            var cart = CartItemActions.GetCart(_context, _httpContextAccessor);

            // Add the product to the cart
            cart.AddToCart(addedProduct);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveItemFromCart(int id)
        {
            var cart = CartItemActions.GetCart(_context, _httpContextAccessor);

            var remainingItems = cart.RemoveItemFromCart(id); 

            if (remainingItems == 0)
            {
                
                return RedirectToAction("Index");
            }

           
            return RedirectToAction("Index");
        }

        //clears cart
        public IActionResult ClearEntireCart()
        {
            var cart = CartItemActions.GetCart(_context, _httpContextAccessor);

            cart.ClearCart();

            return RedirectToAction("Index");
        }


        /*
        public async Task<IActionResult> RemoveItemFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cartModel =await _context.CartItems.FindAsync(id);
            if (cartModel != null)
            {
                _context.CartItems.Remove(cartModel);
            }
            await _context.SaveChangesAsync();
            return RedirectToActionPermanent(nameof(Index));
        }
        */

        private bool CartItemModelExists(int id)
        {
            return _context.CartItems.Any(e => e.CartItemId == id);
        }
    }
}
