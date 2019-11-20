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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Newtonsoft.Json;

namespace Cyxor.Networking
{
    using Networking.Events;

    struct JsonReaderToken
    {
        public object Value { get; set; }
        public JsonToken Type { get; set; }
    }

    public class Logging
    {
        Task Task;
        const int Tab = 5;
        //bool Initialized = false;
        CancellationTokenSource IdleCts;
        BlockingCollection<MessageLoggedEventArgs> LogCollection;

        char Char = '-';
        const int DueTime = 200;
        volatile int IdleState = 0;

        Node Node;

        public Logging(Node node)
        {
            Node = node;

            LogCollection = new BlockingCollection<MessageLoggedEventArgs>();
            Node.Events.MessageLogged += (s, e) => LogCollection.Add(e);
            //Node.Events.ConnectCompleted += (s, e) => Initialized = true;
        }

        async void IdleTimerStart()
        {
            if (IdleState != 0)
                throw new InvalidOperationException("Wait sequence invalid state.");

            IdleState++;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;

            IdleCts = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    await Utilities.Task.Delay(DueTime, IdleCts.Token).ConfigureAwait(false);

                    if (IdleCts.IsCancellationRequested)
                        return;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(Char + "\b");

                    switch (Char)
                    {
                        case '-': Char = '\\'; break;
                        case '\\': Char = '|'; break;
                        case '|': Char = '/'; break;
                        case '/': Char = '-'; break;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
            finally
            {
                IdleCts.Dispose();
                IdleCts = null;
                Console.ForegroundColor = Console.BackgroundColor;
                Console.Write(Char + "\b");
                Console.CursorVisible = true;

                IdleState++;
            }
        }

        void IdleTimerStop()
        {
            if (IdleState != 1)
                return;

            IdleState++;
            IdleCts.Cancel();
            SpinWait.SpinUntil(() => IdleState == 3);
            IdleState = 0;
        }

        public void Process() => Task = Utilities.Task.Run(StartAsync);
        //public void Process() => Utilities.Task.Run(() => StartAsync());

        async Task StartAsync()
        //Task StartAsync()
        {
            try
            {
                var log = default(MessageLoggedEventArgs);

                while (!LogCollection.IsCompleted)
                {
                    if (!Node.Linkage.IsCompleted || (log?.Category ?? LogCategory.Message) == LogCategory.Busy)
                        IdleTimerStart();

                    if (!LogCollection.TryTake(out log, Timeout.Infinite))
                        continue;

                    IdleTimerStop();

                    switch (log.Category)
                    {
                        case LogCategory.Title: Console.ForegroundColor = ConsoleColor.Cyan; break;

                        case LogCategory.Message: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                        case LogCategory.HotMessage: Console.ForegroundColor = ConsoleColor.Gray; break;
                        case LogCategory.Trace: Console.ForegroundColor = ConsoleColor.DarkGreen; break;

                        case LogCategory.Information: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                        case LogCategory.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;

                        case LogCategory.Operation: Console.ForegroundColor = Node.Config.Console.OperationForegroundColor; break;
                        case LogCategory.Success: Console.ForegroundColor = Node.Config.Console.CommandSuccessForegroundColor; break;
                        case LogCategory.Error: Console.ForegroundColor = Node.Config.Console.CommandErrorForegroundColor; break;

                        case LogCategory.OperationHeader: Console.ForegroundColor = Node.Config.Console.OperationHeaderForegroundColor; break;
                        case LogCategory.SuccessHeader: Console.ForegroundColor = Node.Config.Console.CommandSuccessHeaderForegroundColor; break;
                        case LogCategory.ErrorHeader: Console.ForegroundColor = Node.Config.Console.CommandErrorHeaderForegroundColor; break;

                        case LogCategory.ClientIn: Console.ForegroundColor = ConsoleColor.Magenta; break;
                        case LogCategory.ClientOut: Console.ForegroundColor = ConsoleColor.DarkMagenta; break;

                        case LogCategory.White: Console.ForegroundColor = ConsoleColor.White; break;
                        case LogCategory.Blue: Console.ForegroundColor = ConsoleColor.Blue; break;
                        case LogCategory.DarkBlue: Console.ForegroundColor = ConsoleColor.DarkBlue; break;

                        case LogCategory.Fatal: Console.ForegroundColor = ConsoleColor.Red; break;
                    }

                    //Console.WriteLine(log.Message);
                    //if (!LogCollection.IsCompleted)
                    //    continue;






                    //if (Node.WaitingForCommand)
                    //    Console.SetCursorPosition(left: 0, top: Console.CursorTop);

                    await WriteAsync(log);


                    //if (Node.WaitingForCommand && log.Message != $"{nameof(Cyxor)}>")
                    //{
                    //    Console.ForegroundColor = Node.Config.Console.CommandSuccessForegroundColor;
                    //    Console.Write($"{nameof(Cyxor)}>");
                    //}

                    //Console.ForegroundColor = Node.Config.Console.OperationHeaderForegroundColor;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                IdleTimerStop();
            }
            catch (Exception exc)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;

                Console.WriteLine($"{Environment.NewLine}Fatal exception: {exc.Message}");

                var c = 2;

                while (exc.InnerException != null)
                {
                    for (var i = 0; i < c; i++)
                        Console.Write(" ");

                    Console.WriteLine($"=> { exc.InnerException?.Message}");

                    c += 2;
                    exc = exc.InnerException;
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"{Environment.NewLine}{exc}");

                App.Terminate();
            }

            //return Utilities.Task.CompletedTask;
        }

        public Task StopAsync()
        {
            LogCollection.CompleteAdding();
            return Task;
        }

        async Task WriteAsync(MessageLoggedEventArgs log)
        //void WriteAsync(MessageLoggedEventArgs log)
        {
            var ch = '*';
            var il = log.IndentLevel;
            var currentColor = Console.ForegroundColor;

            if (log.Category == LogCategory.OperationHeader)
            {
                ch = '<';
                Console.ForegroundColor = Node.Config.Console.CommandSuccessForegroundColor;
            }
            else if (log.Category == LogCategory.ErrorHeader || log.Category == LogCategory.Success)
            {
                ch = '>';

                if (log.Category == LogCategory.Success)
                    Console.ForegroundColor = Node.Config.Console.OperationHeaderForegroundColor;
                else
                    Console.ForegroundColor = Node.Config.Console.CommandErrorHeaderForegroundColor;
            }
            else
                Console.ForegroundColor = ConsoleColor.DarkGreen;

            if (il != -1)
                for (var i = 0; i < il * Tab; i++)
                    Console.Write(il * Tab - i < il + 2 ? i == il * Tab - 1 ? ' ' : ch : ' ');

            Console.ForegroundColor = currentColor;

            if (!log.IsCommandResult)
            {
                //if (log.Exception == null)
                //    Console.Write(log.Message);
                //else
                if (log.Exception != null)
                    Console.Write(log.Exception == null ? log.Message : log.Exception.ToString());

                Console.Write(log.Exception == null ? log.Message : log.Exception.ToString());
            }
            else
            {
                var result = log.Result;

                do
                {
                    if (result.Code == ResultCode.Success)
                    {
                        Console.ForegroundColor = Node.Config.Console.CommandSuccessHeaderForegroundColor;
                        Console.Write($"{result.Code}: ");
                        Console.ForegroundColor = Node.Config.Console.CommandSuccessForegroundColor;
                        Console.Write(result.Comment);
                    }
                    else
                    {
                        Console.ForegroundColor = Node.Config.Console.CommandErrorHeaderForegroundColor;
                        Console.Write($"{result.Code}: ");
                        Console.ForegroundColor = Node.Config.Console.CommandErrorForegroundColor;
                        Console.Write(result.Comment);
                    }

                    var stringValue = result.GetModel<string>();

                    if (stringValue != null)
                    {
                        try
                        {
                            try
                            {
                                var obj = Utilities.Json.Deserialize(stringValue);

                                if (obj is string objString)
                                    await WriteBodyAsync(objString);
                                else
                                    await WriteJsonAsync(stringValue, obj);
                            }
                            catch
                            {
                                await WriteBodyAsync(stringValue);
                            }
                        }
                        catch { }
                    }

                    if (!(result = result.InnerResult))
                        Console.WriteLine();
                }
                while (!result);
            }

            if (il != -1)
                Console.WriteLine();
        }

        async Task WriteBodyAsync(string value)
        {
            var lines = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Console.ForegroundColor = ConsoleColor.DarkGreen;

            foreach (var line in lines)
            {
                Console.WriteLine();

                for (var i = 0; i < Tab; i++)
                    Console.Write(' ');

                Console.Write(line);

                await Utilities.Task.Delay(Node.Config.Console.OutputSpeedDelay);
            }
        }

        async Task WriteJsonAsync(string jsonValue, object obj)
        //void WriteJsonAsync(string jsonValue)
        {
            //var textData = false;

            var writingArray = false;
            var value = default(object);
            var token = default(JsonToken);
            var nextToken1 = default(JsonToken);
            var nextToken2 = default(JsonToken);
            var previousToken = default(JsonToken);
            var stringReader = new StringReader(jsonValue);
            var jsonReader = new JsonTextReader(stringReader);
            var jsonWriter = new JsonTextWriter(Console.Out) { Formatting = Formatting.Indented };

            async Task PerformDelay(bool checkWritingArray)
            {
                if (!checkWritingArray || (checkWritingArray && writingArray))
                    if (Node.Config.Console.OutputSpeedDelay != Timeout.Infinite)
                        await Utilities.Task.Delay(Node.Config.Console.OutputSpeedDelay);
            }

            var tokens = new List<JsonReaderToken>();

            while (jsonReader.Read())
                tokens.Add(new JsonReaderToken { Type = jsonReader.TokenType, Value = jsonReader.Value });

            Console.WriteLine();

            for (var i = 0; i < tokens.Count; i++)
            {
                previousToken = token;
                token = tokens[i].Type;
                value = tokens[i].Value;
                nextToken1 = tokens.Count > i + 1 ? tokens[i + 1].Type : JsonToken.None;
                nextToken2 = tokens.Count > i + 2 ? tokens[i + 2].Type : JsonToken.None;

                switch (token)
                {
                    case JsonToken.EndArray:
                    case JsonToken.EndConstructor:
                    case JsonToken.EndObject:
                    case JsonToken.StartArray:
                    case JsonToken.StartConstructor:
                    case JsonToken.StartObject: Console.ForegroundColor = ConsoleColor.White; break;

                    case JsonToken.Boolean: Console.ForegroundColor = ConsoleColor.Gray; break;
                    case JsonToken.Null: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                    case JsonToken.Undefined: Console.ForegroundColor = ConsoleColor.Red; break;
                    case JsonToken.String: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                    case JsonToken.Comment: Console.ForegroundColor = ConsoleColor.DarkGreen; break;

                    case JsonToken.PropertyName:

                        //if (value.ToString() == "Data")
                        //{
                        //    if (nextToken1 == JsonToken.String)
                        //        textData = true;
                        //}

                        if (nextToken1 == JsonToken.StartObject || (nextToken1 == JsonToken.Comment && nextToken2 == JsonToken.StartObject))
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        //else if (nextToken1 == JsonToken.StartArray || (nextToken1 == JsonToken.Comment && nextToken2 == JsonToken.StartArray))
                        //    Console.ForegroundColor = ConsoleColor.DarkBlue;
                        else
                            Console.ForegroundColor = ConsoleColor.DarkCyan;

                        break;

                    default: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                }

                writingArray = token == JsonToken.StartArray ? true :
                    token == JsonToken.EndArray ? false : writingArray;

                await PerformDelay(checkWritingArray: true);

                if (token == JsonToken.Comment)
                {
                    switch (previousToken)
                    {
                        case JsonToken.Date:
                        case JsonToken.Null:
                        case JsonToken.Bytes:
                        case JsonToken.Float:
                        case JsonToken.String:
                        case JsonToken.Integer:
                        case JsonToken.Boolean: jsonWriter.WriteWhitespace(" "); break;
                    }
                }

                await PerformDelay(checkWritingArray: true);

                jsonWriter.WriteToken(token, value);

                await PerformDelay(checkWritingArray: false);
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
