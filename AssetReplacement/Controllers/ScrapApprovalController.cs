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
    public class ScrapApprovalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScrapApprovalController(ApplicationDbContext context)
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


		//APPROVE MANAGER
		[HttpPost]
		public async Task<IActionResult> Approve(int id)
		{
			var assetScrap = await _context.AssetScraps.FindAsync(id);
			if (assetScrap == null)
			{
				return NotFound();
			}

			assetScrap.Status = true;
			_context.Update(assetScrap);
			await _context.SaveChangesAsync();

			return Json(new { success = true, message = "Asset Scrap success in Infitrack!" });
		}

		private bool AssetScrapExists(int id)
        {
            return _context.AssetScraps.Any(e => e.Id == id);
        }
    }
}
