using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using NuGet.Configuration;
using PersonsMVC.Interfaces;
using System.Data;
using System.Text;
using System.Threading;

namespace PersonsMVC.Tools
{
    public class SqlTools
    {

        public readonly IDBSettings _settings;
        private const int TimeOut = 1000;
        private string MSGError = string.Empty;
        public SqlTools(IDBSettings settings)
        {
            _settings = settings;
        }
        //-----------------------------------------------------------------------------
        /// <summary>
        ///Procedimientos que ejecuta codigo SQL de Update, Delete e Insert 
        /// </summary>
        /// <param name="pagina">Nombre del página que invoca el comenado</param>
        /// <param name="funcion"></param>
        /// <param name="strSQL">Comando a ejecutar</param>
        /// <returns></returns>
        public long ExecCommand(string PageName, string FunctionName, StringBuilder strSQL)  // Comando a ejecutar en la base de datos
        {
            SqlConnection cnComando = OpenConnection(PageName);
            long Rows = 0;
            MSGError = string.Empty;

            if (cnComando != null)
            {
                try
                {
                    SqlTransaction trComando;                  // Variable para la transación
                    SqlCommand cmComando = new SqlCommand(strSQL.ToString(), cnComando);
                    trComando = cnComando.BeginTransaction();  // Inicia con el Bloqueo

                    cmComando.CommandTimeout = TimeOut;   // Cambioel Time Out por defualt
                    cmComando.Transaction = trComando;
                    cmComando.CommandType = CommandType.Text;

                    Rows = cmComando.ExecuteNonQuery();
                    trComando.Commit();
                    trComando.Dispose();
                    cmComando.Dispose();
                }
                catch (Exception ex)
                {
                    Rows = 0;
                    MSGError = "SQL_Tools.execCommand:" + ex.Message + " " + strSQL.ToString();
                    WriteError(PageName, "execCommand " + PageName + " " + FunctionName, ex.Message + " " + strSQL.ToString());
                }
                finally
                {
                    cnComando.Close();
                }
            }
            return Math.Abs(Rows);
        } // End ExecCommand 


        public SqlConnection OpenConnection(string PageName)
        {
            SqlConnection cnSQL = new SqlConnection { ConnectionString = _settings.DBConnection };
            MSGError = string.Empty;
            try
            {
                cnSQL.Open();
            }
            catch (Exception ex)
            {
                WriteError(PageName, "OpenConnection", ex.Message);
            }
            return cnSQL;
        }

        public void WriteError(string FileName, string FunctionName, string msgError)
        {
            try
            {
                string strDir = _settings.AppDir == null ? "" : _settings.AppDir;

                StreamWriter Output = new StreamWriter(strDir + GetLogFileName(), true);
                Output.WriteLine("\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    + "\",\"" + FileName + "\",\"" + FunctionName + "\",\"" + msgError + "\"");
                Output.Close();
            }
            catch
            {
                ;
            }
        }
        private string GetLogFileName()
        {
            DateTime _date = DateTime.Now;
            if (_date.DayOfWeek == DayOfWeek.Tuesday)
                _date = _date.AddDays(-1);
            else if (_date.DayOfWeek == DayOfWeek.Wednesday)
                _date = _date.AddDays(-2);
            else if (_date.DayOfWeek == DayOfWeek.Thursday)
                _date = _date.AddDays(-3);
            else if (_date.DayOfWeek == DayOfWeek.Friday)
                _date = _date.AddDays(-4);
            else if (_date.DayOfWeek == DayOfWeek.Saturday)
                _date = _date.AddDays(-5);
            else if (_date.DayOfWeek == DayOfWeek.Sunday)
                _date = _date.AddDays(-6);
            return "log_" + _date.ToString("yyyy_MM_dd") + ".csv";
        }


        /// <summary>
        /// Realiza la lectura de en modo conectado de un Data Reader
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="funcion"></param>
        /// <param name="cnSQL"></param>
        /// <param name="cmSQL"></param>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public SqlDataReader OpenDataReader(string pagina, string funcion, StringBuilder strSQL)
        {
            SqlConnection cnSQL;
            SqlCommand cmSQL;
            cnSQL = OpenConnection(pagina);
            SqlDataReader drSQL = null;
            MSGError = string.Empty;

            if (cnSQL != null)
            {
                try
                {
                    cmSQL = new SqlCommand { CommandText = strSQL.ToString(), Connection = cnSQL, CommandTimeout = TimeOut };
                    drSQL = cmSQL.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MSGError = "SQL_Tools.OpenDataReader:" + ex.Message + " " + strSQL.ToString();
                    WriteError(pagina, "OpenDataReader " + pagina + " " + funcion, ex.Message + " " + strSQL.ToString());
                }
            }
            return drSQL;
        } // End Function GetClave
    }
}
