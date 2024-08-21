using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonsClass.Repository;
using PersonsMVC.Data;
using PersonsMVC.Models;
using PersonsClass.Models;

namespace PersonsMVC.Controllers
{
    public class PersonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Persons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persons.ToListAsync());
        }

        // GET: Persons
        public async Task<IActionResult> SearchForm()
        {
            return View();
        }
        // GET: Persons
        public async Task<IActionResult> ShowSearchResult(string SearchName)
        {
            return View("Index", await _context.Persons.Where(j=>j.Name.Contains(SearchName)).ToListAsync());
        }

        // GET: Persons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persons = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persons == null)
            {
                return NotFound();
            }

            return View(persons);
        }

        [Authorize]
        //   GET: Persons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Email")] Persons persons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(persons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(persons);
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persons = await _context.Persons.FindAsync(id);
            if (persons == null)
            {
                return NotFound();
            }
            return View(persons);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Email")] Persons persons)
        {
            if (id != persons.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonsExists(persons.Id))
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
            return View(persons);
        }

        // GET: Persons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persons = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persons == null)
            {
                return NotFound();
            }

            return View(persons);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persons = await _context.Persons.FindAsync(id);
            if (persons != null)
            {
                _context.Persons.Remove(persons);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonsExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }

    }
}
