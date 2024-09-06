using Microsoft.Data.SqlClient;
using PersonsMVC.Interfaces;
using PersonsMVC.Models;
using System.Data;
using System.Data.Common;
using System.Text;

namespace PersonsMVC.Tools
{
    public class PersonsRepo
    {
        private readonly SqlTools _sqlTools;

        public PersonsRepo(IDBSettings _settings)
        {
            _sqlTools = new SqlTools(_settings);
        }


        public async Task<List<Persons>> GetPerson()
        {
            List<Persons> lista = new List<Persons>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select * From Persons"); 
            SqlDataReader dr = _sqlTools.OpenDataReader("", "", strSQL);
            
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
            return lista;
        }

        public async Task<Persons> EditPerson(int? id)
        {
            Persons person = new Persons();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select * From Persons");
            if (id != 0)
            {
                strSQL.AppendLine(" WHERE Id = " + id);
            }
            SqlDataReader dr = _sqlTools.OpenDataReader("", "", strSQL);
            while (await dr.ReadAsync())
            {
                person.Id = Convert.ToInt32(dr["Id"]);
                person.Name = dr["Name"].ToString();
                person.Age = Convert.ToInt32(dr["Age"].ToString());
                person.Email = dr["Email"].ToString();
            } 
            return person;
        }
        public async Task<Persons> UpdatePersonsADO(int id, Persons persons)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("UPDATE Persons");
            strSQL.AppendLine(" SET Name = '" + persons.Name + "',");
            strSQL.AppendLine("     Age = '" + persons.Age + "',");
            strSQL.AppendLine("     Email = '" + persons.Email + "'");
            strSQL.AppendLine(" WHERE Id = " + id);
            _sqlTools.ExecCommand("", "UpdatePersonsADO", strSQL);
            return persons;
        }
    }
}
