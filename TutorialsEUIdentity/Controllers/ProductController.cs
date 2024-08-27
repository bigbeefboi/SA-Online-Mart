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

namespace TutorialsEUIdentity.Controllers
{
    public class ProductController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IProductRepositoryService _productRepositoryService;

        private readonly ApplicationDbContext _context;

        public object ImageFile { get; private set; }

        public ProductController(ApplicationDbContext context, IFileService fService, IProductRepositoryService pRepoService)
        {
            _fileService = fService;
            _productRepositoryService = pRepoService;
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Image,Category")] ProductModel productModel, IFormFile Image)
        {
            if(Image != null)
            {
                var fileResult = _fileService.SaveImage(Image);
                if(fileResult.Item1 == 1)
                {
                    productModel.Image = fileResult.Item2;
                }
            }
            var productResult = _productRepositoryService.Add(productModel);

            if(productResult)
            {
                Console.WriteLine("Add successfull");
            }
            else
            {
                Console.WriteLine("Add unsuccessfull");
            }
            if (ModelState.IsValid)
            {
                _context.Add(productModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));

            //return View(productModel);
            
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Product.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            return View(productModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Image,Category")] ProductModel productModel, IFormFile Image)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }

            ModelState.Remove("ImageFile");
            ModelState.Remove("Image");
            if (ModelState.IsValid)
            {
                if(ImageFile == null)
                {
                    var existingProduct = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    productModel.Image = existingProduct.Image;
                }
                else
                {
                    var existingProduct = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    _fileService.DeleteImage(existingProduct.Image);
                    var fileResult = _fileService.SaveImage(Image);

                    if(fileResult.Item1 == 1)
                    {
                        productModel.Image = fileResult.Item2;
                    }
                    var productResult = _productRepositoryService.Add(productModel);
                }

                try
                {
                    var existingProduct = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.Id))
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
            return View(productModel);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productModel = await _context.Product.FindAsync(id);
            if (productModel != null)
            {
                var fileResult = _fileService.DeleteImage(productModel.Image);
                _context.Product.Remove(productModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductModelExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
