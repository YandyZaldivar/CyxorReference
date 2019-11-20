using System;
using System.IO;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text;

namespace Alimatic.Datadin.Produccion.Controllers
{
    using Cyxor.Controllers;

    //public class Url
    //{
    //    public string Name { get; set; }
    //    public string Address { get; set; }
    //}

    //class Report
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public List<string> Urls { get; set; }
    //}

    public class Menu
    {
        public int ReportId { get; set; }
        public int UrlId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class ReporteController : Controller
    {
        public async Task<IEnumerable<Menu>> Data()
        {
            var consultas = new List<Menu>();

            var connectionString = File.ReadAllText("reporte_cs.txt");

            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("select reports.id as reportId, url.id as urlId, nombre, url, descripcion from reports inner join url on reports.id = url.idreports", connection))
                {
                    var dataReader = await command.ExecuteReaderAsync();

                    while (await dataReader.ReadAsync())
                    {
                        var consulta = new Menu
                        {
                            ReportId = (int)dataReader["reportId"],
                            UrlId = (int)dataReader["urlId"],
                            Name = dataReader["nombre"] as string,
                            Url = dataReader["url"] as string,
                            Description = dataReader["descripcion"] as string,
                        };

                        consultas.Add(consulta);
                    }
                }
            }

            return consultas;
        }
    }
}
