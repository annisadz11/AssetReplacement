using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssetReplacement.Data;
using AssetReplacement.Models;

namespace AssetReplacement.Controllers
{
    public class AssetScrapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssetScrapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssetScraps
        public IActionResult Index()
        {
            return View();
        }

        // untuk mendapatkan data di table
        //API ENDPOINT
        [HttpGet]
        public IActionResult GetData()
        {
            var AssetScraps = _context.AssetScraps
                .Select(g => new
                {
					id = g.Id,
					type = g.Type,
                    serialNumber = g.SerialNumber,
                    location = g.Location,
                    dateInput = g.DateInput.HasValue ? g.DateInput.Value.ToString("dd MMM yyyy") : null,
                    status = g.Status,
                }).ToList();

            return Json(new { rows = AssetScraps });
        }

        // GET: AssetScraps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetScrap = await _context.AssetScraps
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetScrap == null)
            {
                return NotFound();
            }

            return View(assetScrap);
        }

        // GET: AssetScraps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AssetScraps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,SerialNumber,Location,DateInput,Status")] AssetScrap assetScrap)
        {
            if (ModelState.IsValid)
            {
                assetScrap.Status = null; // Menetapkan nilai Status sebagai null (Waiting for Success)
                _context.Add(assetScrap);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Asset Scrap has been created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(assetScrap);
           }

        // GET: AssetScraps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetScrap = await _context.AssetScraps.FindAsync(id);
            if (assetScrap == null)
            {
                return NotFound();
            }

            return View(assetScrap);
        }

        // POST: AssetScraps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,SerialNumber,Location,DateInput,Status")] AssetScrap assetScrap)
        {
            if (id != assetScrap.Id)
            {
                return NotFound();
            }
            var existingRecord = await _context.AssetScraps.FindAsync(id);
            if (existingRecord == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingRecord.Type = assetScrap.Type;
                    existingRecord.SerialNumber = assetScrap.SerialNumber;
                    existingRecord.Location = assetScrap.Location;
                    existingRecord.DateInput = assetScrap.DateInput;
                    _context.Update(existingRecord);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Asset Scrap has been updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetScrapExists(existingRecord.Id))
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

            return View(assetScrap);
        }

        // GET: AssetScraps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetScrap = await _context.AssetScraps.FirstOrDefaultAsync(m => m.Id == id);
            if (assetScrap == null)
            {
                return NotFound();
            }

            return View(assetScrap);
        }

        // POST: AssetScraps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assetScrap = await _context.AssetScraps.FindAsync(id);
            if (assetScrap == null)
            {
                return NotFound();
            }

            _context.AssetScraps.Remove(assetScrap);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Asset Scrap has been deleted successfully.";
            return RedirectToAction(nameof(Index));
        }


        //-----------//

        private bool AssetScrapExists(int id)
        {
            return _context.AssetScraps.Any(e => e.Id == id);
        }

        public IActionResult ApprovalList()
        {
            var assetscrapToApprove = _context.AssetRequests.Where(c => !c.Status.HasValue);
            return View(assetscrapToApprove);
        }
    }
}