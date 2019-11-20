/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace Cyxor.Models
{
    /*
    public class Utilities : Networking.Utilities
    {
        protected Utilities() { }

        //        public static class Data
        //        {
        //            public static List<string> GetDbContextSetNames(Type dbContextType)
        //            {
        //                if (dbContextType == null)
        //                    throw new ArgumentNullException();

        //                if (!dbContextType.IsSubclassOf(typeof(DbContext)))
        //                    throw new ArgumentException();

        //                var setNames = new List<string>();

        //                do
        //                {
        //                    var fields = Reflection.GetDeclaredFields(dbContextType);

        //                    foreach (var field in fields)
        //                        if (string.Compare(field.FieldType.Name, typeof(DbSet<>).Name, true) == 0)
        //                        {
        //                            int startIndex = field.Name.IndexOf('<') + 1;
        //                            int length = field.Name.IndexOf('>') - startIndex;

        //                            setNames.Add(field.Name.Substring(startIndex, length));
        //                        }

        //                    dbContextType = dbContextType.BaseType;
        //                }
        //                while (dbContextType != typeof(DbContext));

        //                return setNames;
        //            }

        //            public static List<string> GetDbContextTableNames(Type dbContextType)
        //            {
        //                if (dbContextType == null)
        //                    throw new ArgumentNullException();

        //                if (!dbContextType.IsSubclassOf(typeof(DbContext)))
        //                    throw new ArgumentException();

        //                var tableNames = new List<string>();

        //                using (var dbContext = Activator.CreateInstance(dbContextType) as DbContext)
        //                {
        //                    var connection = dbContext.Database.Connection;

        //                    if (connection.State == ConnectionState.Closed)
        //                        connection.Open();

        //                    var dataTable = connection.GetSchema("Tables");

        //                    foreach (DataRow row in dataTable.Rows)
        //                        tableNames.Add(row["TABLE_NAME"].ToString());

        //                    tableNames.Remove("__MigrationHistory");
        //                }

        //                return tableNames;
        //            }
        //        }
    }
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
