using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorialsEUIdentity.Data;
using TutorialsEUIdentity.Models;

namespace TutorialsEUIdentity.Controllers
{
    public class CartItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CartItem
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CartItems.Include(c => c.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CartItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItemModel = await _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartItemId == id);
            if (cartItemModel == null)
            {
                return NotFound();
            }

            return View(cartItemModel);
        }

        // GET: CartItem/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "Id", "Id");
            return View();
        }

        // POST: CartItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartItemId,CartId,Quantity,DateCreated,ProductID")] CartItemModel cartItemModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItemModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "Id", "Id", cartItemModel.ProductID);
            return View(cartItemModel);
        }

        // GET: CartItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItemModel = await _context.CartItems.FindAsync(id);
            if (cartItemModel == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "Id", "Id", cartItemModel.ProductID);
            return View(cartItemModel);
        }

        // POST: CartItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartItemId,CartId,Quantity,DateCreated,ProductID")] CartItemModel cartItemModel)
        {
            if (id != cartItemModel.CartItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItemModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemModelExists(cartItemModel.CartItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "Id", "Id", cartItemModel.ProductID);
            return View(cartItemModel);
        }

        // GET: CartItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItemModel = await _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartItemId == id);
            if (cartItemModel == null)
            {
                return NotFound();
            }

            return View(cartItemModel);
        }

        // POST: CartItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItemModel = await _context.CartItems.FindAsync(id);
            if (cartItemModel != null)
            {
                _context.CartItems.Remove(cartItemModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemModelExists(int id)
        {
            return _context.CartItems.Any(e => e.CartItemId == id);
        }
    }
}
