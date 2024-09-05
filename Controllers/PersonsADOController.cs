using Microsoft.AspNetCore.Mvc;
using PersonsMVC.Models;
using PersonsMVC.Interfaces;
using PersonsMVC.Tools;
using NuGet.Configuration;

namespace PersonsMVC.Controllers
{
    public class PersonsADOController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IDBSettings _settings;

        private readonly IPersons _repository;
        public PersonsADOController (IDBSettings conexion)
        {
            //_repository = repo;
            _settings = conexion;
        }

        // GET: Persons
        public async Task<IActionResult> PersonsADO()
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            List<Persons> listaEmpleado = await _repo.GetPerson();

            return View(listaEmpleado);
        }

    }
}
