using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PersonsMVC.Interfaces;
using System.Data;
using System.Text;

namespace PersonsMVC.Tools
{
    public class SqlServer
    {

        public readonly IDBSettings _settings;
        private const int TimeOut = 1000;
        public SqlServer(IDBSettings settings)
        {
            _settings = settings;
        }
        public async Task<IActionResult> ExecuteQueryAsync(string PageName, string FunctionName, StringBuilder strSQL)  // Query a ejecutar  
        {
            SqlConnection conn = OpenConnection();
            long Rows = 0;

            if (conn != null)
            {
                try
                {
                    SqlTransaction trans;
                    SqlCommand comm = new SqlCommand(strSQL.ToString(), conn);
                    trans = conn.BeginTransaction();
                    comm.CommandTimeout = TimeOut;
                    comm.Transaction = trans;
                    comm.CommandType = CommandType.Text;
                    Rows = await comm.ExecuteNonQueryAsync();
                    trans.Commit();
                    trans.Dispose();
                    comm.Dispose();
                }
                catch (Exception ex)
                {
                    Rows = 0;
                    WriteError(PageName, "ExecuteQueryAsync " + PageName + " " + FunctionName, ex.Message + " " + strSQL.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            return new ObjectResult(null) { StatusCode = StatusCodes.Status200OK };
        }


        public long ExecuteQuerySync(string PageName, string FunctionName, StringBuilder strSQL)  // Query a ejecutar
        {
            SqlConnection conn = OpenConnection();
            long Rows = 0;
            if (conn != null)
            {
                try
                {
                    SqlTransaction trans;
                    SqlCommand comm = new SqlCommand(strSQL.ToString(), conn);
                    trans = conn.BeginTransaction();
                    comm.CommandTimeout = TimeOut;
                    comm.Transaction = trans;
                    comm.CommandType = CommandType.Text;
                    Rows = comm.ExecuteNonQuery();
                    trans.Commit();
                    trans.Dispose();
                    comm.Dispose();
                }
                catch (Exception ex)
                {
                    Rows = 0;
                    WriteError(PageName, "ExecuteQuerySync " + PageName + " " + FunctionName, ex.Message + " " + strSQL.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            return Math.Abs(Rows);
        } // End ExecCommand 

        public SqlConnection OpenConnection()
        {
            SqlConnection cnSQL = new SqlConnection { ConnectionString = _settings.DBConnection };
            try
            {
                cnSQL.Open();
            }
            catch (Exception ex)
            {
                WriteError("", "OpenConnection", ex.Message);
            }
            return cnSQL;
        }

        public void WriteError(string FileName, string FunctionName, string msgError)
        {
            try
            {
                string strDir = _settings.AppDir == null ? "" : _settings.AppDir;

                StreamWriter Output = new StreamWriter(strDir + DateTime.Now.ToString() + ".csv" , true);
                Output.WriteLine("\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + "\",\"" + FileName + "\",\"" + FunctionName + "\",\"" + msgError + "\"");
                Output.Close();
            }
            catch
            {
                ;
            }
        }
        public SqlDataReader? OpenDataReader(string pagina, string funcion, StringBuilder strSQL)
        {
            SqlConnection cnSQL;
            SqlCommand cmSQL;
            cnSQL = OpenConnection();
            SqlDataReader? drSQL = null;
            if (cnSQL != null)
            {
                try
                {
                    cmSQL = new SqlCommand { CommandText = strSQL.ToString(), Connection = cnSQL, CommandTimeout = TimeOut };
                    drSQL = cmSQL.ExecuteReader();
                }
                catch (Exception ex)
                {
                    WriteError(pagina, "OpenDataReader " + pagina + " " + funcion, ex.Message + " " + strSQL.ToString());
                }
            }
            return drSQL;
        } // End Function GetClave

        public string QI(string value, bool number, bool coma = true)
        {
            string nvalue = value == null ? value = "" : value;
            string sVal;
            sVal = nvalue.Trim().Length == 0 ? "NULL" :
                        (nvalue.Trim().Replace("'", "").Replace(",", "").Replace("',", ""));
            if (!number && sVal != "NULL") 
            { 
                sVal = "'" + sVal  + "'";
            } 
            else {
                sVal = sVal.Replace("$", "");
            }
            if (coma)
            {
                sVal = sVal + ",";
            }
            return sVal;
        }


    }
}
