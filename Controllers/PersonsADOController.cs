using Microsoft.AspNetCore.Mvc;
using PersonsMVC.Interfaces;
using PersonsMVC.Models;
using PersonsMVC.Tools;

namespace PersonsMVC.Controllers
{
    public class PersonsADOController : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 
        private readonly SqlTools _sqlTools;
        private readonly IDBSettings _settings;

        public PersonsADOController (IDBSettings settings)
        {
            _sqlTools = new SqlTools(settings);
            _settings = settings;
        }

        // GET: Persons
        public async Task<IActionResult> PersonsADO()
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            List<Persons> listPersons = await _repo.GetPerson();
            return View(listPersons);
        }

        // GET: Persons/Edit/5
        public async Task<IActionResult> EditADO(int? id)
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            Persons person = await _repo.EditPerson(id);
            return View(person);
        }

        // SET: Persons
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditADO(int id, [Bind("Id,Name,Age,Email")] Persons persons)
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            await _repo.UpdatePersonsADO(id, persons);
            return View(persons);
        }

    }
}
