﻿/*
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

namespace Cyxor.Networking.Events
{
    public sealed class PacketReceiveProgressChangedEventArgs : ActionEventArgs
    {
        public override int EventId => Node.NodeEventsId.PacketReceiveProgressChanged;

        public readonly int Type;
        public readonly int TotalSize;
        public readonly int ProgressPercent;
        public readonly int BytesTransferred;
        public readonly Connection Connection;

        internal PacketReceiveProgressChangedEventArgs(Link link, ReceiveProgress progress)
           : base(link.Node)
        {
            Connection = link.Connection;
            TotalSize = progress.TotalSize;

            Type = progress.Type;
            BytesTransferred = progress.BytesTransferred;
            ProgressPercent = (int)((ulong)BytesTransferred * 100 / (ulong)TotalSize);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
