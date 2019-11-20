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
using System.Diagnostics;
using System.Threading.Tasks;

namespace Cyxor.Networking
{
    using static Utilities.Threading;

    public partial class Master
    {
        public partial class MasterDatabase : MasterProperty
        {
            public class EngineManager : MasterProperty
            {
                protected int IndentLevel;
                protected Result ConnectResult;

                protected int ReadyCount;

                protected Process Process;
                protected Awaitable ProcessAwaitable;
                protected Awaitable ProcessStartedAwaitable;

                protected volatile bool ProgrammedShutdown = false;

                public EngineManager(Master master) : base(master)
                {

                }

                public virtual Result IsConnected() => Result.Success;
                protected internal virtual Task<Result> ConnectAsync() => Task.FromResult(Result.Success);
                protected internal virtual Task<Result> DisconnectAsync() => Task.FromResult(Result.Success);
                public virtual Task<Result> DumpWriteAsync() => Task.FromResult(Result.Success);
                public virtual Task<Result> DumpLoadAsync(string sqlDump) => Task.FromResult(Result.Success);

                protected virtual void DataReceived(object sender, DataReceivedEventArgs e)
                {

                }

                protected virtual void Exited(object sender, EventArgs e)
                {
                    if (Process.ExitCode == 0)
                        ConnectResult = new Result(ResultCode.Success, "Database server stopped.");
                    else
                        ConnectResult = new Result(ResultCode.Error, "Database server process terminated with errors.");

                    if (!ProgrammedShutdown)
                        Master.Log(LogCategory.Fatal, IndentLevel + 1, "The database server has been shutdown unexpectedly");

                    Process.Dispose();
                    Process = null;

                    ReadyCount = 0;
                    ProcessStartedAwaitable.TrySetResult(ConnectResult);
                    ProcessAwaitable.TrySetResult(ConnectResult);

                    if (!ProgrammedShutdown)
                        Node.DisconnectAsync();
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
