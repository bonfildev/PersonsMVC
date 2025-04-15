using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonsMVC.Data;
using PersonsMVC.Interfaces;
using PersonsMVC.Models;
using PersonsMVC.Tools;

namespace PersonsMVC.Controllers
{
    public class AlpineController : Controller
    {
        private readonly SqlTools _sqlTools;
        private readonly IDBSettings _settings;
        private readonly ApplicationDbContext _context;
        public AlpineController(IDBSettings settings, ApplicationDbContext context)
        {
            _sqlTools = new SqlTools(settings);
            _settings = settings;
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }


        //   GET: Persons/Create
        public async Task<IActionResult> Alpine()
        {
            var rows = await _context.RowItems.ToListAsync();
            return View(rows);
            //return View("Alpine");
        }
        // POST: /Row/Save
        [HttpPost]
        public async Task<IActionResult> Save(List<PersonsTasks> rows)
        {
            // Optional: Clear old data
            _context.RowItems.RemoveRange(_context.RowItems);
            await _context.SaveChangesAsync();

            // Save new data
            _context.RowItems.AddRange(rows);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }

}