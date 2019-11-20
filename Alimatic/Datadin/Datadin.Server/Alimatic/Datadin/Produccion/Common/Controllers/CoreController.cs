// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Microsoft.EntityFrameworkCore;

using ExcelDataReader;

namespace Alimatic.Datadin.Produccion.Controllers
{
    using Data;
    using Models;

    using Cyxor.Models;
    using Cyxor.Extensions;
    using Cyxor.Controllers;
    using Cyxor.Networking.Filters;

    abstract class CoreController<TDatadinDbContext> : BaseController<TDatadinDbContext>
        where TDatadinDbContext : DatadinDbContext
    {
        static ConcurrentDictionary<RecordApiModel, IEnumerable<RecordColumnsApiModel>> Cache
            = new ConcurrentDictionary<RecordApiModel, IEnumerable<RecordColumnsApiModel>>();

        static ConcurrentDictionary<RecordApiModel, IEnumerable<RecordColumnsApiModel>> CacheRT
            = new ConcurrentDictionary<RecordApiModel, IEnumerable<RecordColumnsApiModel>>();

        static AutoMapper.IMapper Apper;

        static CoreController()
        {
            Apper = new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(p => p.ForAllMaps((map, expr) =>
            {
                foreach (var propName in map.GetUnmappedPropertyNames())
                {
                    var srcPropInfo = map.SourceType.GetAnyDeclaredProperty(propName);

                    if (srcPropInfo != null)
                        expr.ForSourceMember(propName, opt => opt.Ignore());

                    var destPropInfo = map.DestinationType.GetAnyDeclaredProperty(propName);

                    if (destPropInfo != null)
                        expr.ForMember(propName, opt => opt.Ignore());
                }
            })));
        }

        //[ActionFilter(typeof(DemoActionFilter))]
        public async Task<DataDinData> Data() => new DataDinData
        {
            Rows = await Rows(0),
            Groups = await Groups(0),
            Models = await Models(),
            Divisions = await Divisions(),
            Frequencies = await Frequencies(),
            Enterprises = await Enterprises(),
        };

        public async Task<IEnumerable<ModelApiModel>> Models()
        {
            var userApiModel = Connection?.Tag as UserApiModel;

            if ((userApiModel?.SecurityLevel ?? -1) == -1)
                return (await DatadinDbContext.Models.ToListAsync()).Select(p => Apper.Map<ModelApiModel>(p));
                //return (await DatadinDbContext.Models.ToListAsync()).Select(p => Node.Mapper.Map(p).ToANew<ModelApiModel>());

            var models = from model in DatadinDbContext.Models
                         join userModel in DatadinDbContext.UserModels
                         on model.Id equals userModel.ModelId
                         //on new { UserId = userApiModel.Id, ModelId = model.Id }
                         //equals new { UserId = userApiModel.Id, userModel.ModelId }
                         where userModel.UserId == userApiModel.Id
                         select model;

            return (await models.ToListAsync()).Select(p => Apper.Map<ModelApiModel>(p));
            //return (await models.ToListAsync()).Select(p => Node.Mapper.Map(p).ToANew<ModelApiModel>());

            //return (await DataDinDbContext.Models.ToListAsync()).Select(p => Node.Mapper.Map<ModelApiModel>(p));
        }

        public async Task<IEnumerable<FrequencyApiModel>> Frequencies()
            => (await DatadinDbContext.Frequencies.ToListAsync()).Select(p => Apper.Map<FrequencyApiModel>(p));
            //=> (await DatadinDbContext.Frequencies.ToListAsync()).Select(p => Node.Mapper.Map(p).ToANew<FrequencyApiModel>());

        public async Task<IEnumerable<DivisionApiModel>> Divisions()
            => (await DatadinDbContext.Divisions.ToListAsync()).Select(p =>
            Apper.Map<Division, DivisionApiModel>(p));
            //Node.Mapper.Map(p).ToANew<DivisionApiModel>());//, opts => opts.ConfigureMap(MemberList.Destination)));

        public async Task<IEnumerable<RowApiModel>> Rows(int modelo)
            => (await DatadinDbContext.Rows.Where(p => modelo != 0 ? modelo == p.ModelId : true)
                .ToListAsync()).Select(p => Apper.Map<RowApiModel>(p));
                //.ToListAsync()).Select(p => Node.Mapper.Map(p).ToANew<RowApiModel>());

        public async Task<IEnumerable<GroupApiModel>> Groups(int division)
            => (await DatadinDbContext.Groups.Where(p => division != 0 ? division == p.DivisionId : true)
                .ToListAsync()).Select(p => Apper.Map<GroupApiModel>(p));
                //.ToListAsync()).Select(p => Node.Mapper.Map(p).ToANew<GroupApiModel>());

        public async Task<IEnumerable<EnterpriseApiModel>> Enterprises()
            => (await DatadinDbContext.Enterprises.ToListAsync()).Select(p => Apper.Map<EnterpriseApiModel>(p));
            //=> (await DatadinDbContext.Enterprises.ToListAsync()).Select(p => Node.Mapper.Map(p).ToANew<EnterpriseApiModel>());

        public void CacheReset()
        {
            Cache.Clear();
            CacheRT.Clear();
        }

        public async Task<AuthResponse> Login(AuthRequest model)
        {
            var authResponse = new AuthResponse();

            var user = await DatadinDbContext.Users.SingleOrDefaultAsync(p => p.Email.ToLowerInvariant() == model.I.ToLowerInvariant());

            if (user == null || user.Password != model.PasswordHash)
                throw new InvalidOperationException("Invalid login credentials");

            Connection.IsAuthenticated = true;
            Connection.Tag = new UserApiModel { Id = user.Id, Email = user.Email, Name = user.Name, EnterpriseId = user.EnterpriseId, Permission = user.Permission, SecurityLevel = user.SecurityLevel };

            return new AuthResponse { Tag = user.SecurityLevel , Token = model.Base64Credentials };

            //var tag = string.Compare(model.I, "admin", ignoreCase: true) == 0 &&
            //   string.Compare(model.A, "123456", ignoreCase: true) == 0 ? -1 : default(int?);

            //return new AuthResponse { Tag = tag, Token = model.Base64Credentials };
        }

        #region Models

        public async Task<KeyApiModel<int>> ModelCreate(ModelApiModel apiModel)
        {
            if (await DatadinDbContext.Models.FindAsync(apiModel.Id) != null)
                throw new InvalidOperationException("The model already exists");

            var year = DateTime.Now.Year;

            var model = Apper.Map<ModelApiModel, Model>(apiModel);
            //var model = Node.Mapper.Map(apiModel).ToANew<Model>();
            model = DatadinDbContext.Add(model).Entity;

            var rows = new List<Row>(model.RowCount);

            for (var j = 1; j < model.RowCount + 1; j++)
                rows.Add(new Row { Id = j, Model = model });

            DatadinDbContext.AddRange(rows);

            var enterprises = await DatadinDbContext.Enterprises.ToListAsync();

            var monthCount = CultureInfo.InvariantCulture.Calendar.GetMonthsInYear(year) + (apiModel.IsEFModel ? 1 : 0);

            if (model.FrequencyId == 1)
            {
                for (var i = 1; i <= monthCount; i++)
                {
                    var daysCount = CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(year, i);

                    for (var j = 1; j <= daysCount; j++)
                        GenerateRecords(rows, enterprises, GenerateTemplate(model, day: j, month: i, year: year));
                }
            }
            else if (model.FrequencyId == 2)
            {
                for (var i = 1; i <= monthCount; i++)
                    GenerateRecords(rows, enterprises, GenerateTemplate(model, day: 1, month: i, year: year));
            }
            else
                throw new InvalidOperationException("Invalid model data");

            await DatadinDbContext.SaveChangesAsync();

            return new KeyApiModel<int> { Id = model.Id };
        }

        Template GenerateTemplate(Model model, int day, int month, int year)
            => DatadinDbContext.Add(new Template
            {
                Day = day,
                Month = month,
                Year = year,
                Model = model,
            }).Entity;

        void GenerateRecords(List<Row> rows, List<Enterprise> enterprises, Template template)
        {
            var records = new List<Record>(enterprises.Count * rows.Count);

            foreach (var enterprise in enterprises)
                foreach (var row in rows)
                    records.Add(new Record
                    {
                        Row = row,
                        Template = template,
                        Model = template.Model,
                        Enterprise = enterprise,
                    });

            DatadinDbContext.AddRange(records);
        }

        public async Task ModelDelete(int id)
        {
            var model = DatadinDbContext.Remove(new Model { Id = id }).Entity;
            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ModelRowsAdd(RowApiModel apiModel)
        {
            var row = Apper.Map<RowApiModel, Row>(apiModel);
            //var row = Node.Mapper.Map(apiModel).ToANew<Row>();

            var model = await DatadinDbContext.Models.Include(p => p.Templates).SingleAsync(p => p.Id == apiModel.ModelId);
            model.RowCount++;

            row.Id = model.RowCount;
            DatadinDbContext.Rows.Add(row);

            var enterprises = await DatadinDbContext.Enterprises.ToListAsync();

            foreach (var template in model.Templates)
                GenerateRecords(new List<Row> { row }, enterprises, template);

            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ModelRowsDelete(int modelId)
        {
            var model = await DatadinDbContext.FindAsync<Model>(modelId);
            var row = DatadinDbContext.Remove(new Row { Id = model.RowCount, ModelId = modelId }).Entity;

            model.RowCount--;
            var entity = DatadinDbContext.Attach(model);
            entity.Property(p => p.RowCount).IsModified = true;

            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ModelUpdateColumns(ModelApiModel apiModel)
        {
            var model = Apper.Map<ModelApiModel, Model>(apiModel);
            //var model = Node.Mapper.Map(apiModel).ToANew<Model>();

            var entity = DatadinDbContext.Attach(model);
            entity.Property(p => p.ColumnNames).IsModified = true;

            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ModelUpdateRow(RowApiModel apiModel)
        {
            var model = Apper.Map<RowApiModel, Row>(apiModel);
            //var model = Node.Mapper.Map(apiModel).ToANew<Row>();

            var entity = DatadinDbContext.Attach(model);
            entity.Property(p => p.Description).IsModified = true;

            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion

        public async Task<TemplateRecordsApiModel> TemplateRecords(TemplateApiModel apiModel)
        {
            var enterpriseId = apiModel.EnterpriseId;

            var records = await (
                from record in DatadinDbContext.Records
                where apiModel.Year == record.Year &&
                      apiModel.Month == record.Month &&
                      apiModel.Day == record.Day &&
                      apiModel.ModelId == record.ModelId &&
                      apiModel.EnterpriseId == record.EnterpriseId
                orderby record.Row
                select Apper.Map<Record, RecordColumnsApiModel>(record)).ToListAsync();
                //select Node.Mapper.Map(record).ToANew<RecordColumnsApiModel>()).ToListAsync();

            var template = await DatadinDbContext.Templates.FindAsync(apiModel.Year, apiModel.Month, apiModel.Day, apiModel.ModelId);

            foreach (var record in records)
                record.SetEditMode(!template.Locked);

            return new TemplateRecordsApiModel { Records = records, Template = Apper.Map<Template, TemplateApiModel>(template) };
            //return new TemplateRecordsApiModel { Records = records, Template = Node.Mapper.Map(template).ToANew<TemplateApiModel>() };
        }

        public async Task TemplateLock(TemplateApiModel apiModel)
        {
            var template = Apper.Map<TemplateApiModel, Template>(apiModel);
            //var template = Node.Mapper.Map(apiModel).ToANew<Template>();

            var entity = DatadinDbContext.Attach(template);
            entity.Property(p => p.Locked).IsModified = true;

            foreach (var key in Cache.Keys)
                if (key.Model == apiModel.ModelId && key.Year == apiModel.Year &&
                    (key.Month == apiModel.Month || key.Month == null) &&
                    (key.Day == apiModel.Day || key.Day == null))
                    Cache.TryRemove(key, out var value);

            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        static string ProcessExcel(Stream stream)
        {
            var maxLength = 0;
            var rows = new List<List<string>>();

            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                while (reader.Name != "1")
                    reader.NextResult();

                while (reader.Read())
                {
                    var fields = new List<string>();
                    rows.Add(fields);

                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.GetValue(i).ToString();
                        if (maxLength < value.Length)
                            maxLength = value.Length;
                        fields.Add(value);
                    }
                }
            }

            var sb = new StringBuilder();

            foreach (var row in rows)
            {
                foreach (var field in row)
                    sb.Append(field.PadLeft(maxLength + 1));

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public async Task TemplateImport(TemplateApiModel apiModel)
        {
            var line = default(string);
            var records = new List<RecordApiModel>();

            var fileExtension = Path.GetExtension(apiModel.FileName);

            var bytes = Convert.FromBase64String(apiModel.FileData);

            if (fileExtension == ".xlsx" || fileExtension == ".xls")
            {
                var excelStream = new MemoryStream(bytes);
                apiModel.FileData = ProcessExcel(excelStream);
            }
            else
                apiModel.FileData = Encoding.UTF8.GetString(bytes);

            var stream = new StringReader(apiModel.FileData);

            var model = await DatadinDbContext.Models.FindAsync(apiModel.ModelId);

            var rows = 0;

            while ((line = stream.ReadLine()) != null)
            {
                rows++;

                var tokens = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Length != 6 + model.ColumnCount)
                    throw new InvalidOperationException("Invalid import data");
                else if (tokens[0] != apiModel.ModelId.ToString() || tokens[1] != apiModel.EnterpriseId.ToString())
                    throw new InvalidOperationException("Invalid import data");

                var record = new Record
                {
                    Year = apiModel.Year,
                    Month = apiModel.Month,
                    Day = apiModel.Day,
                    ModelId = apiModel.ModelId,
                    EnterpriseId = apiModel.EnterpriseId,
                    RowId = int.Parse(tokens[5]),
                };

                var entity = DatadinDbContext.Attach(record);

                for (var i = 1; i <= model.ColumnCount; i++)
                {
                    record.GetType().GetProperty($"C0{i}").SetValue(record, decimal.Parse(tokens[i + 5]));
                    entity.Property($"C0{i}").IsModified = true;
                }
            }

            if (rows != model.RowCount)
                throw new InvalidOperationException("Invalid import data");

            foreach (var key in CacheRT.Keys)
                if (key.Model == apiModel.ModelId && key.Year == apiModel.Year &&
                    (key.Month == apiModel.Month || key.Month == null) &&
                    (key.Day == apiModel.Day || key.Day == null) &&
                    (key.Enterprise == apiModel.EnterpriseId || key.Enterprise == null))
                    CacheRT.TryRemove(key, out var value);

            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        #region Record Value

        public void RecordValueGet(RecordApiModel apiModel)
        {

        }

        public async Task RecordValueSet(RecordApiModel apiModel)
        {
            var record = new Record
            {
                Year = apiModel.Year,
                Month = apiModel.Month ?? 1,
                Day = apiModel.Day ?? 1,
                EnterpriseId = apiModel.Enterprise ?? 0,
                ModelId = apiModel.Model,
                RowId = apiModel.Row ?? 0,
            };

            var columnName = $"C0{apiModel.Column}";

            record.GetType().GetProperty(columnName).SetValue(record, apiModel.Value);

            var entity = DatadinDbContext.Attach(record);
            entity.Property(columnName).IsModified = true;

            foreach (var key in CacheRT.Keys)
                if (key.Model == apiModel.Model && key.Year == apiModel.Year &&
                    (key.Month == apiModel.Month || key.Month == null) &&
                    (key.Day == apiModel.Day || key.Day == null) &&
                    (key.Enterprise == apiModel.Enterprise || key.Enterprise == null))
                    CacheRT.TryRemove(key, out var value);

            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task RecordValueClear(RecordApiModel apiModel)
        {
            var record = new Record
            {
                Year = apiModel.Year,
                Month = apiModel.Month ?? 1,
                Day = apiModel.Day ?? 1,
                EnterpriseId = apiModel.Enterprise ?? 0,
                ModelId = apiModel.Model,
                RowId = apiModel.Row ?? 0,
            };

            var columnName = $"C0{apiModel.Column}";

            record.GetType().GetProperty(columnName).SetValue(record, default(decimal?));

            var entity = DatadinDbContext.Attach(record);
            entity.Property(columnName).IsModified = true;

            await DatadinDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion

        public class ApaisadoApiModel
        {
            public string[] Columns { get; set; }
            public RecordColumnsAApiModel[] Records { get; set; }
        }

        public class RecordColumnsAApiModel
        {
            public int RowId { get; set; }

            public decimal?[] Values { get; set; }

            public static RecordColumnsAApiModel Create(int rowId, int columnCount)
                => new RecordColumnsAApiModel { RowId = rowId, Values = new decimal?[columnCount] };
        }

        public async Task<ApaisadoApiModel> ComputedApaisados(RecordApiModel apiModel)
        {
            var model = await DatadinDbContext.Models.FindAsync(apiModel.Model).ConfigureAwait(false);

            var columnName = $"C0{apiModel.ColumnApaisado ?? model.ColumnCount}";
            var propertyInfo = typeof(RecordColumnsApiModel).GetProperty(columnName);

            var apaisado = new ApaisadoApiModel();

            var recordsTotal = (List<RecordColumnsApiModel>)(await ComputedRecords(apiModel));

            apaisado.Records = new RecordColumnsAApiModel[recordsTotal.Count];

            void CreateRecords(int columnCount)
            {
                for (var i = 0; i < recordsTotal.Count; i++)
                {
                    var recordColumnsAApiModel = RecordColumnsAApiModel.Create(recordsTotal[i].RowId, columnCount);
                    var columnValue = (decimal?)propertyInfo.GetValue(recordsTotal[i]);
                    recordColumnsAApiModel.Values[0] = columnValue;
                    apaisado.Records[i] = recordColumnsAApiModel;
                }
            }

            async Task SetRecordValues(RecordApiModel newModel, int columnId)
            {
                var records = (List<RecordColumnsApiModel>)(await ComputedRecords(newModel));

                for (var j = 0; j < recordsTotal.Count(); j++)
                {
                    var columnValue = (decimal?)propertyInfo.GetValue(records[j]);

                    apaisado.Records[j].RowId = j + 1;
                    apaisado.Records[j].Values[columnId] = columnValue;
                }
            }

            if (apiModel.Division == null && apiModel.Group == null)
            {
                var divisions = await DatadinDbContext.Divisions.ToListAsync();

                apaisado.Columns = new string[divisions.Count + 1];
                apaisado.Columns[0] = "Total";

                CreateRecords(divisions.Count + 1);

                for (var i = 0; i < divisions.Count; i++)
                {
                    apaisado.Columns[i + 1] = divisions[i].Name;
                    var newModel = apiModel.CreateCopy();
                    newModel.Division = divisions[i].Id;
                    await SetRecordValues(newModel, i + 1);
                }
            }
            else if (apiModel.Division != null && apiModel.Group == null)
            {
                var groups = await DatadinDbContext.Groups.Include(p => p.Division).Where(p => p.DivisionId == apiModel.Division).ToListAsync();

                apaisado.Columns = new string[groups.Count + 1];
                apaisado.Columns[0] = groups[0].Division.Name;

                CreateRecords(groups.Count + 1);

                for (var i = 0; i < groups.Count; i++)
                {
                    apaisado.Columns[i + 1] = groups[i].Name;
                    var newModel = apiModel.CreateCopy();
                    newModel.Group = groups[i].Id;
                    await SetRecordValues(newModel, i + 1);
                }
            }
            else if (apiModel.Division != null && apiModel.Group != null)
            {
                var enterprises = await DatadinDbContext.Enterprises.Include(p => p.Group).Where(p => p.GroupId == apiModel.Group && p.DivisionId == apiModel.Division).ToListAsync();

                apaisado.Columns = new string[enterprises.Count + 1];
                apaisado.Columns[0] = enterprises[0].Group.Name;

                CreateRecords(enterprises.Count + 1);

                for (var i = 0; i < enterprises.Count; i++)
                {
                    apaisado.Columns[i + 1] = enterprises[i].Name;
                    var newModel = apiModel.CreateCopy();
                    newModel.Enterprise = enterprises[i].Id;
                    await SetRecordValues(newModel, i + 1);
                }
            }

            return apaisado;
        }

        public async Task<IEnumerable<RecordColumnsApiModel>> ComputedRecords(RecordApiModel model)
        {
            var cache = model.PartialData ? CacheRT : Cache;

            var isDateRange = model.Day2 != null && model.Month2 != null;

            if (!cache.ContainsKey(model))
                cache[model] = await (from record in DatadinDbContext.Records.Include(p => p.Enterprise)
                       join template in DatadinDbContext.Templates
                       on new { record.ModelId, record.Year, record.Month, record.Day }
                       equals new { template.ModelId, template.Year, template.Month, template.Day }
                       where record.Year == model.Year && record.ModelId == model.Model
                           && (model.PartialData | template.Locked)
                           && (isDateRange ? model.Month != null ? record.Month >= model.Month
                           && record.Month <= model.Month2 : record.Month <= model.Month2 : true)
                           && (isDateRange ? model.Day != null ? record.Day >= model.Day
                           && record.Day <= model.Day2 : record.Day <= model.Day2 : true)
                           && (!isDateRange ? model.Month != null ? record.Month == model.Month : true : true)
                           && (!isDateRange ? model.Day != null ? record.Day == model.Day : true : true)
                           && (model.Division != null ? record.Enterprise.DivisionId == model.Division : true)
                           && (model.Group != null ? record.Enterprise.GroupId == model.Group : true)
                           && (model.Row != null ? record.RowId == model.Row : true)
                           && (model.Enterprise != null ? record.EnterpriseId == model.Enterprise : true)
                       group record by record.Row into g
                       orderby g.Key.Id
                       select new RecordColumnsApiModel
                       {
                           RowId = g.Key.Id,
                           C01 = g.Sum(p => p.C01),
                           C02 = g.Sum(p => p.C02),
                           C03 = g.Sum(p => p.C03),
                           C04 = g.Sum(p => p.C04),
                           C05 = g.Sum(p => p.C05),
                           C06 = g.Sum(p => p.C06),
                           C07 = g.Sum(p => p.C07),
                           C08 = g.Sum(p => p.C08),
                           C09 = g.Sum(p => p.C09),
                           //Description = g.Key.Description,
                       }).ToListAsync().ConfigureAwait(false);

            return cache[model];
        }

        [Action(HttpContentType = "text/plain", HttpContentFormat = HttpContentFormat.String)]
        public async Task<decimal?> ComputedValue(RecordApiModel model)
        {
            var ef = (await ComputedRecords(model).ConfigureAwait(false)).SingleOrDefault();

            if (ef == null)
                return null;

            switch (model.Column)
            {
                case 1: return ef.C01;
                case 2: return ef.C02;
                case 3: return ef.C03;
                case 4: return ef.C04;
                case 5: return ef.C05;
                case 6: return ef.C06;
                case 7: return ef.C07;
                case 8: return ef.C08;
                case 9: return ef.C09;

                default: return null;
            }
        }

        [Action(HttpContentType = "text/plain", HttpContentFormat = HttpContentFormat.String)]
        public async Task<string> ComputedNumber(RecordApiModel model)
            => (await ComputedValue(model))?.ToString("N");
    }
}
// { Alimatic.Datadin } - Backend
