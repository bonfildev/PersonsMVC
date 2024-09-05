using Microsoft.AspNetCore.Mvc;
using PersonsMVC.Models;
using PersonsMVC.Interfaces;

namespace PersonsMVC.Controllers
{
    public class PersonsADOController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IPersons _repository;
        public PersonsADOController (IPersons repo)
        {
            _repository = repo;
        }


        // GET: Persons
        public async Task<IActionResult> Persons()
        {
            List<Persons> listaEmpleado = await _repository.ObtenerEmpleado();

            return View(listaEmpleado);
        }

    }
}
