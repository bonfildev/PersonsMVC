﻿using Microsoft.Data.SqlClient;
using PersonsMVC.Interfaces;
using PersonsMVC.Models;
using System.Text;

namespace PersonsMVC.Tools
{
	public class PersonsTasksRepo : IPersonsTasks
	{
        private readonly SqlServer _sqlTools;
		public List<PersonsTasks> Options => GetPersonsTasks();

		public PersonsTasksRepo(IDBSettings _settings)
        {
            _sqlTools = new SqlServer(_settings);
        }
        public List<PersonsTasks> GetPersonsTasks()
        {
            List<PersonsTasks> lista = new List<PersonsTasks>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendLine("Select * From PersonsTasks");
            SqlDataReader? dr = _sqlTools.OpenDataReader("", "", strSQL); // Use nullable type for SqlDataReader  

            if (dr != null)
            {
                while (dr.Read())
                {
                    lista.Add(new PersonsTasks()
                    {
                        Idtask = Convert.ToInt32(dr["IdTask"].ToString()),
                        Description = dr["Description"].ToString(),
                        RegisterDate = Convert.ToDateTime(dr["RegisterDate"].ToString()),
                        Finished = Convert.ToBoolean(dr["Finished"].ToString()),
                        IDPerson = Convert.ToInt32(dr["IDPerson"].ToString())
                    });
                }
                dr.Close(); // Ensure the SqlDataReader is closed after use  
            }

            return lista;
        }
		 
	}
}
