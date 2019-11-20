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

namespace Cyxor.Networking.Events.Server
{
    public sealed class DbLoadProgressChangedEventArgs : ActionEventArgs
    {
        public override int EventId => Master.MasterEventsId.DbLoadProgressChanged;

        public readonly string Comment;
        public readonly int IndentLevel;
        public readonly int ProgressPercent;

        public DbLoadProgressChangedEventArgs(Node node, int progressPercent, string comment)
           : this(node, 0, progressPercent, comment)
        { }

        public DbLoadProgressChangedEventArgs(Node node, int indentLevel, int progressPercent, string comment)
           : base(node)
        {
            Comment = comment;
            IndentLevel = indentLevel;
            ProgressPercent = progressPercent;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
