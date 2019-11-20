using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.DataDin.Controllers
{
    using Models;
    using Cyxor.Models;

    using Cyxor.Controllers;

    //public class EF
    //{
    //    public int Id { get; set; }
    //    [EmailAddress]
    //    public string Name { get; set; }
    //    public bool Dime { get; set; }
    //    public IEnumerable<EmpresaApiModel> Empresas { get; set; }
    //}

    public class EF
    {
        public int Year { get; set; } = DateTime.Now.Year;
        public int? Month { get; set; }
        public int? Reeup { get; set; }
        public int Model { get; set; } = 5920;

        public IEnumerable<EstadoFinancieroApiModel> EFModels { get; set; }
    }

    public class DataDinData
    {
        public IEnumerable<FilaApiModel> Filas { get; set; }
        public IEnumerable<GrupoApiModel> Grupos { get; set; }
        public IEnumerable<ModeloApiModel> Modelos { get; set; }
        public IEnumerable<EmpresaApiModel> Empresas { get; set; }
        public IEnumerable<DivisionApiModel> Divisiones { get; set; }
    }

    [Controller(Route = nameof(DataDin))]
    class DataDinController : BaseController
    {
        //public View<string> Prueba(EF ef)
        //{
        //    return new View<string> { Model = "Ok" };
        //}

        //public async Task<View<EF>> EF(EF model)
        //{
        //    var query = from ef in DataDinDbContext.EstadosFinancieros
        //                where ef.Año == model.Year && ef.ModeloId == model.Model
        //                    && (model.Month != null ? ef.Mes == model.Month : true)
        //                    && (model.Reeup != null ? ef.EmpresaId == model.Reeup : true)
        //                group ef by ef.Fila into g
        //                orderby g.Key.Id
        //                select new EstadoFinancieroApiModel
        //                {
        //                    Id = g.Key.Id,
        //                    C1 = g.Sum(p => p.C1),
        //                    C2 = g.Sum(p => p.C2),
        //                    C3 = g.Sum(p => p.C3),
        //                    Descripcion = g.Key.Descripcion,
        //                };

        //    model.EFModels = await query.ToListAsync();

        //    return new View<EF> { Model = model };
        //}

        //[Action(typeof(EmpresaListApiModel))]
        //public async Task<IEnumerable<EmpresaApiModel>> Empresas(EmpresaListApiModel model)
        //{
        //    var entries = new List<EmpresaApiModel>();

        //    var empresas = await Query(DataDinDbContext.Empresas.OrderBy(p => p.Id), model).ToListAsync();

        //    foreach (var empresa in empresas)
        //        entries.Add(new EmpresaApiModel
        //        {
        //            Id = empresa.Id,
        //            GrupoId = empresa.GrupoId,
        //            DivisionId = empresa.DivisionId,
        //            Nombre = empresa.Nombre,
        //            NombreCompleto = empresa.NombreCompleto,
        //        });

        //    return entries;
        //}

        //[Action(typeof(EstadosFinancierosGetApiModel))]
        //public async Task<IEnumerable<EstadoFinancieroApiModel>> EF(EstadosFinancierosGetApiModel model)
        //{
        //    var query = from ef in DataDinDbContext.EstadosFinancieros
        //                where ef.Año == model.Year && ef.ModeloId == model.Model
        //                    && (model.Month != null ? ef.Mes == model.Month : true)
        //                    && (model.Empresa != null ? ef.EmpresaId == model.Empresa : true)
        //                group ef by ef.Fila into g
        //                orderby g.Key.Id
        //                select new EstadoFinancieroApiModel
        //                {
        //                    Id = g.Key.Id,
        //                    C1 = g.Sum(p => p.C1),
        //                    C2 = g.Sum(p => p.C2),
        //                    C3 = g.Sum(p => p.C3),
        //                    Descripcion = g.Key.Descripcion,
        //                };

        //    return await query.ToListAsync();
        //}

        class Excel
        {
            public string File { get; set; }
            public int Sheet { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
            public string Value { get; set; }
        }

#if !NETCOREAPP2_1
        //public async Task<View<EF>> Pdf(int value)
        [Action(ContentType = "application/pdf")]
        public byte[] Pdf(int value)
        {
            GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var file = GemBox.Spreadsheet.ExcelFile.Load(@"D:\Documents\ExcelExport\Chart.xlsx");

            file.Worksheets.First().Cells[0, 0].Value = value;

            file.Worksheets.First().PrintOptions.FitToPage = true;

            //var df = new GemBox.Spreadsheet.ImageSaveOptions(GemBox.Spreadsheet.ImageSaveFormat.Jpeg);
            //df.PageNumber = 0;

            //file.Save(@"D:\Documents\ExcelExport\Chart.Jpeg", df);
            //file.Save(@"D:\Documents\ExcelExport\Chart.pdf", new GemBox.Spreadsheet.PdfSaveOptions());

            var memoryStream = new System.IO.MemoryStream();
            var saveOptions = new GemBox.Spreadsheet.PdfSaveOptions();
            saveOptions.SelectionType = GemBox.Spreadsheet.SelectionType.ActiveSheet;
            file.Save(memoryStream, saveOptions);

            return memoryStream.ToArray();
        }
#endif

        //======================================================

        static ConcurrentDictionary<EstadosFinancierosGetApiModel, IEnumerable<EstadoFinancieroApiModel>> Cache
            = new ConcurrentDictionary<EstadosFinancierosGetApiModel, IEnumerable<EstadoFinancieroApiModel>>();

        public async Task<DataDinData> Data() => new DataDinData
        {
            Filas = await Filas(0),
            Grupos = await Grupos(0),
            Modelos = await Modelos(),
            Empresas = await Empresas(),
            Divisiones = await Divisiones(),
        };

        public async Task<IEnumerable<ModeloApiModel>> Modelos()
            => (await DataDinDbContext.Modelos.ToListAsync()).Select(p => Node.Mapper.Map<ModeloApiModel>(p));

        public async Task<IEnumerable<DivisionApiModel>> Divisiones()
            => (await DataDinDbContext.Divisiones.ToListAsync()).Select(p => Node.Mapper.Map<DivisionApiModel>(p));

        public async Task<IEnumerable<FilaApiModel>> Filas(int modelo)
            => (await DataDinDbContext.Filas.Where(p => modelo != 0 ? modelo == p.ModeloId : true)
                .ToListAsync()).Select(p => Node.Mapper.Map<FilaApiModel>(p));

        public async Task<IEnumerable<GrupoApiModel>> Grupos(int division)
            => (await DataDinDbContext.Grupos.Where(p => division != 0 ? division == p.DivisionId : true)
                .ToListAsync()).Select(p => Node.Mapper.Map<GrupoApiModel>(p));

        public async Task<IEnumerable<EmpresaApiModel>> Empresas()
            => (await DataDinDbContext.Empresas.ToListAsync()).Select(p => Node.Mapper.Map<EmpresaApiModel>(p));

        //[Action("datadin cache reset")]
        public void CacheReset() => Cache.Clear();

        public async Task<IEnumerable<EstadoFinancieroApiModel>> EF(EstadosFinancierosGetApiModel model)
        {
            if (!Cache.ContainsKey(model))
                Cache[model] = await (from ef in DataDinDbContext.EstadosFinancieros.Include(p => p.Empresa)
                                      where ef.Año == model.Year && ef.ModeloId == model.Model
                                          && (model.Month != null ? ef.Mes == model.Month : true)
                                          && (model.Division != null ? ef.Empresa.DivisionId == model.Division : true)
                                          && (model.Grupo != null ? ef.Empresa.GrupoId == model.Grupo : true)
                                          && (model.Row != null ? ef.FilaId == model.Row : true)
                                          && (model.Empresa != null ? ef.EmpresaId == model.Empresa : true)
                                      group ef by ef.Fila into g
                                      orderby g.Key.Id
                                      select new EstadoFinancieroApiModel
                                      {
                                          Id = g.Key.Id,
                                          C1 = g.Sum(p => p.C1),
                                          C2 = g.Sum(p => p.C2),
                                          C3 = g.Sum(p => p.C3),
                                          Descripcion = g.Key.Descripcion,
                                      }).ToListAsync();

            return Cache[model];
        }

        public async Task<decimal?> Value(EstadosFinancierosGetApiModel model)
        {
            var ef = (await EF(model).ConfigureAwait(false)).SingleOrDefault();
             
            if (ef == null)
                return null;

            switch (model.Column)
            {
                case 1: return ef.C1;
                case 2: return ef.C2;
                case 3: return ef.C3;
                //case 4: return ef.C4;
                //case 5: return ef.C5;
                //case 6: return ef.C6;
                //case 7: return ef.C7;
                //case 8: return ef.C8;
                //case 9: return ef.C9;

                default: return null;
            }
        }
    }
}
