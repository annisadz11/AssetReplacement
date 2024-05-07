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
    public class AssetRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssetRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssetRequest
        public IActionResult Index()
        {
            return View();
        }

        // untuk dapet data di table
        //API ENDPOINT
        [HttpGet]
        public IActionResult GetData()
        {
            var AssetRequests = _context.AssetRequests
                .Select(g => new
                {
                    id = g.Id,
                    name = g.Name,
                    departement = g.Departement,
                    type = g.Type,
                    serialNumber = g.SerialNumber,
                    baseline = g.Baseline,
                    usageLocation = g.UsageLocation,
                    requestDate = g.RequestDate.HasValue ? g.RequestDate.Value.ToString("dd MMM yyyy") : null,
                    reason = g.Reason,
                    status = g.Status,
                    approvalDate = g.ApprovalDate.HasValue ? g.ApprovalDate.Value.ToString("dd MMM yyyy") : null,
                    justify = g.Justify,
                    typeReplace = g.TypeReplace,
                }).ToList();

            return Json(new { rows = AssetRequests });
        }

        // GET: AssetRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetRequest = await _context.AssetRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assetRequest == null)
            {
                return NotFound();
            }

            return View(assetRequest);
        }

        // GET: AssetRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AssetRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Departement,Type,SerialNumber,Baseline,UsageLocation,RequestDate,Reason")] AssetRequest assetRequest)
        {
            if (ModelState.IsValid)
            {
                // Set default values for properties that should not be set in the Create action
                assetRequest.Status = null;
                assetRequest.ApprovalDate = null;
                assetRequest.Justify = null;
                assetRequest.TypeReplace = null;

                _context.Add(assetRequest);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Asset Request has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(assetRequest);
        }

        // GET: AssetRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetRequest = await _context.AssetRequests.FindAsync(id);
            if (assetRequest == null)
            {
                return NotFound();
            }
            return View(assetRequest);
        }
        // POST: AssetRequest/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Departement,Type,SerialNumber,Baseline,UsageLocation,RequestDate,Reason,Status")] AssetRequest assetRequest)
        {
            if (id != assetRequest.Id)
            {
                return NotFound();
            }

            var existingRecord = await _context.AssetRequests.FindAsync(id);
            if (existingRecord == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingRecord.Name = assetRequest.Name;
                    existingRecord.Departement = assetRequest.Departement;
                    existingRecord.Type = assetRequest.Type;
                    existingRecord.SerialNumber = assetRequest.SerialNumber;
                    existingRecord.Baseline = assetRequest.Baseline;
                    existingRecord.UsageLocation = assetRequest.UsageLocation;
                    existingRecord.RequestDate = assetRequest.RequestDate;
                    existingRecord.Reason = assetRequest.Reason;
                    existingRecord.Status = assetRequest.Status; // Mempertahankan nilai Status yang sudah ada
                    existingRecord.ApprovalDate = assetRequest.ApprovalDate;
                    existingRecord.Justify = assetRequest.Justify;
                    existingRecord.TypeReplace = assetRequest.TypeReplace;

                    _context.Update(existingRecord);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Asset Request has been updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetRequestExists(existingRecord.Id))
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
            return View(assetRequest);
        }

        // GET: AssetRequests/Delete/5
        public async Task<IActionResult> Delete(int? id, string handler)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assetRequest = await _context.AssetRequests
                .FirstOrDefaultAsync(m => m.Id == id);

            if (assetRequest == null)
            {
                return NotFound();
            }

            if (handler == "Delete")
            {
                _context.AssetRequests.Remove(assetRequest);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Asset request has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(assetRequest);
        }

        // POST: AssetRequest/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id, string handler)
        {
            var assetRequest = await _context.AssetRequests.FindAsync(id);

            if (assetRequest == null)
            {
                return NotFound();
            }

            if (handler == "Delete")
            {
                _context.AssetRequests.Remove(assetRequest);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Asset request has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }

            return View(assetRequest);
        }
        //-----------//
        private bool AssetRequestExists(int id)
        {
            return _context.AssetRequests.Any(e => e.Id == id);
        }

        public IActionResult ApprovalList()
        {
            var assetrequestToApprove = _context.AssetRequests.Where(c => !c.Status.HasValue);
            return View(assetrequestToApprove);
        }

       
    }
}