using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using PersonsMVC.Interfaces;
using PersonsMVC.Models;
using System.Data;

namespace PersonsMVC.Tools
{
    public class PersonsRepo
    {
        private readonly IDBSettings _conexion;

        public PersonsRepo(IDBSettings conexion)
        {
            _conexion = conexion;
        }


        public async Task<List<Persons>> GetPerson()
        {
            List<Persons> lista = new List<Persons>();

            using (var conexion = new SqlConnection(_conexion.DBConnection))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("Select * From Persons", conexion);
                cmd.CommandType = CommandType.Text;


                using (var dr = await cmd.ExecuteReaderAsync())
                {

                    while (await dr.ReadAsync())
                    {

                        lista.Add(new Persons()
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            Age = Convert.ToInt32(dr["Age"].ToString()),
                            Email = dr["Email"].ToString()

                        });


                    }
                }
            }
            return lista;
        }
    }
}
