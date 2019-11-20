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

namespace Cyxor.Networking
{
    class ProtocolMap
    {
        // If not fin and not continuation then full message length will follow plus channel.
        // If continuation or (fin and continuation) then channel will follow.

        //000 - From Node to Client
        //001 - From Client to Client                     x
        //010 - Free
        //100 - Broadcast From Server to Client
        //011 - Free
        //101 - Broadcast From Client to Client           x
        //110 - Group Broadcast From Server to Client
        //111 - Group Broadcast From Client to Client     x

        internal const byte

            Fin = 0,
            Rsv1 = 1,
            Rsv2 = 2,
            Rsv3 = 3,
            Opcode = 4,
            //Opcode = 5,
            //Opcode = 6,
            //Opcode = 7,

            Mask = 0,
            PayloadLength = 1,
            //Length = 2,
            //Length = 3,
            //Length = 4,
            //Length = 5,
            //Length = 6,
            //Length = 7,

            // TODO: Swap Broadcast with Reserved4 to implement modules??
            Encrypted = 0,
            Command = 1,
            QueryMode = 2,
            //QueryMode = 3,
            Broadcast = 4,
            Address = 5,
            Sender = 6,
            CfgByte2 = 7,

            Error = 0,
            Progress = 1,
            Compress = 2,
            Anonymous = 3,
            Reserved4 = 4,
            Priority = 5,
            //Priority = 6,
            CfgByte3 = 7;
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
