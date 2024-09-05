using Microsoft.AspNetCore.Mvc;
using PersonsMVC.Models;
using PersonsMVC.Interfaces;
using PersonsMVC.Tools;
using NuGet.Configuration;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PersonsMVC.Controllers
{
    public class PersonsADOController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly IDBSettings _settings;

        public PersonsADOController (IDBSettings conexion)
        {
            _settings = conexion;
        }

        // GET: Persons
        public async Task<IActionResult> PersonsADO()
        {
            PersonsRepo _repo = new PersonsRepo(_settings);
            List<Persons> listPersons = await _repo.GetPerson(0);
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
        //public async Task<IActionResult> EditADO(int id, [Bind("Id,Name,Age,Email")] Persons persons)
        //{
        //    PersonsRepo _repo = new PersonsRepo(_settings);
        //    await _repo.UpdatePersonsADO(id, persons);
        //    return View(persons);
        //}

    }
}
