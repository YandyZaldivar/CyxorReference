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
    using static Utilities.Threading;

#if NET35
    using Extensions;
#endif

    public enum LinkageState
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Disconnecting = 3,
    }

    public abstract partial class Node
    {
        public sealed class NodeLinkage : NodeProperty
        {
            Mutex Mutex;
            InterlockedInt Interlocked;

            public LinkageState State => (LinkageState)Interlocked.Value;
            public bool IsCompleted => Interlocked.Value == 0 || Interlocked.Value == 2;

            internal NodeLinkage(Node node) : base(node) { }

            internal Result ConnectionAcquire()
            //internal async Task<Result> ConnectionAcquireAsync()
            {
                var result = Result.Success;

                if (Interlocked.CompareExchange(1, 0) != 0)
                    result = new Result(ResultCode.Error, "Node connection acquire failed.");
                else if (Node.Config.ExclusiveProcess && !Node.AppMode)
                {
                    Mutex = new Mutex(true, Node.Config.ExclusiveProcessName, out var createdNew);

                    if (!createdNew)
                    {
                        Mutex.Dispose();
                        Mutex = null;
                        var sc = Node.IsServer ? nameof(Server).ToLowerInvariant() : nameof(Client).ToLowerInvariant();
                        var message = $"An instance of '{Node.Config.ExclusiveProcessName}' {sc} is already running. " +
                            $"You can only run one instance of this {sc} process on the same machine.";

                        Node.Log(LogCategory.Warning, message);

                        result = new Result(ResultCode.Error, message);
                    }
                }

                //await Node.LoadPluginsAsync();

                //if (result)
                //    Node.ConnectionScope = Node.CreateScope(resetServiceProvider: true);

                return result;
            }

            internal Result DisconnectionAcquire()
            {
                var result = Interlocked.CompareExchange(3, 2) == 2;

                if (result)
                    Node.ConnectionScope = Node.CreateScope();

                return result;
            }

            internal void ConnectionRelease(Result result)
            {
                if (Interlocked.Exchange(result ? 2 : 0) != 1)
                    throw new InvalidOperationException(Utilities.ResourceStrings.CyxorInternalException);

                Node.ConnectionScope?.Dispose();
                Node.ConnectionScope = null;
            }

            internal void DisconnectionRelease(Result result)
            {
                if (Interlocked.Exchange(result ? 0 : 2) != 3)
                    throw new InvalidOperationException(Utilities.ResourceStrings.CyxorInternalException);

                Node.ConnectionScope?.Dispose();
                Node.ConnectionScope = null;

                Mutex?.ReleaseMutex();
                Mutex?.Dispose();
                Mutex = null;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
