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

using Newtonsoft.Json;

namespace Cyxor.Networking
{
    using Config;

    public class ConfigQZBorrar
    {
        Node Node;
        const string InvalidArguments = "Invalid command line arguments.";
        const string NoCommandLineArguments = "No command line arguments were provided.";
        const string HelpOptionsMessage = "For configuration options use: cyxor -h|--help";
        const string ConfigurationMessage = "Usage: cyxor -c|--configuration [arg]";

        public Result Result { get; private set; }

        public ConfigQZBorrar(Node node)
        {
            Node = node;
        }

        public Result Process(string[] args)
        {
            Result = Result.Success;

            var help = false;
            var verbose = false;
            var configuration = false;

            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Configuring application...");

                if ((args?.Length ?? 0) == 0)
                {
                    Console.WriteLine(NoCommandLineArguments);
                    Console.WriteLine(HelpOptionsMessage);
                    Console.WriteLine();

                    ProcessConfigFile();

                    return Result;
                }

                var argsCount = 0;
                var sb = new StringBuilder();
                var jsonConfiguration = default(string);

                for (var i = 0; i < args.Length; i++)
                {
                    sb.Append($" {args[i]}");

                    if (!help && (help = $"--{nameof(help)}".Contains(args[i].ToLowerInvariant())))
                        argsCount++;
                    else if (!verbose && (verbose = $"--{nameof(verbose)}".Contains(args[i].ToLowerInvariant())))
                        argsCount++;
                    else if (!configuration && (configuration = $"--{nameof(configuration)}".Contains(args[i].ToLowerInvariant())))
                    {
                        if (i + 1 == args.Length)
                        {
                            Result = new Result(ResultCode.Error, InvalidArguments);
                            return Result;
                        }

                        i++;
                        argsCount += 2;
                        jsonConfiguration = args[i];
                        sb.Append($" {jsonConfiguration}");
                        Utilities.Json.Deserialize(jsonConfiguration, Node.Config.GetType());
                    }
                    else
                    {
                        Result = new Result(ResultCode.Error, InvalidArguments);
                        return Result;
                    }
                }

                Console.WriteLine($"Processing arguments:{sb.ToString()}");

                if (argsCount != args.Length)
                    Result = new Result(ResultCode.Error);
                else if (configuration)
                {
                    Node.Config.Load(jsonConfiguration);
                    ProcessConfigFile();
                    Node.Config.Load(jsonConfiguration);
                }
            }
            catch (Exception ex)
            {
                Result = new Result(ResultCode.Exception, exception: ex);
            }
            finally
            {
                if (!Result)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Result.Comment);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(HelpOptionsMessage);
                }
                else if (help)
                {
                    if (!verbose)
                        ShowHelp();
                    else
                        ShowVerboseHelp();

                    Result = new Result(ResultCode.Error, "Program executed in help mode.");
                }
            }

            return Result;
        }

        //void WriteName()
        //{
        //    var name = Node.Config.Name ?? "UnnamedApp";

        //    Console.Title = $"Cyxor - {name}";

        //    var configName = $"  {name}";
        //    var coreTypeName = $" [{nameof(Cyxor)} {Node.CoreTypeName}] ";
        //    var versionValue = $"v{Version.Value}  ";

        //    var charCount = configName.Length + coreTypeName.Length + versionValue.Length;

        //    Console.ForegroundColor = ConsoleColor.Green;

        //    for (int i = 0; i < charCount; i++)
        //        Console.Write('=');

        //    Console.WriteLine();

        //    Console.Write(configName);
        //    Console.ForegroundColor = ConsoleColor.DarkGreen;
        //    Console.Write(coreTypeName);
        //    Console.ForegroundColor = ConsoleColor.Green;
        //    Console.WriteLine(versionValue);

        //    Console.ForegroundColor = ConsoleColor.Green;

        //    for (int i = 0; i < charCount; i++)
        //        Console.Write('=');

        //    Console.WriteLine();
        //}

        void ProcessConfigFile()
        {
            try
            {
                switch (Node.Config.File.Mode)
                {
                    case FileConfigMode.None: break;
                    case FileConfigMode.Create: Node.Config.Save(); break;
                    case FileConfigMode.Open:
                    {
                        if (File.Exists(Node.Config.File.Name))
                            Node.Config.Load();
                        else
                            Result = new Result(ResultCode.Error, $"The specified config file doesn't exists. FileMode = {Node.Config.File.Mode}, FileName = '{Node.Config.File.Name}'.");

                        break;
                    }
                    case FileConfigMode.OpenOrCreate:
                    {
                        if (File.Exists(Node.Config.File.Name))
                            Node.Config.Load();
                        else
                            Node.Config.Save();

                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.ToString());
            }
        }

        void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine("Starts a Cyxor Node.");
            Console.WriteLine();
            Console.WriteLine("Usage: cyxor [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -h|--help                             Show help information");
            Console.WriteLine("  -v|--verbose                          Enable verbose output");
            Console.WriteLine("  -i|--information                      Display Cyxor core information");
            Console.WriteLine("  -c|--configuration <CONFIGURATION>    Json string with configuration options");
            Console.WriteLine();
        }

        void ShowVerboseHelp()
        {
            ShowHelp();

            var jsonConfig = Node.Config.SerializeToJson();
            var stringReader = new StringReader(jsonConfig);
            var jsonReader = new JsonTextReader(stringReader);

            var jsonWriter = new JsonTextWriter(Console.Out);
            jsonWriter.Formatting = Formatting.Indented;

            var previousToken = default(JsonToken);

            while (jsonReader.Read())
            {
                switch (jsonReader.TokenType)
                {
                    case JsonToken.EndArray:
                    case JsonToken.EndConstructor:
                    case JsonToken.EndObject:
                    case JsonToken.StartArray:
                    case JsonToken.StartConstructor:
                    case JsonToken.StartObject: Console.ForegroundColor = ConsoleColor.White; break;

                    case JsonToken.Boolean: Console.ForegroundColor = ConsoleColor.Cyan; break;
                    case JsonToken.Null: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                    case JsonToken.Undefined: Console.ForegroundColor = ConsoleColor.Red; break;
                    case JsonToken.String: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                    case JsonToken.Comment: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                    case JsonToken.PropertyName: Console.ForegroundColor = ConsoleColor.DarkCyan; break;

                    default: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                }

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

                jsonWriter.WriteToken(jsonReader.TokenType, jsonReader.Value);
                previousToken = jsonReader.TokenType;
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
