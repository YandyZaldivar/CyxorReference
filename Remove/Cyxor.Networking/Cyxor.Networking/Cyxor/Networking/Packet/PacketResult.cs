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

    public struct PacketResponse : IDisposable
    {
        Result FullResult;

        public bool Ready { get; private set; }
        public Packet Reply { get; private set; }
        public Result Result { get; private set; }

        internal PacketResponse(Packet reply, Result result)
        {
            Ready = true;
            Reply = reply;
            FullResult = Reply?.Result ?? result;
            Result = FullResult.LightCopy();
        }

        internal void Dispose()
        {
            Reply?.Dispose();
            Ready = false;
            Reply = null;
        }

        public T GetModel<T>() => FullResult.GetModel<T>();
        public T GetModel<T>(T value) => FullResult.GetModel(value);
        public object GetModel(Type type) => FullResult.GetModel(type);
        public void PopulateObject<T>(T value) => FullResult.PopulateObject(value);
        public T GetModel<T>(IBackingSerializer serializer) => FullResult.GetModel<T>(serializer);
        public T GetModel<T>(T value, IBackingSerializer serializer) => FullResult.GetModel(value, serializer);

        void IDisposable.Dispose() => Dispose();
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
