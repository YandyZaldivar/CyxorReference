using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Alimatic.Nexus.Models
{
    public class TableDataModel : TableApiModel
    {
        public IEnumerable<RowApiModel> Rows { get; set; }
        public IEnumerable<ColumnApiModel> Columns { get; set; }
        public IEnumerable<RowColumnApiModel> RowColumns { get; set; }

        public void Export(string path, string format)
        {
            var sb = new StringBuilder();

            var delimiter = default(char);

            switch (format)
            {
                case "csv":
                case "csv,": delimiter = ','; break;
                case "csv;": delimiter = ';'; break;
                case "txt": delimiter = '\t'; break;
                default: throw new InvalidOperationException("The supplied 'format' is not supported.");
            }

            var value = Name.Contains('"') ? Name.Replace("\"", "\"\"") : Name;
            var enclose = Name.Contains('"') || Name.Contains(delimiter);
            sb.Append(enclose ? $"\"{value}\"" : value);

            var orderedColumns = Columns.OrderBy(p => p.Order);

            foreach (var column in orderedColumns)
                sb.Append($"{delimiter}{(ColumnTypeValue)column.TypeId}");

            sb.AppendLine();

            foreach (var column in orderedColumns)
            {
                value = column.Name.Contains('"') ? column.Name.Replace("\"", "\"\"") : column.Name;
                enclose = column.Name.Contains('"') || column.Name.Contains(delimiter);
                sb.Append(enclose ? $"{delimiter}\"{value}\"" : $"{delimiter}{value}");
            }

            foreach (var row in Rows)
            {
                sb.AppendLine();

                if (row.UserId != null)
                    sb.Append(row.UserId);

                var rowColumns = RowColumns.Where(p => p.RowId == row.Id).OrderBy(p => orderedColumns.Single(c => c.Id == p.ColumnId).Order);

                foreach (var rowColumn in rowColumns)
                {
                    value = rowColumn.Value?.Contains('"') ?? false ? rowColumn.Value.Replace("\"", "\"\"") : rowColumn.Value;
                    enclose = (rowColumn.Value?.Contains('"') ?? false) || (rowColumn.Value?.Contains(delimiter) ?? false);
                    sb.Append(enclose ? $"{delimiter}\"{value}\"" : $"{delimiter}{value}");
                }
            }

            File.WriteAllText(path, sb.ToString());
        }
    }
}
