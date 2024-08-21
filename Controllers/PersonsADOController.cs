using Microsoft.AspNetCore.Mvc;
using PersonsClass.Repository;

using PersonsClass.Models;
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
        public async Task<IActionResult> ShowPersons()
        {
            List<PersonsADO> listaEmpleado = await _repository.GetPersons();

            return View(listaEmpleado);
        }

    }
}
