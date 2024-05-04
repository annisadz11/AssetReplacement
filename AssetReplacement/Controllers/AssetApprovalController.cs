﻿using System;
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
    public class AssetApprovalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssetApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssetRequest
        public IActionResult Index()
        {
            return View();
        }

        ///API ENDPOINT
        [HttpGet]
        public IActionResult GetData()
        {
            var AssetRequests = _context.AssetRequests
                .Where(g => g.Status == null) // Filter untuk status null
                .Select(g => new
                {
                    id = g.Id,
                    name = g.Name,
                    departement = g.Departement,
                    type = g.Type,
                    serialNumber = g.SerialNumber,
                    baseline = g.Baseline,
                    usageLocation = g.UsageLocation,
                    reason = g.Reason,
                    justify = g.Justify,
                    requestDate = g.RequestDate.HasValue ? g.RequestDate.Value.ToString("dd MMM yyyy") : null,
                    status = g.Status,
                    approvalDate = g.ApprovalDate.HasValue ? g.ApprovalDate.Value.ToString("dd MMM yyyy") : null,
                }).ToList();

            return Json(new { rows = AssetRequests });
        }

        //APPROVE MANAGER
        [HttpPost]
        public async Task<IActionResult> Approve(int id, string justify)
        {
            var assetRequest = await _context.AssetRequests.FindAsync(id);
            if (assetRequest == null)
            {
                return NotFound();
            }

            assetRequest.Status = true;
            assetRequest.Justify = justify;
            assetRequest.ApprovalDate = DateTime.Now;
            _context.Update(assetRequest);
            await _context.SaveChangesAsync();


            return Json(new { success = true, message = "Asset request approved successfully!" });
        }



        //REJECT MANAGER
        [HttpPost]
        public async Task<IActionResult> Reject(int id, string justify)
        {
            var assetRequest = await _context.AssetRequests.FindAsync(id);
            if (assetRequest == null)
            {
                return NotFound();
            }

            assetRequest.Status = false; // Mengubah status menjadi false (0) untuk rejected
            assetRequest.Justify = justify;
            assetRequest.ApprovalDate = DateTime.Now;
            _context.Update(assetRequest);
            await _context.SaveChangesAsync();


            return Json(new { success = true, message = "Asset request rejected successfully!" });
        }

        private bool AssetRequestExists(int id)
        {
            return _context.AssetRequests.Any(e => e.Id == id);
        }
    }
}
