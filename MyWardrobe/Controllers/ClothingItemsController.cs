using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWardrobe.Data;
using MyWardrobe.Models;

namespace MyWardrobe.Controllers
{
    public class ClothingItemsController : Controller
    {
        private readonly MyWardrobeContext _context;

        public ClothingItemsController(MyWardrobeContext context)
        {
            _context = context;
        }

        // GET: ClothingItems
        public async Task<IActionResult> Index(int? categoryId)
        {
            // Filter Options for Categories
            ViewData["CategoryId"] = new SelectList(_context.Category.Distinct(), "Id", "Name");

            var myWardrobeContext = _context.ClothingItem.Include(c => c.Brand).Include(c => c.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                myWardrobeContext = myWardrobeContext.Where(c => c.CategoryId == categoryId);
            }

            return View(await myWardrobeContext.ToListAsync());

        }

        // GET: ClothingItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItem = await _context.ClothingItem
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothingItem == null)
            {
                return NotFound();
            }

            return View(clothingItem);
        }

        // GET: ClothingItems/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: ClothingItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,BrandId,Size,Description,ImageFileName")] ClothingItem clothingItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clothingItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", clothingItem.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", clothingItem.CategoryId);
            return View(clothingItem);
        }

        // GET: ClothingItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItem = await _context.ClothingItem.FindAsync(id);
            if (clothingItem == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", clothingItem.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", clothingItem.CategoryId);
            return View(clothingItem);
        }

        // POST: ClothingItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,BrandId,Size,Description,ImageFileName")] ClothingItem clothingItem)
        {
            if (id != clothingItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clothingItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClothingItemExists(clothingItem.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", clothingItem.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", clothingItem.CategoryId);
            return View(clothingItem);
        }

        // GET: ClothingItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItem = await _context.ClothingItem
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothingItem == null)
            {
                return NotFound();
            }

            return View(clothingItem);
        }

        // POST: ClothingItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clothingItem = await _context.ClothingItem.FindAsync(id);
            if (clothingItem != null)
            {
                _context.ClothingItem.Remove(clothingItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ClothingItems/AddImage/5
        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clothingItem = await _context.ClothingItem
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothingItem == null)
            {
                return NotFound();
            }

            return View(clothingItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(int id, [Bind("Id,CategoryId,BrandId,Size,Description,ImageFileName")] ClothingItem clothingItem, IFormFile ImageFileName)
        {
            if (id != clothingItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var filename = ImageFileName!.FileName;
                    // Images to be partitioned based on the clothing item's id
                    var destinationFolder = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Images", Convert.ToString(id));
                    var destinationFilePath = Path.Combine(destinationFolder, filename);

                    if (!Directory.Exists(destinationFolder))
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }

                    // Copy the file to the intended destination folder
                    using FileStream fs = new FileStream(destinationFilePath, FileMode.Create);
                    await ImageFileName.CopyToAsync(fs);

                    clothingItem.ImageFileName = filename;

                    _context.Update(clothingItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClothingItemExists(clothingItem.Id))
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
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveImage(int? id)
        {
            // Just a copy and paste of Details() Task thus far
            if (id == null)
            {
                return NotFound();
            }

            var clothingItem = await _context.ClothingItem
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clothingItem == null)
            {
                return NotFound();
            }
            // Remove the image attribute from the clothing item
            else
            {
                try
                {
                    string? fileName = clothingItem.ImageFileName;

                    // Delete the image from the filesystem
                    await DeleteImageAsset(clothingItem.Id, fileName);

                    // Remove the reference to the image file from the clothing item
                    clothingItem.ImageFileName = null;
                    _context.Update(clothingItem);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClothingItemExists(clothingItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> DeleteImageAsset(int id, string? filename)
        {
            var folderPathBase = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Images");

            if (filename == null)
            {
                return BadRequest("This clothing item does not have an imagefile associated with it");
            }

            string idAndfileName = Path.Combine(Convert.ToString(id), filename);

            var filePath = Path.Combine(folderPathBase, idAndfileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    // Asynchronous deletion of the stored file
                    await Task.Run(() => System.IO.File.Delete(filePath));
                }
                catch
                {
                    return StatusCode(500, "An unexpected error occurred.");
                }
            }
            return Ok();
        }

        private bool ClothingItemExists(int id)
        {
            return _context.ClothingItem.Any(e => e.Id == id);
        }
    }
}
