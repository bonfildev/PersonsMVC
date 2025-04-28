using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly SqlServer _sqlTools;

        public PersonsRepo(IDBSettings _settings)
        {
            _sqlTools = new SqlServer(_settings);
        }

        public async Task<List<Persons>> GetPerson()
        {
            List<Persons> lista = new List<Persons>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select * From Persons"); 
            SqlDataReader? dr = _sqlTools.OpenDataReader("", "", strSQL);
            if (dr != null)
            {
                while (await dr.ReadAsync())
                {
                    lista.Add(new Persons()
                    {
                        Id = Convert.ToInt32(dr["Id"].ToString()),
                        Name = dr["Name"].ToString(),
                        Age = Convert.ToInt32(dr["Age"].ToString()),
                        Email = dr["Email"].ToString() ?? ""

                    });
                }
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
            strSQL.AppendLine("    WHERE Name LIKE '%" + SearchString + "%'"); // Added condition to use SearchString  

            SqlDataReader? dr = _sqlTools.OpenDataReader("", "", strSQL); // Changed to nullable SqlDataReader  

            if (dr != null) // Ensure dr is not null before proceeding  
            {
                while (await dr.ReadAsync())
                {
                    lista.Add(new Persons()
                    {
                        Id = Convert.ToInt32(dr["Id"].ToString()),
                        Name = dr["Name"].ToString(),
                        Age = Convert.ToInt32(dr["Age"].ToString()),
                        Email = dr["Email"].ToString() ?? ""
                    });
                }
                await dr.CloseAsync();
                await dr.DisposeAsync();
            }
            return lista;
        }


        public async Task<Persons> CreatePersonADO(Persons persons)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("INSERT INTO Persons (Name,Age,Email)");
            strSQL.AppendLine(" Values(" + _sqlTools.QI(persons.Name ?? "", false, true));
            strSQL.AppendLine("        " + _sqlTools.QI(persons.Age.ToString(),true, true));
            strSQL.AppendLine("        " + _sqlTools.QI(persons.Email, false, false) + ")");
            await _sqlTools.ExecuteQueryAsync("", "CreatePersonsADO", strSQL);
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
            SqlDataReader? dr = _sqlTools.OpenDataReader("", "", strSQL); // Fix: Changed to nullable SqlDataReader  

            if (dr != null) // Fix: Ensure dr is not null before proceeding  
            {
                while (await dr.ReadAsync())
                {
                    person.Id = Convert.ToInt32(dr["Id"].ToString());
                    person.Name = dr["Name"].ToString();
                    person.Age = Convert.ToInt32(dr["Age"].ToString());
                    person.Email = dr["Email"].ToString() ?? "";
                }
                await dr.CloseAsync();
                await dr.DisposeAsync();
            }
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
            await _sqlTools.ExecuteQueryAsync("", "UpdatePersonsADO", strSQL);
            return persons;
        }

        public async Task<List<Autocomplete>> PersonsAutocomplete(string str)
        {
            List<Autocomplete> lista = new List<Autocomplete>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select ID,Name From Persons");
            strSQL.AppendLine(" WHERE Name like '%" + str + "%'");
            SqlDataReader? dr = _sqlTools.OpenDataReader("", "", strSQL);
            if (dr != null) // Fix: Ensure dr is not null before proceeding  
            {
                while (await dr.ReadAsync())
                {
                    lista.Add(new Autocomplete()
                    {
                        label = dr["Name"]?.ToString() ?? string.Empty, // Fix: Handle possible null value  
                        val = Convert.ToInt32(dr["Id"].ToString())
                    });
                }
                await dr.CloseAsync();
                await dr.DisposeAsync();
            }
            return lista;
        }
        public async Task<List<PersonsRoles>> GetPersonsRoles()
        {
            List<PersonsRoles> lista = new List<PersonsRoles>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select IDRole,RoleName From Roles");
            SqlDataReader? dr = _sqlTools.OpenDataReader("", "", strSQL);

            if (dr != null) // Ensure dr is not null before proceeding  
            {
                if (dr != null)
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new PersonsRoles()
                        {
                            RoleName = dr["RoleName"].ToString() ?? "",
                            IDRole = Convert.ToInt32(dr["IDRole"].ToString())
                        });
                    }
                    await dr.CloseAsync();
                    await dr.DisposeAsync();
                }
            }
            return lista;
        }



        public async Task<int> InsertPersonsManualTsk(List<PersonsTasks> rows)
        {
            StringBuilder strSQL = new StringBuilder();

            foreach (var row in rows)
            {
                strSQL.Clear();
                strSQL.AppendLine("INSERT INTO PersonsTasks ([Description],[RegisterDate],[Finished],[IDPerson])");
                strSQL.AppendLine(" Values(" + _sqlTools.QI(row.Description?.ToString() ?? string.Empty, false)); // Fix: Use null-coalescing operator to handle possible null value  
                strSQL.AppendLine("        " + _sqlTools.QI(row.RegisterDate?.ToString() ?? string.Empty, false)); 
                strSQL.AppendLine("        " + _sqlTools.QI(row.Finished.ToString(), true)); 
                strSQL.AppendLine("        " + _sqlTools.QI(row.IDPerson.ToString(), false, true)); 
                await _sqlTools.ExecuteQueryAsync("", "InsertPersonsManualTsk", strSQL);
            }
            return 0;
        }
    }
}
