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
using System.IO;
using System.Linq;
using System.Threading;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Cyxor.Networking
{
    using Data;
    using Config.Server;

    using static Utilities.Threading;

    public partial class Master
    {
        public partial class MasterDatabase : MasterProperty
        {
            public class MySqlServerManager : EngineManager
            {
                //DbConnection DbConnection;



                //FileStream LogFile;
                //string TemporaryPassword;
                //StreamReader LogFileReader;

                //static char dest = '-';

                public MySqlServerManager(Master master) : base(master)
                {
                    //DbConnection = master.Config.DatabaseEngine.
                }

                public override Result IsConnected()
                {
                    var result = Result.Success;

                    if (Process?.HasExited ?? true)
                        return result = new Result(ResultCode.Error, "The MySQL server process is not running.");

                    return result;
                }

                public override async Task<Result> DumpWriteAsync()
                {
                    //mysqlpump --user=root --password=cyxor --host=localhost --port=12484 --databases ufo --add-drop-database=true --result-file=cyxor.sql

                    var result = Result.Success;

                    try
                    {
                        var dumpAwaitable = new Awaitable();

                        Master.Log(LogCategory.Message, "Creating database dump...");

                        var dump = new Process
                        {
                            EnableRaisingEvents = true,

                            StartInfo = new ProcessStartInfo
                            {
                                FileName = Master.Config.Database.Engine.ServerDumpFile,
                                //Arguments = $"--user=root --password={DefaultRootPassword} --host=localhost --port={DefaultPort} --databases ufo --add-drop-database=true --result-file=cyxor.sql"
                                Arguments = $" --no-defaults" +
                                            $" --{nameof(DatabaseEngineConfig.User).ToLowerInvariant()} = {Master.Config.Database.Engine.User}" +
                                            $" --{nameof(DatabaseEngineConfig.Password).ToLowerInvariant()} = {Master.Config.Database.Engine.Password}" +
                                            $" --host = {Master.Config.Database.Engine.Server}" +
                                            $" --{nameof(DatabaseEngineConfig.Port).ToLowerInvariant()} = {Master.Config.Database.Engine.Port}" +
                                            $" --databases datadin2 --add-drop-database = true --result-file = cyxor.sql"
                                            //$"  --"
                            }
                        };

                        //dump.Exited += async (s, e) =>
                        dump.Exited += (s, e) =>
                        {
                            if (dump.ExitCode == 0)
                                result = new Result(ResultCode.Success, "MySQL system database initialized successfully.");
                            else
                                result = new Result(ResultCode.Error, "MySQL system database initialization failed.");

                            Master.Log(result);

                            // TODO: await for awaitableprocess to write a the key line
                            //await ProcessLog(indentLevel: 3);

                            dumpAwaitable.TrySetResult(result);
                        };

                        dump.Start();

                        result = await dumpAwaitable.ConfigureAwait(false);

                        dump.Dispose();


                        return result;
                    }
                    catch (Exception ex)
                    {
                        result = new Result(exception: ex);
                        return result;
                    }
                    finally
                    {

                    }
                }

                public override Task<Result> DumpLoadAsync(string sqlDump)
                {
                    var result = Result.Success;

                    try
                    {
                        return Task.FromResult(result);
                    }
                    catch
                    {
                        return Task.FromResult(result);
                    }
                    finally
                    {

                    }
                }

                async Task<Result> InitializeDataDirAsync()
                {
                    var result = Result.Success;

                    Master.Config.Database.Engine.FirstRun = false;

                    if (Directory.Exists(Master.Config.Database.Engine.DataDir))
                        return result;

                    Master.Config.Database.Engine.FirstRun = true;

                    var myinit = default(Process);
                    var myinitDbAwaitable = new Awaitable();

                    try
                    {
                        Master.Log(LogCategory.Information, "MySQL server data directory missing.");
                        Master.Log(LogCategory.Message, "Initializing MySQL system database and populating tables...");

                        //if (!Directory.Exists(Master.Config.Database.Engine.DataDir))
                        //    Directory.CreateDirectory(Master.Config.Database.Engine.DataDir);

                        myinit = new Process
                        {
                            EnableRaisingEvents = true,

                            StartInfo = new ProcessStartInfo
                            {
                                CreateNoWindow = true,
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true,
                                StandardErrorEncoding = System.Text.Encoding.UTF8,

                                FileName = Master.Config.Database.Engine.ServerFile,
                                Arguments = $"{Master.Config.Database.Engine.Arguments} --initialize-insecure --console"
                                //Arguments = $"{Master.Config.Database.Engine.Arguments} --initialize-insecure"
                            }
                        };

                        myinit.ErrorDataReceived += DataReceived;
                        myinit.OutputDataReceived += DataReceived;
                        //myinit.Exited += Exited;

                        myinit.Exited += (s, e) =>
                        {
                            if (myinit.ExitCode == 0)
                                result = new Result(ResultCode.Success, "MySQL system database initialized successfully.");
                            else
                                result = new Result(ResultCode.Error, $"MySQL system database initialization failed. " +
                                    $"MySQL exit code: {myinit.ExitCode}");

                            myinitDbAwaitable.TrySetResult(result);
                        };

                        myinit.Start();

                        myinit.BeginErrorReadLine();
                        myinit.BeginOutputReadLine();

                        result = await myinitDbAwaitable.ConfigureAwait(false);
                    }
                    catch (Exception exc)
                    {
                        result = new Result(ResultCode.Error, comment: exc.Message);

                        //if (Directory.Exists(Master.Config.Database.Engine.DataDir))
                        //    Directory.Delete(Master.Config.Database.Engine.DataDir, recursive: true);
                    }
                    finally
                    {
                        myinit?.Dispose();
                    }

                    return result;
                }

                async Task<Result> GetMySqlProcessAsync()
                {
                    var result = Result.Success;

                    try
                    {
                        if (File.Exists(Master.Config.Database.Engine.ProcessPidFile))
                        {
                            //var mysqlProcessId = default(int);

                            var idString = File.ReadAllText(Master.Config.Database.Engine.ProcessPidFile);

                            if (!int.TryParse(idString, out var mysqlProcessId))
                            {
                                if (File.Exists(Master.Config.Database.Engine.ProcessPidFile))
                                    return result = new Result(ResultCode.Error, $"Failed to read MySQL server process id from '{Master.Config.Database.Engine.ProcessPidFile}'.");
                            }

                            Master.Config.Database.Engine.ProcessId = mysqlProcessId;
                        }

                        if (Master.Config.Database.Engine.ProcessId != DatabaseEngineConfig.DefaultProcessId)
                        {
                            var process = Process.GetProcessById(Master.Config.Database.Engine.ProcessId);

                            // NOTE: We can decide to reuse the process instead of killing it. But we need to investigate
                            //       how to get read access to the process log file when the process is already running.
                            if (string.Compare("mysqld", process.ProcessName, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                process.Kill();

                                await Task.Run(() => process.WaitForExit());

                                process.Dispose();
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        result = new Result(ResultCode.Error, comment: exc.Message, innerResult: result);
                    }
                    finally
                    {

                    }

                    return result;
                }

                protected override void DataReceived(object sender, DataReceivedEventArgs e)
                {
                    var il = IndentLevel + 1;

                    var line = e.Data;

                    if (string.IsNullOrWhiteSpace(line))
                        return;

                    //var tmpPasswordOutput = "A temporary password is generated for root@localhost: ";
                    //var tmpPasswordIndex = line.IndexOf(tmpPasswordOutput);

                    //if (tmpPasswordIndex != -1)
                    //    TemporaryPassword = line.Substring(tmpPasswordIndex + tmpPasswordOutput.Length);

                    var index = line.IndexOf("[");

                    if (index < 0 || index >= line.Length)
                        index = 0;

                    line = line.Substring(index);

                    if (line.IndexOf("[NOTE]", StringComparison.OrdinalIgnoreCase) != -1)
                        Master.Log(LogCategory.Operation, il, line);
                    else if (line.IndexOf("[WARNING]", StringComparison.OrdinalIgnoreCase) != -1)
                        Master.Log(LogCategory.Information, il, line);
                    else if (line.IndexOf("[ERROR]", StringComparison.OrdinalIgnoreCase) != -1)
                        Master.Log(LogCategory.Error, il, line);
                    else
                        Master.Log(LogCategory.HotMessage, il, line);

                    if (line.IndexOf("ready for connections", StringComparison.OrdinalIgnoreCase) != -1)
                        ReadyCount++;

                    if (line.IndexOf("MySQL Community Server", StringComparison.OrdinalIgnoreCase) != -1)
                        ReadyCount++;

                    if (ReadyCount == 2)
                        ProcessStartedAwaitable.TrySetResult(Result.Success);
                }

                protected internal override async Task<Result> ConnectAsync()
                {
                    IndentLevel = 3;
                    var result = Result.Success;
                    var needReleaseConnection = false;
                    var dbContext = default(DbContext);
                    var scope = default(IServiceScope);

                    var workingDirectory = Directory.GetCurrentDirectory();

                    try
                    {
                        scope = Master.CreateScope();

                        dbContext = scope.GetService<MasterDbContext>();

                        if (dbContext == null)
                            dbContext = scope.GetServices<DbContext>().FirstOrDefault();

                        if (dbContext == null)
                        {
                            //Node.Log();
                            result = new Result(ResultCode.Error, "No database context were found in the " +
                                "available services. You must add one or disable the database engine.");
                            Node.Log(result);
                            return result;
                        }

#if DEBUG
                        if (!Directory.Exists(Master.Config.Database.Engine.BaseDir))
                        {
                            Directory.SetCurrentDirectory("D:/Documents/Visual Studio 2017/Projects/Cyxor/database/");
                            //Master.Config.Database.Engine.BaseDir = @"D:\Documents\Visual Studio 2017\Projects\Cyxor\database\" + Master.Config.Database.Engine.BaseDir;
                        }
#endif

                        if (!(result = await InitializeDataDirAsync().ConfigureAwait(false)))
                        {
                            Node.Log(result);
                            return result;
                        }

                        //if (!(result = await GetMySqlProcessAsync().ConfigureAwait(false)))
                        //    return result;

                        await GetMySqlProcessAsync().ConfigureAwait(false);


                        //if (LogFile == null && !Master.Config.Database.Engine.FirstRun)
                        //{
                        //    if (File.Exists(Master.Config.Database.Engine.ProcessLogFile))
                        //        File.Delete(Master.Config.Database.Engine.ProcessLogFile);

                        //    LogFile = File.Open(Master.Config.Database.Engine.ProcessLogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                        //    //File.Open(DefaultProcessLogFile, FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite).Dispose();
                        //    //LogFileReader?.Dispose();
                        //    LogFileReader = new StreamReader(LogFile);
                        //}



                        ProcessStartedAwaitable?.Reset();
                        ProcessStartedAwaitable = ProcessStartedAwaitable ?? new Awaitable();

                        ProcessAwaitable?.Reset();
                        ProcessAwaitable = ProcessAwaitable ?? new Awaitable();

                        Process = new Process
                        {
                            EnableRaisingEvents = true,

                            StartInfo = new ProcessStartInfo
                            {
                                CreateNoWindow = true,
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true,
                                StandardErrorEncoding = System.Text.Encoding.UTF8,

                                FileName = Master.Config.Database.Engine.ServerFile,
                                //Arguments = $"--defaults-file=\"{Master.Config.Database.Engine.ConfigFile}\""
                                //Arguments = $"--defaults-file=\"{Master.Config.Database.Engine.ConfigFile}\" --console"
                                Arguments = $"{Master.Config.Database.Engine.Arguments} --console"
                            },
                        };

                        Process.ErrorDataReceived += DataReceived;
                        Process.OutputDataReceived += DataReceived;
                        Process.Exited += Exited;

                        //MySqlProcess.Exited += (s, e) =>
                        //{
                        //    if (MySqlProcess.ExitCode == 0)
                        //        result = new Result(ResultCode.Success, "MySQL server stopped.");
                        //    else
                        //        result = new Result(ResultCode.Error, "MySQL server process terminated with errors.");

                        //    if (!ProgrammedShutdown)
                        //    {
                        //        Master.Log(LogCategory.Fatal, IndentLevel + 1, "The database engine process has been shutdown unexpectedly");
                        //    }

                        //    MySqlProcess.Dispose();
                        //    MySqlProcess = null;

                        //    MySqlStartAwaitable.TrySetResult(result);
                        //    MySqlProcessAwaitable.TrySetResult(result);

                        //    if (!ProgrammedShutdown)
                        //        Node.DisconnectAsync("");
                        //};

                        Master.Log(LogCategory.OperationHeader, IndentLevel, $"Starting {Master.Config.Database.Engine.Provider} server...");

                        try
                        {
                            Process.Start();
                            var startTime = DateTime.Now;

                            Process.BeginErrorReadLine();
                            Process.BeginOutputReadLine();

                            await ProcessStartedAwaitable.ConfigureAwait(false);

                            while (true)
                            {
                                var connection = default(DbConnection);
                                //await Task.Delay(Master.Config.Database.Engine.WaitForStartMillisecondsDelay).ConfigureAwait(false);


                                //Borrar line
                                //Master.Config.Database.Engine.FirstRun = true;
                                //Borrar line


                                try
                                {
                                    connection = dbContext.Database.GetDbConnection();

                                    connection.ConnectionString = Master.Config.Database.Engine
                                        .GetConnectionString(excludePassword: Master.Config.Database.Engine.FirstRun);

                                    await dbContext.Database.OpenConnectionAsync().ConfigureAwait(false);

                                    needReleaseConnection = true;

                                    Master.Config.Database.Engine.ProcessId = Process.Id;
                                }
                                catch (Exception ex)
                                {
                                    if (connection == null)
                                        continue;

                                    if (ex.Message.IndexOf("Unable to connect to any of the specified MySQL hosts", StringComparison.OrdinalIgnoreCase) == -1)
                                        return result = new Result(ResultCode.Exception, exception: ex);

                                    if ((DateTime.Now - startTime).TotalMilliseconds > Master.Config.Database.Engine.WaitForStartMilliseconds)
                                        return result = new Result(ResultCode.OperationTimedOut, "Time expired while trying to connect to the specified MySQL host.");

                                    continue;
                                }

                                //if (connection.State != System.Data.ConnectionState.Open)
                                if (dbContext.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
                                    return result = new Result(ResultCode.Error, "The connection to the MySQL host was established but the current connection state is not open.");

                                Process.PriorityClass = Master.Config.Database.Engine.ProcessPriorityClass;

                                break;
                            }
                        }
                        finally
                        {
                            if (result.Code != ResultCode.OperationTimedOut)
                            {
                                // TODO: Log
                                //await ProcessLog(indentLevel: il + 1);
                            }

                            Master.Log(LogCategory.Information, IndentLevel + 1, $"MySQL process priority class set to {Process.PriorityClass}");

                            if (result)
                                Master.Log(LogCategory.Success, IndentLevel, $"{Master.Config.Database.Engine.Provider} server successfully started");
                            else
                                Master.Log(LogCategory.ErrorHeader, IndentLevel, $"{Master.Config.Database.Engine.Provider} server starting failed");
                        }

                        if (Master.Config.Database.Engine.FirstRun)
                        {
                            var commandResult = -1;

                            Master.Log(LogCategory.Information, "Setting up a new MySQL password...");

                            try
                            {
                                //var command = connection.CreateCommand();
                                //command.CommandText = $"alter user 'root'@'localhost' identified by '{Master.Config.Database.Engine.Password}'";
                                //commandResult = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                                ////command.Dispose(); TODO: test!

                                // TODO: Test this
                                // TODO: Remove 'with mysql_native_password' when SQLyog support it.
                                // If need to set all privileges to that user:
                                // grant all privileges on *.* to 'user'@'localhost' with grant option;
                                var cmdstr = $"alter user '{Master.Config.Database.Engine.User}'@'localhost' identified with mysql_native_password by '{Master.Config.Database.Engine.Password}'";
                                //var cmdstr = $"alter user '{Master.Config.Database.Engine.User}'@'localhost' identified by '{Master.Config.Database.Engine.Password}'";
                                commandResult = await dbContext.Database.ExecuteSqlCommandAsync(cmdstr).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                return result = new Result(ResultCode.Exception, exception: ex);
                            }
                            finally
                            {
                                if (commandResult != 0)
                                    Master.Log(LogCategory.Error, "MySQL password changing failed.");
                                else
                                    Master.Log(LogCategory.Information, "MySQL password successfully changed.");
                            }
                        }

                        return result;
                    }
                    catch (Exception ex)
                    {
                        return result = new Result(ResultCode.Exception, exception: ex, innerResult: result);
                    }
                    finally
                    {
                        if (needReleaseConnection)
                            try { dbContext.Database.CloseConnection(); }
                            catch { }

                        scope?.Dispose();

                        //connection?.Close();
                        //connection?.Dispose();

                        //dbContext.Database.CloseConnection();
                        //dbContext.Dispose();

                        //await Task.Delay(ServerStepDelay).ConfigureAwait(false);

                        //Master.Log(result);

                        Directory.SetCurrentDirectory(workingDirectory);
                    }
                }

                protected internal override async Task<Result> DisconnectAsync()
                {
                    var result = Result.Success;
                    var dbContext = default(DbContext);
                    var scope = default(IServiceScope);

                    try
                    {
                        scope = Master.CreateScope();

                        dbContext = scope.GetService<MasterDbContext>();

                        if (dbContext == null)
                            dbContext = scope.GetServices<DbContext>().FirstOrDefault();

                        if (dbContext == null)
                            return result;
                        
                        Master.Log(LogCategory.OperationHeader, 3, $"Stopping {Master.Config.Database.Engine.Provider} server...");

                        if (Process?.HasExited ?? true)
                            return result;

                        //connection = Master.Config.Database.Engine.DbFactory.CreateConnection();
                        //connection.ConnectionString = Master.Config.Database.Engine.GetConnectionString();

                        //await connection.OpenAsync().ConfigureAwait(false);

                        //if (connection.State != System.Data.ConnectionState.Open)
                        //    return result = new Result(ResultCode.Error, "Connection to the database engine process failed.");

                        ProgrammedShutdown = true;

                        //var command = connection.CreateCommand();
                        //command.CommandText = $"shutdown";
                        //var commandResult = await command.ExecuteNonQueryAsync().ConfigureAwait(false);


                        var commandResult = await dbContext.Database.ExecuteSqlCommandAsync("shutdown");


                        if (commandResult != 0)
                            return result = new Result(ResultCode.Error, $"Shutdown command execution failed with result '{commandResult}'.");

                        return result = await ProcessAwaitable.ConfigureAwait(false);
                    }
                    catch (Exception exc)
                    {
                        return result = new Result(ResultCode.Error, comment: exc.Message);
                    }
                    finally
                    {
                        try
                        {
                            dbContext?.Database.CloseConnection();
                            scope?.Dispose();

                            //connection?.Close();
                            //connection?.Dispose();

                            //dbContext.Database.CloseConnection();
                            //dbContext.Dispose();

                            ProgrammedShutdown = false;
                            var mySqlProcessResult = Result.Success;

                            if (!ProcessAwaitable?.IsCompleted ?? false)
                            {
                                Process?.Kill();
                                mySqlProcessResult = await (ProcessAwaitable ?? Awaitable.CompletedAwaitable).ConfigureAwait(false);
                            }

                            //if (LogFileReader != null)
                            //{
                            //    // TODO: Log
                            //    await ProcessLog(indentLevel: 4);

                            //    LogFileReader.Dispose();
                            //    LogFileReader = null;
                            //}

                            Process?.Dispose();
                            Process = null;

                            //if (result)
                            //    Master.Log(LogCategory.Information, "Database engine successfully shutdown.");
                            //else
                            //{
                            //    Master.Log(result);
                            //    Master.Log("There were some problems while shutting down the database engine and it was forcefully closed:");
                            //}

                            if (result)
                                Master.Log(LogCategory.Success, 3, $"{Master.Config.Database.Engine.Provider} server successfully stopped");
                            else
                            {
                                Master.Log(LogCategory.Information, 4, result.Comment);
                                Master.Log(LogCategory.Warning, 3, $"There were some problems while stopping the {Master.Config.Database.Engine.Provider} server and it was forcefully closed");
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
