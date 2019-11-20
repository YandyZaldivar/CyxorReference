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
    using Serialization;

    internal interface IBox
    {
        int Id { get; set; }
        bool ReadOnly { get; }
        string Sender { get; }
        Serializer Serializer { get; }
        bool Encrypt { get; set; }
        bool IsFromNode { get; }
        bool Compress { get; set; }
        bool Anonymous { get; set; }
        bool Broadcast { get; set; }
        string Address { get; set; }
        bool IsGroupAddress { get; }
        PacketSending Sending { get; set; }
        PacketProtocol Protocol { get; set; }
        PacketPriority Priority { get; set; }
        PacketQueryMode QueryMode { get; }
        bool TrackSendProgress { get; set; }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
