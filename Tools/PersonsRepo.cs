using Microsoft.Data.SqlClient;
using PersonsMVC.Interfaces;
using PersonsMVC.Models;
using System.Data;
using System.Text;

namespace PersonsMVC.Tools
{
    public class PersonsRepo
    {
        private readonly IDBSettings _conexion;

        public PersonsRepo(IDBSettings conexion)
        {
            _conexion = conexion;
        }


        public async Task<List<Persons>> GetPerson(int? id)
        {
            List<Persons> lista = new List<Persons>();

            using (var conexion = new SqlConnection(_conexion.DBConnection))
            {
                conexion.Open(); 
                StringBuilder strSQL = new StringBuilder();
                strSQL.AppendLine("Select * From Persons");
                if (id != 0)
                {
                    strSQL.AppendLine(" WHERE Id = " + id );
                }
                SqlCommand cmd = new SqlCommand(strSQL.ToString(), conexion);
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
                conexion.Close();
            }
            return lista;
        }

        public async Task<Persons> EditPerson(int? id)
        {
            Persons person = new Persons();
            using (var conexion = new SqlConnection(_conexion.DBConnection))
            {
                conexion.Open();
                StringBuilder strSQL = new StringBuilder();
                strSQL.AppendLine("Select * From Persons");
                if (id != 0)
                {
                    strSQL.AppendLine(" WHERE Id = " + id);
                }
                SqlCommand cmd = new SqlCommand(strSQL.ToString(), conexion);
                cmd.CommandType = CommandType.Text;


                using (var dr = await cmd.ExecuteReaderAsync())
                { 
                    while (await dr.ReadAsync())
                    {
                        person.Id = Convert.ToInt32(dr["Id"]);
                        person.Name = dr["Name"].ToString();
                        person.Age = Convert.ToInt32(dr["Age"].ToString());
                        person.Email = dr["Email"].ToString();
                    }
                }
                conexion.Close();
            }
            return person;
        }
        public async Task<Persons> UpdatePersonsADO(int id, Persons persons)
        {
            using (var conexion = new SqlConnection(_conexion.DBConnection))
            {
                conexion.Open();
                StringBuilder strSQL = new StringBuilder();
                strSQL.AppendLine("UPDATE Persons");
                strSQL.AppendLine(" SET Name = '" + persons.Name + "',");
                strSQL.AppendLine("     Age = '" + persons.Age + "',");
                strSQL.AppendLine("     Email = '" + persons.Name + "'" );
                strSQL.AppendLine(" WHERE Id = " + id);
                SqlCommand cmd = new SqlCommand("Update Persons SET", conexion);
                cmd.ExecuteNonQuery();
                conexion.Close();
                conexion.Dispose();
            }
            return persons;
        }
    }
}
