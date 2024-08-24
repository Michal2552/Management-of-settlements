using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Settlement_pm.Context;
using Settlement_pm.Models;

namespace Settlement_pm.Controllers
{
    public class SettlementsController : Controller
    {
        private readonly SettlementDbContext _context;

        public SettlementsController(SettlementDbContext context)
        {
            _context = context;
        }

        // GET: Settlements
        //חיפוש+ הפרדת דפים- עד חמש בעמוד + מיון בסדר עולה ויורד
        public async Task<IActionResult> Index(string searchString, string sortOrder, int page = 1)
        {
            int pageSize = 5; 

            
            var settlements = from s in _context.Settlements
                              select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                settlements = settlements.Where(s => s.SettlementName.Contains(searchString));
            }

            
            switch (sortOrder)
            {
                case "name_desc":
                    settlements = settlements.OrderByDescending(s => s.SettlementName);
                    break;
                default:
                    settlements = settlements.OrderBy(s => s.SettlementName);
                    break;
            }

            
            var pagedSettlements = await PaginatedList<Settlement>.CreateAsync(
                settlements.AsNoTracking(), page, pageSize);

            
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            return View(pagedSettlements);
        }


        // GET: Settlements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Settlements == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settlement == null)
            {
                return NotFound();
            }

            return View(settlement);
        }

        // GET: Settlements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Settlements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SettlementName")] Settlement settlement)
        {
            if (ModelState.IsValid)
            {
                // בדיקה אם שם היישוב כבר קיים
                bool exists = _context.Settlements.Any(s => s.SettlementName == settlement.SettlementName);
                if (exists)
                {
                    ModelState.AddModelError("SettlementName", "השם הזה כבר קיים במערכת");
                    return View(settlement);
                }

                _context.Add(settlement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(settlement);
        }

        // GET: Settlements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Settlements == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlements.FindAsync(id);
            if (settlement == null)
            {
                return NotFound();
            }
            return View(settlement);
        }

        // POST: Settlements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SettlementName")] Settlement settlement)
        {
            if (id != settlement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // בדיקה אם שם היישוב כבר קיים (למקרה שמשנים שם)
                    bool exists = _context.Settlements.Any(s => s.SettlementName == settlement.SettlementName && s.Id != settlement.Id);
                    if (exists)
                    {
                        ModelState.AddModelError("SettlementName", "השם הזה כבר קיים במערכת");
                        return View(settlement);
                    }

                    _context.Update(settlement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettlementExists(settlement.Id))
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
            return View(settlement);
        }

        // GET: Settlements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Settlements == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settlement == null)
            {
                return NotFound();
            }

            return View(settlement);
        }

        // POST: Settlements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Settlements == null)
            {
                return Problem("Entity set 'SettlementDbContext.Settlements'  is null.");
            }
            var settlement = await _context.Settlements.FindAsync(id);
            if (settlement != null)
            {
                _context.Settlements.Remove(settlement);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SettlementExists(int id)
        {
            return (_context.Settlements?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
