using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using X.PagedList;


namespace StoreFront.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreFrontContext _context;

        public ProductsController(StoreFrontContext context)
        {
            _context = context;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var storeFrontContext = _context.Products.Include(p => p.Category).Include(p => p.Manufacturer);
            return View(await storeFrontContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Discontinued()
        {
            var products = _context.Products
            .Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.OrderProducts);
            return View(await products.ToListAsync());
            //to create this view, we added the action, right click -> add View and selected the List template
        }

        public async Task<IActionResult> TiledProducts(string searchTerm, int categoryId, int page = 1)
        {
            int pageSize = 6;

            var products = _context.Products
                .Include(p => p.Category).Include(p => p.Manufacturer).Include(p => p.OrderProducts)
                .ToList();

            #region Optional Category Filter

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewBag.Category = 0;

            if (categoryId != 0)
            {
                //If the user selected a Category...
                //(1) Filter the Products
                products = products.Where(p => p.CategoryId == categoryId).ToList();

                //(2) Repopulate the Dropdown with the chosen Category selected.
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);

                //(3) Persist the Category in the ViewBag.
                ViewBag.Category = categoryId;
            }

            #endregion

            #region Optional Search Filter

            if (!String.IsNullOrEmpty(searchTerm))
            {
                //If we have a SearchTerm...
                products = products
                    .Where(p =>
                    p.ProductName.ToLower().Contains(searchTerm.ToLower())
                    || p.Manufacturer.ManufacturerName.ToLower().Contains(searchTerm.ToLower())
                    || p.Category.CategoryName.ToLower().Contains(searchTerm.ToLower())
                    || p.ProductDescription.ToLower().Contains(searchTerm.ToLower()))
                    .ToList();
                ViewBag.NbrResults = products.Count;
                ViewBag.SearchTerm = searchTerm;
            }
            else
            {
                //If we don't have a SearchTerm...
                ViewBag.NbrResults = null;
                ViewBag.SearchTerm = null;
            }

            #endregion

            //return View(await products.ToListAsync());
            //return View(products);
            return View(products.ToPagedList(page, pageSize));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.ProductStatusNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductDescription,ProductImage,CategoryId,ManufacturerId,ProductStatus")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", product.ManufacturerId);
            ViewData["ProductStatus"] = new SelectList(_context.ProductStatuses, "ProductStatus1", "ProductStatusName", product.ProductStatus);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", product.ManufacturerId);
            ViewData["ProductStatus"] = new SelectList(_context.ProductStatuses, "ProductStatus1", "ProductStatusName", product.ProductStatus);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductDescription,ProductImage,CategoryId,ManufacturerId,ProductStatus")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", product.ManufacturerId);
            ViewData["ProductStatus"] = new SelectList(_context.ProductStatuses, "ProductStatus1", "ProductStatusName", product.ProductStatus);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.ProductStatusNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'StoreFrontContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
