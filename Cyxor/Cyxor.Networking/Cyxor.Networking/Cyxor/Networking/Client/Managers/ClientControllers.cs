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
using System.Threading;
using System.Threading.Tasks;

namespace Cyxor.Networking
{
    using Models;

    public partial class Client
    {
        public partial class ClientControllers : NodeControllers
        {
            internal override void Internal() => throw new InvalidOperationException();

            protected Client Client { get; }

            protected internal ClientControllers(Node node) : base(node) => Client = node as Client;

            public async Task<Result> ExecuteRemoteAsync(string value, int millisecondsTimeout, CancellationToken cancellationToken)
            {
                var result = Result.Success;

                try
                {
                    var values = value.Split(Separators, 2, StringSplitOptions.RemoveEmptyEntries);

                    if (values[0] != Client.Config.RemoteCommandTokenName)
                        return result = new Result(ResultCode.CommandParseError);

                    if (string.IsNullOrEmpty(values[1]))
                        return result = new Result(ResultCode.CommandParseError);

                    using (var packet = new Packet(Node) { Model = values[1] })
                    {
                        if (!await packet.CommandAsync(millisecondsTimeout, cancellationToken).ConfigureAwait(false))
                            return packet.Response.Result;

                        result = new Result(packet.Response.Result, model: packet.Response.GetModel<string>());
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    return result = new Result(ResultCode.Exception, exception: ex);
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
