using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMarked.Data;
using MvcMarked.Models;

namespace MvcMarked.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly MvcMarkedContext _context;

        public ProductsController(MvcMarkedContext context)
        {
            _context = context;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index(string ProductCategory, string searchString)
        {
           //Use LINQ to get list of genres.
           var categoryQuery = from m in _context.Category
                                              orderby m.CategoryName
                                              select m.CategoryName;

            var products = from m in _context.Product
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Text.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(ProductCategory))
            {
                products = products.Where(p => p.Category.CategoryName == ProductCategory);
            }

            var productCategoryMV = new ProductCategoriesViewModel
            {
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Products = await products.ToListAsync()
            };

            return View(productCategoryMV);
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        
        public IActionResult Create()
        {
            //var categoryQuery = from m in _context.Category
            //                    orderby m.CategoryName
            //                    select m.CategoryName;
            //var productCategoryMV = new ProductCategoriesViewModel
            //{
            //    Categories = new SelectList(categoryQuery.Distinct().ToList()),

            //};
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Link,Text,PhoneNr,CategoryId,Price,UserName")] Product product)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(product);

                //var categoryQuery = from m in _context.Category
                //                    orderby m.CategoryName
                //                    select m.CategoryName;
                //var productCategoryMV = new ProductCategoriesViewModel
                //{
                //    Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                    
                //};
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (product.UserName != User.Identity.Name)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Link,Text,PhoneNr,DateCreated,Price,UserName,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid && product.UserName== User.Identity.Name)
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
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "AdminAndDelete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
