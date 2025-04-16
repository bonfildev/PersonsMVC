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


        //   GET: 
        public async Task<IActionResult> Alpine()
        {
            var rows = await _context.RowItems.ToListAsync();
            return View("Alpine",rows);
            //return View("Alpine");
        }
        //   GET: 
        public async Task<IActionResult> AlpineIndexedDB()
        {
            var rows = await _context.RowItems.ToListAsync();
            return View("AlpineIndexedDB", rows);
            //return View("Alpine");
        }
        //   GET: 
        public async Task<IActionResult> AlpineDexie()
        {
            var rows = await _context.RowItems.ToListAsync();
            return View("AlpineDexie", rows);
            //return View("Alpine");
        }
        //   GET: 
        public async Task<IActionResult> HTMX()
        {
            var rows = await _context.RowItems.ToListAsync();
            return View("HTMX", rows);
            //return View("Alpine");
        }
        //   GET: 
        public async Task<IActionResult> PersonsTasks()
        {
            var rows = await _context.RowItems.ToListAsync();
            return View("PersonsTasks", rows);
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

            return RedirectToAction("Alpine", rows);
        }

        [HttpPost]
        public IActionResult SaveIndexed([FromBody] List<PersonsTasks> tasks)
        {
            foreach (var task in tasks)
            {
                var existing = _context.RowItems.FirstOrDefault(t => t.Idtask == task.Idtask);
                if (existing != null)
                {
                    // Update
                    existing.Description = task.Description;
                    existing.RegisterDate = task.RegisterDate;
                    existing.Finished = task.Finished;
                    existing.IDPerson = task.IDPerson;
                }
                else
                {
                    // Add
                    _context.RowItems.Add(task);
                }
            }

            _context.SaveChanges();
            return Ok(new { success = true });
        }

    }

}