using PersonsMVC.Models;
using PersonsMVC.Tools;

namespace PersonsMVC.Interfaces
{
    public interface IPersons
    {
        Task<List<Persons>> GetPerson();

        //Task<List<Persons>> ObtenerEmpleado();

    }
}
