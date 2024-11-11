using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using PersonsMVC.Interfaces;
using PersonsMVC.Models;
using System;
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
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    Name = dr["Name"].ToString(),
                    Age = Convert.ToInt32(dr["Age"].ToString()),
                    Email = dr["Email"].ToString()

                });
            }
            await dr.CloseAsync();
            await dr.DisposeAsync();
            return lista;
        }


        public async Task<List<Persons>> SearchPersons(string SearchString)
        {
            List<Persons> lista = new List<Persons>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select * From Persons");
            strSQL.AppendLine("    WHERE ");
            SqlDataReader dr = _sqlTools.OpenDataReader("", "", strSQL);

            while (await dr.ReadAsync())
            {
                lista.Add(new Persons()
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    Name = dr["Name"].ToString(),
                    Age = Convert.ToInt32(dr["Age"].ToString()),
                    Email = dr["Email"].ToString()

                });
            }
            await dr.CloseAsync();
            await dr.DisposeAsync();
            return lista;
        }


        public async Task<Persons> CreatePersonADO(Persons persons)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("INSERT INTO Persons (Name,Age,Email)");
            strSQL.AppendLine(" Values(" + _sqlTools.QI(persons.Name, false, true));
            strSQL.AppendLine("        " + _sqlTools.QI(persons.Age.ToString(),true, true));
            strSQL.AppendLine("        " + _sqlTools.QI(persons.Email, false, false) + ")");
            await _sqlTools.ExecCommand("", "CreatePersonsADO", strSQL);
            return persons;
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
                person.Id = Convert.ToInt32(dr["Id"].ToString());
                person.Name = dr["Name"].ToString();
                person.Age = Convert.ToInt32(dr["Age"].ToString());
                person.Email = dr["Email"].ToString();
            }
            await dr.CloseAsync();
            await dr.DisposeAsync();
            return person;
        }
        public async Task<Persons> UpdatePersonsADO(int id, Persons persons)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("UPDATE Persons");
            strSQL.AppendLine(" SET Name = " + _sqlTools.QI(persons.Name, false, true));
            strSQL.AppendLine("     Age = " + _sqlTools.QI(persons.Age.ToString(), true, true));
            strSQL.AppendLine("     Email = " + _sqlTools.QI(persons.Email, false, false));
            strSQL.AppendLine(" WHERE Id = " + id);
            await _sqlTools.ExecCommand("", "UpdatePersonsADO", strSQL);
            return persons;
        }

        public async Task<List<Autocomplete>> PersonsAutocomplete(string str)
        {
            List<Autocomplete> lista = new List<Autocomplete>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select ID,Name From Persons");
            strSQL.AppendLine(" WHERE Name like '%" + str +"%'");
            SqlDataReader dr = _sqlTools.OpenDataReader("", "", strSQL);

            while (await dr.ReadAsync())
            {
                lista.Add(new Autocomplete()
                {
                    label = dr["Name"].ToString(),
                    val = Convert.ToInt32(dr["Id"].ToString())
                });
            }
            await dr.CloseAsync();
            await dr.DisposeAsync();
            return lista;
        }
        public async Task<List<PersonsRoles>> GetPersonsRoles()
        {
            List<PersonsRoles> lista = new List<PersonsRoles>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select IDRole,RoleName From Roles");
            SqlDataReader dr = _sqlTools.OpenDataReader("", "", strSQL);

            while (await dr.ReadAsync())
            {
                lista.Add(new PersonsRoles()
                {
                    RoleName = dr["RoleName"].ToString(),
                    IDRole = Convert.ToInt32(dr["IDRole"].ToString())
                });
            }
            await dr.CloseAsync();
            await dr.DisposeAsync();
            return lista;
        }
    }
}
