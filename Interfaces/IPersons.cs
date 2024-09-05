using PersonsMVC.Models;
using PersonsMVC.Tools;

namespace PersonsMVC.Interfaces
{
    public interface IPersons
    {
        Task<List<Persons>> ObtenerEmpleado();
        //Task<List<PersonsADO>> ObtenerEmpleado();



    }
}
