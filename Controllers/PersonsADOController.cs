using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        //   GET: Persons/Create
        public IActionResult CreateADO()
        {
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateADO([Bind("ID,Name,Age,Email")] Persons persons)
        {
            if (ModelState.IsValid)
            {
                PersonsRepo _repo = new PersonsRepo(_settings);
                await _repo.CreatePersonADO(persons);
            }
            return View(persons);
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
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditADO(int id, [Bind("Id,Name,Age,Email")] Persons persons)
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            await _repo.UpdatePersonsADO(id, persons);
            return View(persons);
        }
        // GET: Persons/Edit/5
        public async Task<IActionResult> SearchForm()
        { 
            return View("SearchForm");
        }
        // GET: Persons/Edit/5
        public async Task<IActionResult> ShowSearchResults(string SearchString)
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            List<Persons> listPersons = await _repo.SearchPersons(SearchString);
            return View(listPersons);
        }

        [HttpPost]
        public async Task<JsonResult> NameAutocomplete(string prefix)
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            var List = await _repo.PersonsAutocomplete(prefix);
            return Json(List);
        }

        /// <summary>
        /// Funcion Utilizada para el llenado de un select list,
        /// Nota importante, en el lado del cliente en la funcion AJAX
        /// los elementos declarados en el modelo se leen como camelCase,
        /// de lo contrario no detecta los elementos de la respuesta 
        /// Json
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetRoles()
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            List<PersonsRoles> proles = await _repo.GetPersonsRoles();
            return Json(proles);
        }


        // GET: Persons/Edit/5
        public async Task<IActionResult> ManualTask()
        {
            return View("ManualTask");
        }
        // Action to save table data using ADO.NET
        [HttpPost]
        public async Task<JsonResult> SaveTableData([FromBody]List<TableRowModel> rows)
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            if (rows != null && rows.Count > 0)
            {
                await _repo.InsertPersonsManualTsk(rows);
                return Json(new { success = true, message = "Data saved successfully." });
            }

            return Json(new { success = false, message = "No data to save." });
        }
    }
}
