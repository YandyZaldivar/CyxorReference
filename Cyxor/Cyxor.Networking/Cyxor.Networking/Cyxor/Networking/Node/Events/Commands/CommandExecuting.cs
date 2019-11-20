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

namespace Cyxor.Networking.Events
{
    public sealed class CommandExecutingEventArgs : CancelableAsyncActionEventArgs
    {
        public override int EventId => Networking.Server.ServerEventsId.CommandExecuting;

        public bool Handled { get; set; }
        public Result Result { get; set; }

        public string Argument { get; }
        public Connection Connection { get; }
        internal ControllerAction ControllerAction { get; }

        internal CommandExecutingEventArgs(Node node, Connection connection, ControllerAction action, string argument)
           : base(node)
        {
            Argument = argument;
            Connection = connection;
            ControllerAction = action;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
