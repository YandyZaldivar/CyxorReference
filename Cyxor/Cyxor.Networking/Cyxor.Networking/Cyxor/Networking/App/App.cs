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
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Cyxor.Networking
{
    using Extensions;

    //public class HeaderEventArgs : EventArgs
    //{
    //    public bool ShowBaseHeader { get; set; }
    //    //public bool ShowBaseHeader { get; set; }
    //}

    public static class App
    {
        static Node Node;
        static string Name;
        static Mutex Mutex;

        static ManualResetEventSlim ProgramCompletedEvent = new ManualResetEventSlim();

        public static Result Result { get; set; }

        public static void Wait() => ProgramCompletedEvent.Wait();
        public static void Terminate() => ProgramCompletedEvent.Set();

        /// <summary>
        /// Occurs whenever the project title is first shown.
        /// </summary>
        public static event EventHandler<EventArgs> HeaderShown;

        static void PrintHeader()
        {
            //var hotColor = ConsoleColor.Cyan;
            var baseColor = ConsoleColor.DarkCyan;

            Console.ForegroundColor = baseColor;
            for (var i = 0; i < Name.Length; i++)
                Console.Write('=');
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            if (Node.IsServer)
                Console.BackgroundColor = ConsoleColor.Red;
            else
                Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(Name);
            Console.ForegroundColor = baseColor;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($"  v{Node.Config.UserVersion}");
            for (var i = 0; i < Name.Length; i++)
                Console.Write('=');
            Console.WriteLine();
        }

        static void PrintLogo(int left, int top, ConsoleColor color = ConsoleColor.Cyan)
        {
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            var foregroundColor = Console.ForegroundColor;

            Console.ForegroundColor = color;

            Console.SetCursorPosition(left, top);
            Console.Write("_ _");
            Console.SetCursorPosition(left - 1, top + 1);
            Console.Write("/ | \\");
            Console.SetCursorPosition(left - 2, top + 2);
            Console.Write("[ \\ / ]");
            Console.SetCursorPosition(left - 1, top + 3);
            Console.Write("\\_|_/");

            Console.ForegroundColor = foregroundColor;
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        static void ShowTitle()
        {
//#if NESTANDARD1_3
            Console.Title = $"Cyxor - {Name}";
//#else
//            Console.Title = $"Cyxor - {Name} - {Environment.OSVersion.VersionString}";
//#endif

            var hotColor = ConsoleColor.Cyan;
            var baseColor = ConsoleColor.DarkCyan;
            //var infoColor = ConsoleColor.DarkYellow;

            Console.Clear();

            if (!Node.Config.Console.HideBaseHeader)
                PrintHeader();

            Console.WriteLine();

            var headerCursorTop = Console.CursorTop;

            HeaderShown?.Invoke(Node, EventArgs.Empty);

            var startCursorTop = Console.CursorTop;

            if (startCursorTop > headerCursorTop)
                Console.WriteLine();
            else
                startCursorTop--;

            var margenLength = 4;
            var versionValue = $" v{Version.Value}  ";
            var coreTypeName = $"  [{nameof(Cyxor)}] {(Node.IsServer ? nameof(Server) : nameof(Client))}";
            var slogan = "       .NET Core Backend Framework  ";
            var charCount = coreTypeName.Length + versionValue.Length + slogan.Length + 2;

            //Console.ForegroundColor = baseColor;

            for (var i = 0; i < margenLength; i++)
                Console.Write(' ');

            for (var i = 0; i < charCount; i++)
                Console.Write(i == 0 || i == charCount - 1 ? '+' : '-');

            Console.WriteLine();
            //Console.ForegroundColor = baseColor;
            for (var i = 0; i < margenLength; i++)
                Console.Write(' ');
            Console.Write("|");
            Console.ForegroundColor = hotColor;
            Console.Write(coreTypeName);
            Console.ForegroundColor = baseColor;
            Console.Write(versionValue);
            Console.ForegroundColor = hotColor;
            Console.Write(slogan);
            Console.ForegroundColor = baseColor;
            Console.WriteLine('|');

            //Console.ForegroundColor = baseColor;

            for (var i = 0; i < margenLength; i++)
                Console.Write(' ');

            var cyxorCopyrightLine = "  Copyright (C)  2017 Yandy Zaldivar  ";
            var cyxorLicenseLine = "  Tíis program is free software; type 'license' for details  ";

            charCount = margenLength + cyxorLicenseLine.Length - 2;

            for (var i = 0; i < margenLength + (cyxorLicenseLine.Length - 2); i++)
                Console.Write(i == 0 || i == charCount - 1 ? '+' : '-');

            Console.WriteLine();

            charCount = cyxorLicenseLine.Length - cyxorCopyrightLine.Length;

            for (var i = 0; i < margenLength; i++)
                Console.Write(' ');
            Console.Write("|");
            Console.ForegroundColor = baseColor;
            Console.Write(cyxorCopyrightLine);
            //for (var i = 0; i < (margenLength + cyxorLicenseLine.Length) - (cyxorCopyrightLine.Length + 4); i++)
            for (var i = 0; i < charCount; i++)
                Console.Write(' ');
            Console.ForegroundColor = baseColor;
            Console.WriteLine('|');

            for (var i = 0; i < margenLength; i++)
                Console.Write(' ');
            Console.Write("|");
            Console.ForegroundColor = baseColor;
            Console.Write(cyxorLicenseLine);
            Console.ForegroundColor = baseColor;
            Console.WriteLine('|');

            for (var i = 0; i < margenLength; i++)
                Console.Write(' ');

            charCount = margenLength + cyxorLicenseLine.Length - 2;

            for (var i = 0; i < charCount; i++)
                Console.Write(i == 0 || i == charCount - 1 ? '+' : '-');

            Console.ForegroundColor = baseColor;

            Console.WriteLine(Environment.NewLine);

            for (var i = 0; i < Node.Config.Console.MargenLength; i++)
                Console.Write(' ');

            Console.WriteLine("Type 'help' to see usage information");

            Console.WriteLine();

            PrintLogo(31, startCursorTop, ConsoleColor.Cyan);
        }

        static bool CheckExclusiveProcess()
        {
            if (!Node.Config.ExclusiveProcess)
                return true;

            Mutex = new Mutex(initiallyOwned: true, name: Node.Config.ExclusiveProcessName, createdNew: out var createdNew);

            if (!createdNew)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("An instance of ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(Node.Config.ExclusiveProcessName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(" is already running.");
                Console.WriteLine();
                Console.WriteLine("You can only run one instance of this process on the same machine.");

                Mutex.Dispose();
                Mutex = null;

                return false;
            }

            return true;
        }



        static async void CommandLoop()
        {
            var result = Result.Success;

            try
            {
                var line = default(string);

                if (Node.Config.AppAutoStart)
                    line = nameof(Controllers.NodeController.Connect).ToLowerInvariant();

                do
                {
                    if (Console.CursorLeft != 0)
                        Node.Log(LogCategory.NewLine, "");

                    Node.Log(LogCategory.Success, indentLevel: -1, message: $"{nameof(Cyxor)}>");

                    Node.WaitingForCommand = true;

                    if (line == default)
                        line = (await Console.In.ReadLineAsync().ConfigureAwait(false)).Trim();
                    else
                        Node.Log(LogCategory.White, line);

                    Node.WaitingForCommand = false;

                    if (ProgramCompletedEvent.IsSet)
                        return;

                    switch (line)
                    {
                        case "":
                        case null: break;

                        default:
                        {
                            try
                            {
                                Node.Log(LogCategory.Busy, indentLevel: -1, message: "");
                                result = await Node.Controllers.ExecuteAsync(line).ConfigureAwait(false);
                            }
                            catch (Exception exc)
                            {
                                result = new Result(ResultCode.Exception, exception: exc);
                            }
                            finally
                            {
                                Node.Log(result, isCommandResult: true);
                            }

                            break;
                        }
                    }

                    line = default;
                }
                while (true);
            }
            catch (Exception exc)
            {
                result = new Result(ResultCode.Exception, exception: exc);
                Node.Log(result);
            }
            finally
            {
                //if (!result)
                //    node.Log(result);

                //Exit();
            }
        }

        public static void Run(Node node, string[] args) => Run(node, name: null, args: args);

        public static void Run(Node node, string name) => Run(node, name, args: null);

        public static void Run(Node node) => Run(node, name: null, args: null);

        // TODO: Treat args as commands to execute with ExecuteAsync sequentially
        public static void Run(Node node, string name, string[] args)
        {
            Node = node;
            Name = name ?? Node.Config.Name;

            Node.AppMode = true;

            Console.BackgroundColor = ConsoleColor.Black;

            //Console.InputEncoding = Encoding.GetEncoding(Encoding.UTF8.WebName);
            //Console.OutputEncoding = Encoding.GetEncoding(Encoding.UTF8.WebName);

            //Console.InputEncoding = Encoding.GetEncoding(Node.Config.Console.InputEncodingName);
            //Console.OutputEncoding = Encoding.GetEncoding(Node.Config.Console.OutputEncodingName);

            //var result = Result.Success;

            try
            {
                //var cmd = new Cmd(Framework);
                var log = new Logging(Node);
                //var config = new Config(Framework);

                TaskScheduler.UnobservedTaskException += (s, e) =>
                {
                    e.SetObserved();
                    Result = new Result(ResultCode.Exception, exception: e.Exception);
                    ProgramCompletedEvent.Set();

                    Node.Log(LogCategory.Fatal, exception: e.Exception);
                };

                Console.CancelKeyPress += (s, e) =>
                {
                    //e.Cancel = true;
                    if (!ProgramCompletedEvent.IsSet)
                        ProgramCompletedEvent.Set();
                    //Console.In.Dispose();
                };

                ShowTitle();

                if (!CheckExclusiveProcess())
                    Result = new Result(ResultCode.Error);

                //if (!(result = config.Process(args)))
                //    return result;

                log.Process();

                Utilities.Task.Run(() => CommandLoop());

                //Utilities.Task.Run(async () =>
                //{
                //    if (node.Config.AppAutoStart)
                //        Result = await Node.ConnectAsync().ConfigureAwait(false);

                //    Node.Log(LogCategory.Fatal, Result.Comment);

                //    CommandLoop();
                //});

                ProgramCompletedEvent.Wait();
                log.StopAsync().Wait();

                //if (result)
                //{
                //    result = new Result(ResultCode.Error, "Exiting process...");
                //    Thread.Sleep(5000);
                //}
                //else
                //    result = new Result(ResultCode.Error, "Process terminated.");

                //Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.SetCursorPosition(0, Console.CursorTop);
                //Console.WriteLine(" ");
                //Console.WriteLine(result.Comment);

                //return Result.Success;
            }
            catch (Exception exc)
            {
                Result = new Result(ResultCode.Exception, exception: exc);
                //return Result;
            }
            finally
            {
                Mutex?.ReleaseMutex();
                Mutex?.Dispose();
                Mutex = null;

                Console.WriteLine(Environment.NewLine);

                if (Result)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{Result.Code}: ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(Result.Comment);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                }

                Console.WriteLine("Process terminated, press a key to exit...");
                Console.ResetColor();
                Console.ReadKey(intercept: true);
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
