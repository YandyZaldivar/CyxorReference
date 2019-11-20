using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Alimatic.Nexus.Controllers
{
    using Data;
    using Models;

    using Cyxor.Networking;
    using Cyxor.Controllers;

    using Newtonsoft.Json;

    using Controllers = Network.NetworkControllers;

    class TableController : BaseController
    {
        static char[] CsvSeparator = new char[] { ',', ';' };
        static string[] LineSeparator = new string[] { Environment.NewLine };

        #region Get
        //[Action(ApiId.GetTable)]
        public async Task<TableApiModel> GetTable(GetTableApiModel getTableApiModel)
        {
            var entry = await NexusDbContext.Tables.AsNoTracking().SingleAsync(p =>
                getTableApiModel.IsId ? p.Id == getTableApiModel.Id : p.Name == getTableApiModel.Name);

            return new TableApiModel { Id = entry.Id, Name = entry.Name };
        }

        //[Command("nexus table get", Arguments = "$table", Description = "Get the Nexus $table.")]
        //public Task<TableApiModel> GetTable(CommandArgs args) => InvokeAsync<TableApiModel>(new GetTableApiModel { NameOrId = args["$table"] });
        #endregion

        #region Add
        //[Action(ApiId.AddTable)]
        public async Task<TableApiModel> AddTable(AddTableApiModel addTableApiModel)
        {
            var entry = NexusDbContext.Tables.Add(new Table { Name = addTableApiModel.Name }).Entity;
            await NexusDbContext.SaveChangesAsync();
            return new TableApiModel { Id = entry.Id, Name = entry.Name };
        }

        //[Command("nexus table add", Arguments = "$name", Description = "Add a new Nexus table with the specified $name.")]
        //public Task<TableApiModel> AddTable(CommandArgs args) => InvokeAsync<TableApiModel>(new AddTableApiModel { Name = args["$name"] });
        #endregion

        #region Remove
        //[Action(ApiId.RemoveTable)]
        public async Task RemoveTable(RemoveTableApiModel removeTableApiModel)
        {
            var entry = removeTableApiModel.IsId ? new Table { Id = removeTableApiModel.Id ?? 0 } :
                await NexusDbContext.Tables.SingleAsync(p => p.Name == removeTableApiModel.Name);

            NexusDbContext.Tables.Remove(entry);
            await NexusDbContext.SaveChangesAsync();
        }

        //[Command("nexus table remove", Arguments = "$table", Description = "Remove the Nexus $table.")]
        //public Task RemoveTable(CommandArgs args) => InvokeActionAsync(new RemoveTableApiModel { NameOrId = args["$table"] });
        #endregion

        #region Update
        //[Action(ApiId.UpdateTable)]
        //public async Task UpdateTable(UpdateTableApiModel updateTableApiModel)
        //{
        //    var entry = updateTableApiModel.IsId ? new Table { Id = updateTableApiModel.Id ?? 0 } :
        //        await NexusDbContext.Tables.SingleAsync(p => p.Name == updateTableApiModel.Name);

        //    entry.Name = updateTableApiModel.NewName;

        //    NexusDbContext.Tables.Update(entry);
        //    await NexusDbContext.SaveChangesAsync();
        //}

        //[Command("nexus table update", Arguments = "$table $new-name",
        //    Description = "Update the Nexus $table with the $new-name.")]
        //public Task UpdateTable(CommandArgs args) => InvokeActionAsync(new UpdateTableApiModel
        //{
        //    NameOrId = args["$table"],
        //    NewName = args["$new-name"]
        //});
        #endregion

        #region GetAll
        //[Action(ApiId.GetAllTables)]
        public async Task<IEnumerable<TableApiModel>> GetAllTables()
        {
            var entries = new List<TableApiModel>();

            foreach (var item in await NexusDbContext.Tables.AsNoTracking().ToListAsync())
                entries.Add(new TableApiModel { Id = item.Id, Name = item.Name });

            return entries;
        }

        //[Command("nexus table list", Description = "Get all tables in the Nexus.")]
        //public Task<TablesApiModel> GetAllTables(CommandArgs args) => InvokeAsync<TablesApiModel>();
        #endregion

        //[Action(ApiId.GetTableData)]
        async Task<TableDataModel> GetTableData(GetTableDataApiModel getTableDataApiModel)
        {
            var user = default(User);

            if (Connection != null)
                user = await NexusDbContext.Users.Include(p => p.Roles).Include(p => p.Security).SingleAsync(p => p.AccountId == Connection.Account.Id);

            if (Connection != null && (user == null || user.Security.Value == SecurityValue.None))
                throw new InvalidOperationException("You don't have permission to enter the Nexus.");

            var table = await NexusDbContext.Tables
                    .Include(p => p.Roles)
                    .Include(p => p.Rows).ThenInclude(p => p.Columns)
                    .Include(p => p.Columns).ThenInclude(p => p.Roles)
                    .Include(p => p.Columns).ThenInclude(p => p.Type)
                .SingleAsync(p => getTableDataApiModel.IsId ? p.Id == getTableDataApiModel.Id : p.Name == getTableDataApiModel.Name);

            var maxTableRoleSecurityValue = default(SecurityValue);

            if (Connection != null)
            {
                var tableRoles = from tableRole in table.Roles
                                 join userRole in user.Roles
                                 on tableRole.RoleId equals userRole.RoleId
                                 orderby tableRole.SecurityId
                                 select tableRole;

                var accessDenied = tableRoles.Count() == 0 ? false : true;

                foreach (var tableRole in tableRoles)
                    if (tableRole.Security.Value > SecurityValue.None && tableRole.Permission.Value > PermissionValue.None)
                    {
                        accessDenied = false;
                        break;
                    }

                if (accessDenied)
                    throw new InvalidOperationException($"You don't have permission to the table '{table.Name}'.");

                if (tableRoles.Count() > 0)
                    maxTableRoleSecurityValue = tableRoles.Where(p => p.Security.Value > SecurityValue.None && p.Permission.Value > PermissionValue.None).Max(p => p.Security.Value);
            }

            var rows = new List<RowApiModel>(table.Rows.Count);
            var columns = new List<ColumnApiModel>(table.Columns.Count);
            var rowColumns = new List<RowColumnApiModel>(table.Rows.Count * table.Columns.Count);

            foreach (var column in table.Columns)
            {
                // TODO: If column is not elegible, continue

                columns.Add(new ColumnApiModel
                {
                    Id = column.Id,
                    Name = column.Name,
                    Order = column.Order,
                    TypeId = column.TypeId,
                    TableId = column.TableId,
                    NotNull = column.NotNull,
                    EnumValues = column.EnumValues,
                });
            }

            foreach (var row in table.Rows)
            {
                if (Connection != null)
                    if (maxTableRoleSecurityValue == SecurityValue.User)
                        if (row.UserId != user.Id)
                            continue;

                foreach (var rowColumn in row.Columns)
                {
                    var column = rowColumn.Column;

                    // TODO: If column is not elegible, continue

                    var objectValue = ParseRowColumnValue(column, column.Type.Value, rowColumn.Value);

                    if (rowColumn.Value != null)
                    {
                        switch (column.Type.Value)
                        {
                            case ColumnTypeValue.Int32Enum:
                            case ColumnTypeValue.StringEnum:

                                var enumValues = JsonConvert.DeserializeObject<string[]>(column.EnumValues);

                                if (!enumValues.Contains(objectValue.ToString()))
                                    throw new InvalidOperationException("Database is corrupted.");

                                break;
                        }
                    }

                    rowColumns.Add(new RowColumnApiModel
                    {
                        RowId = rowColumn.RowId,
                        ColumnId = rowColumn.ColumnId,
                        Value = rowColumn.Value
                    });
                }

                rows.Add(new RowApiModel { Id = row.Id, UserId = row.UserId, TableId = row.TableId });
            }

            return new TableDataModel
            {
                Id = table.Id,
                Name = table.Name,
                Rows = rows,
                Columns = columns,
                RowColumns = rowColumns,
            };
        }

        //[Command("nexus table data", Arguments = "$table [$start-row] [$row-count]" ,
        //    Description = "Get the Nexus $table data returning a number of rows equal to [$row-count] starting at index [$start-row].")]
        //public Task<TableDataModel> GetTableData(CommandArgs args) => InvokeAsync<TableDataModel>(new GetTableDataApiModel
        //{
        //    NameOrId = args["$table"],
        //    StartRow = args["$start-row"] != null ? int.Parse(args["$start-row"]) : null as int?,
        //    RowCount = args["$row-count"] != null ? int.Parse(args["$row-count"]) : null as int?,
        //    //StartRow = args["$start-row"] != null ? new ApiModel<int> { Value = int.Parse(args["$start-row"]) } : null,
        //    //RowCount = args["$row-count"] != null ? new ApiModel<int> { Value = int.Parse(args["$row-count"]) } : null,
        //});

        #region Import
        //[Action(ApiId.ImportTable)]
        public async Task<Result> ImportTable(ImportTableApiModel importTableApiModel)
        {
            if (Connection != null)
            {
                var user = await NexusDbContext.Users.Include(p => p.Security).SingleAsync(p => p.AccountId == Connection.Account.Id);

                if (user.Security.Value != SecurityValue.Administrator)
                    return new Result(ResultCode.Error, "You don't have permissions to import tables");
            }

            var table = new Table { Name = importTableApiModel.Name };

            if (importTableApiModel.Format == ImportTableFormat.File)
            {
                var extension = Path.GetExtension(importTableApiModel.Data).Substring(startIndex: 1);
                importTableApiModel.Format = (ImportTableFormat)Enum.Parse(typeof(ImportTableFormat), extension, ignoreCase: true);
                importTableApiModel.Data = File.ReadAllText(importTableApiModel.Data);
            }

            switch (importTableApiModel.Format)
            {
                case ImportTableFormat.Csv:

                    var lines = importTableApiModel.Data.Split(LineSeparator, StringSplitOptions.RemoveEmptyEntries);

                    var lineTokens = new List<List<string>>(lines.Length);

                    for (var i = 0; i < lines.Length; i++)
                    {
                        if (!Controllers.TryParse(lines[i], out var tokens, separator: CsvSeparator, removeEmptyEntries: false, trimEntries: true))
                            return new Result(ResultCode.Error, comment: "");

                        lineTokens.Add(tokens);
                    }

                    for (var i = 2; i < lineTokens.Count; i++)
                        table.Rows.Add(new Row { UserId = !string.IsNullOrEmpty(lineTokens[i][0]) ? int.Parse(lineTokens[i][0]) : null as int? });

                    for (var i = 1; i < lineTokens[1].Count; i++)
                        table.Columns.Add(new Column { Name = lineTokens[1][i], Order = i, TypeId = (int)(ColumnTypeValue)Enum.Parse(typeof(ColumnTypeValue), lineTokens[0][i] ?? nameof(String), ignoreCase: true)});

                    var rows = new List<Row>(table.Rows);
                    var columns = new List<Column>(table.Columns);

                    for (var i = 0; i < columns.Count; i++)
                    {
                        var typeValue = (ColumnTypeValue)columns[i].TypeId;

                        for (var j = 0; j < rows.Count; j++)
                        {
                            var value = lineTokens[j + 2][i + 1];
                            value = !string.IsNullOrEmpty(value) ? value : null;

                            if (value != null)
                                ParseRowColumnValue(columns[i], typeValue, value);

                            columns[i].Rows.Add(new RowColumn { Row = rows[j], Value = value });
                        }

                        switch (typeValue)
                        {
                            case ColumnTypeValue.Int32Enum:
                            case ColumnTypeValue.StringEnum:

                                var enumValues = new SortedSet<string>();

                                for (var k = 2; k < lineTokens.Count; k++)
                                    enumValues.Add(lineTokens[k][i + 1]);

                                columns[i].EnumValues = JsonConvert.SerializeObject(enumValues);

                                break;
                        }
                    }

                    break;

                default:
                    break;
            }

            NexusDbContext.Tables.Add(table);
            await NexusDbContext.SaveChangesAsync();

            return new Result(comment: $"Table '{table.Name}' successfully created with {table.Columns.Count} columns and {table.Rows.Count} rows");
        }

        //[Command("nexus table import", Arguments = "$name $format $data",
        //    Description = "Imports $data with the specified $format into a new Nexus table identified by $name. The available formats are: " +
        //    "csv, xml, json and file. If 'file' is specified then $data will represent a file path from which data will be read.")]
        //public Task<Result> ImportTable(CommandArgs args) => InvokeAsync<Result>(new ImportTableApiModel
        //{
        //    Name = args["$name"],
        //    Data = args["$data"],
        //    Format = (ImportTableFormat)Enum.Parse(typeof(ImportTableFormat), args["$format"], ignoreCase: true)
        //});
        #endregion

        object ParseRowColumnValue(Column column, ColumnTypeValue typeValue, string value)
        {
            var objectValue = null as object;

            if (value == null)
            {
                if (column.NotNull)
                    throw new InvalidOperationException("Database is corrupted.");

                return value;
            }

            switch (typeValue)
            {
                case ColumnTypeValue.Boolean: objectValue = bool.Parse(value); break;
                case ColumnTypeValue.Char: objectValue = char.Parse(value); break;
                case ColumnTypeValue.Byte: objectValue = byte.Parse(value); break;
                case ColumnTypeValue.SByte: objectValue = sbyte.Parse(value); break;

                case ColumnTypeValue.Int16: objectValue = short.Parse(value); break;
                case ColumnTypeValue.UInt16: objectValue = ushort.Parse(value); break;
                case ColumnTypeValue.Int32:
                case ColumnTypeValue.Int32Enum: objectValue = int.Parse(value); break;
                case ColumnTypeValue.UInt32: objectValue = uint.Parse(value); break;
                case ColumnTypeValue.Int64: objectValue = long.Parse(value); break;
                case ColumnTypeValue.UInt64: objectValue = ulong.Parse(value); break;

                case ColumnTypeValue.Single: objectValue = float.Parse(value); break;
                case ColumnTypeValue.Double: objectValue = double.Parse(value); break;
                case ColumnTypeValue.Decimal: objectValue = decimal.Parse(value); break;
                case ColumnTypeValue.Guid: objectValue = Guid.Parse(value); break;

                case ColumnTypeValue.Binary: objectValue = Convert.FromBase64String(value); break;

                case ColumnTypeValue.File:
                case ColumnTypeValue.String:
                case ColumnTypeValue.StringEnum:
                case ColumnTypeValue.Text:
                case ColumnTypeValue.LongText: objectValue = value; break;

                case ColumnTypeValue.Date: objectValue = DateTime.Parse(value).Date; break;
                case ColumnTypeValue.Time: objectValue = DateTime.Parse(value).TimeOfDay; break;
                case ColumnTypeValue.DateTime: objectValue = DateTime.Parse(value); break;
                case ColumnTypeValue.TimeSpan: objectValue = TimeSpan.Parse(value); break;
                case ColumnTypeValue.DateTimeOffset: objectValue = DateTimeOffset.Parse(value); break;
            }

            return objectValue;
        }
    }
}
