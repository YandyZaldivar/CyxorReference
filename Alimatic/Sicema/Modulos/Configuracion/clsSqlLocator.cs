using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Configuracion
{
	public class clsSqlLocator
	{
		[DllImport("odbc32.dll")]
		private static extern short SQLAllocHandle(short hType, IntPtr inputHandle, out IntPtr outputHandle);
		[DllImport("odbc32.dll")]
		private static extern short SQLSetEnvAttr(IntPtr henv, int attribute, IntPtr valuePtr, int strLength);
		[DllImport("odbc32.dll")]
		private static extern short SQLFreeHandle(short hType, IntPtr handle); 
		[DllImport("odbc32.dll",CharSet=CharSet.Ansi)]
		private static extern short SQLBrowseConnect(IntPtr hconn, StringBuilder inString, 
			short inStringLength, StringBuilder outString, short outStringLength,
			out short outLengthNeeded);

		private const short SQL_HANDLE_ENV = 1;
		private const short SQL_HANDLE_DBC = 2;
		private const int SQL_ATTR_ODBC_VERSION = 200;
		private const int SQL_OV_ODBC3 = 3;
		private const short SQL_SUCCESS = 0;
		
		private const short SQL_NEED_DATA = 99;
		private const short DEFAULT_RESULT_SIZE = 1024;
		private const string SQL_DRIVER_STR = "DRIVER=SQL SERVER";
	
		private clsSqlLocator(){}

		public static string[] GetServers()
		{
			string[] retval = null;
			string txt = string.Empty;
			IntPtr henv = IntPtr.Zero;
			IntPtr hconn = IntPtr.Zero;
			StringBuilder inString = new StringBuilder(SQL_DRIVER_STR);
			StringBuilder outString = new StringBuilder(DEFAULT_RESULT_SIZE);
			short inStringLength = (short) inString.Length;
			short lenNeeded = 0;

			try
			{
				if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_ENV, henv, out henv))
				{
					if (SQL_SUCCESS == SQLSetEnvAttr(henv,SQL_ATTR_ODBC_VERSION,(IntPtr)SQL_OV_ODBC3,0))
					{
						if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_DBC, henv, out hconn))
						{
							if (SQL_NEED_DATA ==  SQLBrowseConnect(hconn, inString, inStringLength, outString, 
								DEFAULT_RESULT_SIZE, out lenNeeded))
							{
								if (DEFAULT_RESULT_SIZE < lenNeeded)
								{
									outString.Capacity = lenNeeded;
									if (SQL_NEED_DATA != SQLBrowseConnect(hconn, inString, inStringLength, outString, 
										lenNeeded,out lenNeeded))
									{
										throw new ApplicationException("No se han podido obtener los Servidores SQL.");
									}	
								}
								txt = outString.ToString();
								int start = txt.IndexOf("{") + 1;
								int len = txt.IndexOf("}") - start;
								if ((start > 0) && (len > 0))
								{
									txt = txt.Substring(start,len);
								}
								else
								{
									txt = string.Empty;
								}
							}						
						}
					}
				}
			}
			catch (Exception)
			{
				//Throw away any error if we are not in debug mode
#if (DEBUG)
                throw new ApplicationException("No se han podido obtener los Servidores SQL.");
#endif 
			}
			finally
			{
				if (hconn != IntPtr.Zero)
				{
					SQLFreeHandle(SQL_HANDLE_DBC,hconn);
				}
				if (henv != IntPtr.Zero)
				{
					SQLFreeHandle(SQL_HANDLE_ENV,hconn);
				}
			}
	
			if (txt.Length > 0)
			{
				retval = txt.Split(',');
			}

			return retval;
		}

        public static string[] GetSqlServers()
        {
            string[] retval = null;
            string listServers = String.Empty;
            string server;

            try
            {
                // Retrieve the enumerator instance and then the data.
                SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
                DataTable table = instance.GetDataSources();

                // Display the contents of the table.
                foreach (DataRow row in table.Rows)
                {
                    server = string.Empty;
                    foreach (DataColumn col in table.Columns)
                    {
                        if (col.ColumnName.ToLower() == "servername") { server = row[col].ToString(); }
                        else
                        {
                            if ((col.ColumnName.ToLower() == "instancename") && (row[col].ToString() != "")) 
                            { server += @"\" + row[col].ToString(); }
                        }
                    }
                    if (listServers.Length == 0) { listServers += server; }
                    else { listServers += "," + server; }
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("No se han podido obtener los Servidores SQL.");
            }

            if (listServers.Length > 0)
            {
                retval = listServers.Split(',');
            }
            return retval;
        }

        public static List<string> GetDatabases(string strServer, bool bolSeguridad = true, string strUsuario = "", string strContrasenna = "")
        {
            string select;
            string cadenaConexion;
            List<string> listaBasesDatos = new List<string>();
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();

            if (strServer.Length > 0)
            {
                csb.DataSource = strServer;
                csb.InitialCatalog = "master";
                if (bolSeguridad)
                {
                    csb.IntegratedSecurity = true;
                    cadenaConexion = csb.ConnectionString;
                }
                else
                {
                    csb.UserID = strUsuario;
                    csb.Password = strContrasenna;
                    cadenaConexion = csb.ConnectionString;
                }

                using (SqlConnection con = new SqlConnection(cadenaConexion))
                {
                    try
                    {
                        // Abrimos la conexión
                        con.Open();

                        // Obtenemos los nombres de las bases de datos que haya en el servidor
                        // se pueden filtrar para no mostrar las bases de datos de sistema
                        select = "select name from sysdatabases order by name;";

                        // Obtenemos un dataReader con el resultado
                        SqlCommand com = new SqlCommand(select, con);
                        SqlDataReader dr = com.ExecuteReader();

                        // Recorremos el dataReader y añadimos un elemento nuevo 
                        // por cada registro
                        while (dr.Read())
                        {
                            listaBasesDatos.Add(dr[0].ToString());
                        }
                    }
                    catch (Exception)
                    {
#if (DEBUG)
                        throw new ApplicationException("No se ha podido obtener la lista de bases de datos.");
#endif
                    }
                }
            }

            // Devolvemos la lista de bases de datos
            return listaBasesDatos;
        }

        public static bool ConnectSQL(string strServer, string strCatalogo, bool bolSeguridad = true, string strUsuario = "", string strContrasenna = "")
        {
            bool result = false;
            string cadenaConexion;
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();

            csb.DataSource = strServer;
            csb.InitialCatalog = strCatalogo;
            if (bolSeguridad)
            {
                csb.IntegratedSecurity = true;
                cadenaConexion = csb.ConnectionString;
            }
            else
            {
                csb.UserID = strUsuario;
                csb.Password = strContrasenna;
                cadenaConexion = csb.ConnectionString;
            }

            using (SqlConnection con = new SqlConnection(cadenaConexion))
            {
                try
                {
                    // Abrimos la conexión
                    con.Open();

       				while(con.State == ConnectionState.Connecting)
				    {
					    Application.DoEvents();
				    };

                    result = (con.State == ConnectionState.Open);
                }
                catch (Exception)
                {
#if (DEBUG)
                    throw new ApplicationException("No se ha podido realizar la conexión con la bases de datos.");
#endif
                }
            }
            return result;
        }

    }
}