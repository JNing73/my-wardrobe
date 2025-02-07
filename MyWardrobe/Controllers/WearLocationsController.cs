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
    public class WearLocationsController : Controller
    {
        private readonly MyWardrobeContext _context;

        public WearLocationsController(MyWardrobeContext context)
        {
            _context = context;
        }

        // GET: WearLocations
        public async Task<IActionResult> Index()
        {
            return View(await _context.WearLocation.ToListAsync());
        }

        // GET: WearLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wearLocation = await _context.WearLocation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wearLocation == null)
            {
                return NotFound();
            }

            return View(wearLocation);
        }

        // GET: WearLocations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WearLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] WearLocation wearLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wearLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wearLocation);
        }

        // GET: WearLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wearLocation = await _context.WearLocation.FindAsync(id);
            if (wearLocation == null)
            {
                return NotFound();
            }
            return View(wearLocation);
        }

        // POST: WearLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] WearLocation wearLocation)
        {
            if (id != wearLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wearLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WearLocationExists(wearLocation.Id))
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
            return View(wearLocation);
        }

        // GET: WearLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wearLocation = await _context.WearLocation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wearLocation == null)
            {
                return NotFound();
            }

            return View(wearLocation);
        }

        // POST: WearLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wearLocation = await _context.WearLocation.FindAsync(id);
            if (wearLocation != null)
            {
                _context.WearLocation.Remove(wearLocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WearLocationExists(int id)
        {
            return _context.WearLocation.Any(e => e.Id == id);
        }
    }
}
