/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Author: Yandy Zaldivar
*/

using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config.Server
{
    using Serialization;

    [Description("TODO:")]
    public class DatabaseEngineConfig : ConfigProperty
    {
        //public const string DefaultDatabase = null;
        //string database = DefaultDatabase;
        //[DefaultValue(DefaultDatabase)]
        //[Description("TODO:")]
        //public string Database
        //{
        //    get { return database; }
        //    set { SetProperty(ref database, value, GetType()); }
        //}

        //public const int DefaultWaitForStartMillisecondsDelay = 1000 * 1;
        //int waitForStartMillisecondsDelay = DefaultWaitForStartMillisecondsDelay;
        //[DefaultValue(DefaultWaitForStartMillisecondsDelay)]
        //[Description("TODO:")]
        //public int WaitForStartMillisecondsDelay
        //{
        //    get => waitForStartMillisecondsDelay;
        //    set => SetProperty(ref waitForStartMillisecondsDelay, value);
        //}

        //public const string DefaultRelativeConfigFile = "my.ini";
        //string relativeConfigFile = DefaultRelativeConfigFile;
        //[DefaultValue(DefaultRelativeConfigFile)]
        //[Description("TODO:")]
        //public string RelativeConfigFile
        //{
        //    get => relativeConfigFile;
        //    set => SetProperty(ref relativeConfigFile, value);
        //}

        //[JsonIgnore]
        //public string ConfigFile => Path.Combine(BaseDir, RelativeConfigFile);

        //public const string DefaultRelativeProcessLogFile = "process.log";
        //string relativeProcessLogFile = DefaultRelativeProcessLogFile;
        //[DefaultValue(DefaultRelativeProcessLogFile)]
        //[Description("TODO:")]
        //public string RelativeProcessLogFile
        //{
        //    get => relativeProcessLogFile;
        //    set => SetProperty(ref relativeProcessLogFile, value);
        //}

        //[JsonIgnore]
        //public string ProcessLogFile => Path.Combine(DataDir, RelativeProcessLogFile);

        //public string GetConfigFileText()
        //{
        //    var text = new StringBuilder();

        //    foreach (var line in ConfigFileLines)
        //        text.AppendLine(line);

        //    return text.ToString();
        //}

        //public string Arguments
        //{
        //    get
        //    {
        //        var options = @"
        //            --no-defaults
        //            --basedir=mysql
        //            --datadir=data
        //            --ssl=false
        //            --port=12484
        //            --log-syslog=false
        //            --bind-address=::1
        //            --pid-file=process.pid
        //            --secure-file-priv=null
        //            --log-error-verbosity=3
        //            --lower-case-table-names=2
        //            --max-allowed-packet=4194304
        //            --character-set-server=utf8mb4
        //            --explicit-defaults-for-timestamp
        //            --innodb-buffer-pool-size=536870912
        //            --collation-server=utf8mb4_general_ci";

        //        var tokens = options.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        //        var args = new StringBuilder();

        //        foreach (var token in tokens)
        //            args.Append($"{token.Trim()} ");

        //        return args.ToString().Trim();
        //    }
        //}

        internal bool FirstRun { get; set; }

        public DatabaseEngineConfig()
        {
            //if (Networking.Utilities.Platform.OS == Networking.Utilities.Platform.PlatformOS.Linux)
            //    relativeServerFile += Networking.Utilities.Platform.PlatformOS.Linux.ToString();
            //else if (Networking.Utilities.Platform.OS == Networking.Utilities.Platform.PlatformOS.MacOSX)
            //    relativeServerFile += Networking.Utilities.Platform.PlatformOS.MacOSX.ToString();
            //else if (Networking.Utilities.Platform.OS == Networking.Utilities.Platform.PlatformOS.Windows)
            //    relativeServerFile += Networking.Utilities.Platform.PlatformOS.Windows.ToString();

            //if (Networking.Utilities.Platform.OS == Networking.Utilities.Platform.PlatformOS.Linux)
            //    relativeServerFile = "bin/Linux/mysqld";
            //else if (Networking.Utilities.Platform.OS == Networking.Utilities.Platform.PlatformOS.MacOSX)
            //    relativeServerFile = "bin/MacOS/mysqld";
            //else if (Networking.Utilities.Platform.OS == Networking.Utilities.Platform.PlatformOS.Windows)
            //    relativeServerFile = "bin/Windows/mysqld";
        }

        public string GetConnectionString(string database) => GetConnectionString(excludePassword: false, database: database);
        public string GetConnectionString(bool excludePassword) => GetConnectionString(excludePassword, database: null);
        public string GetConnectionString() => GetConnectionString(excludePassword: false, database: null);

        public string GetConnectionString(bool excludePassword, string database)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
                return ConnectionString;

            try
            {
                var connectionString = $"{nameof(server)}={Server};{nameof(port)}={Port};{nameof(user)}={User}";

                if (!excludePassword)
                    connectionString += $";{nameof(password)}={Password}";

                if (database != null)
                    connectionString += $";{nameof(database)}={database}";

                return connectionString;
            }
            catch (Exception exc)
            {
                Node.Log(LogCategory.Warning, "Automatic connection string generation failed, try to specify the connection string explicitly.");
                Node.Log(LogCategory.Error, exception: exc);
                return ConnectionString;
            }
        }

        public const bool DefaultEnabled = false;
        bool enabled = DefaultEnabled;
        [DefaultValue(DefaultEnabled)]
        [Description("TODO:")]
        public bool Enabled
        {
            get => enabled;
            set => SetProperty(ref enabled, value);
        }

        public const ProcessPriorityClass DefaultProcessPriorityClass = ProcessPriorityClass.Normal;
        ProcessPriorityClass processPriorityClass = DefaultProcessPriorityClass;
        [DefaultValue(DefaultProcessPriorityClass)]
        [Description("TODO:")]
        public ProcessPriorityClass ProcessPriorityClass
        {
            get => processPriorityClass;
            set => SetProperty(ref processPriorityClass, value);
        }

        public const DatabaseEngineProvider DefaultProvider = DatabaseEngineProvider.MySql;
        DatabaseEngineProvider provider = DefaultProvider;
        [DefaultValue(DefaultProvider)]
        [Description("TODO:")]
        public DatabaseEngineProvider Provider
        {
            get => provider;
            set => SetProperty(ref provider, value);
        }

        public const string DefaultConnectionString = null;
        string connectionString = DefaultConnectionString;
        [DefaultValue(DefaultConnectionString)]
        [Description("TODO:")]
        public string ConnectionString
        {
            get => connectionString;
            set => SetProperty(ref connectionString, value);
        }

        public const int DefaultWaitForStartMilliseconds = 1000 * 64;
        int waitForStartMilliseconds = DefaultWaitForStartMilliseconds;
        [DefaultValue(DefaultWaitForStartMilliseconds)]
        [Description("TODO:")]
        public int WaitForStartMilliseconds
        {
            get => waitForStartMilliseconds;
            set => SetProperty(ref waitForStartMilliseconds, value);
        }

        public const string DefaultServer = "localhost";
        string server = DefaultServer;
        [DefaultValue(DefaultServer)]
        [Description("TODO:")]
        public string Server
        {
            get => server;
            set => SetProperty(ref server, value);
        }

        public const int DefaultPort = 12484;
        int port = DefaultPort;
        [DefaultValue(DefaultPort)]
        [Description("TODO:")]
        public int Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }

        public const string DefaultUser = "root";
        string user = DefaultUser;
        [DefaultValue(DefaultUser)]
        [Description("TODO:")]
        public string User
        {
            get => user;
            set => SetProperty(ref user, value);
        }

        public const string DefaultPassword = "cyxor";
        string password = DefaultPassword;
        [DefaultValue(DefaultPassword)]
        [Description("TODO:")]
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public const int DefaultProcessId = -1;
        int processId = DefaultProcessId;
        [DefaultValue(DefaultProcessId)]
        [Description("TODO:")]
        public int ProcessId
        {
            get => processId;
            set => SetProperty(ref processId, value);
        }

        public const string DefaultBaseDir = "mysql";
        string baseDir = DefaultBaseDir;
        [DefaultValue(DefaultBaseDir)]
        [Description("TODO:")]
        public string BaseDir
        {
            get => baseDir;
            set => SetProperty(ref baseDir, value);
        }

        public const string DefaultRelativeDataDir = "data";
        string relativeDataDir = DefaultRelativeDataDir;
        [DefaultValue(DefaultRelativeDataDir)]
        [Description("TODO:")]
        public string RelativeDataDir
        {
            get => relativeDataDir;
            set => SetProperty(ref relativeDataDir, value);
        }

        [JsonIgnore]
        public string DataDir => Path.Combine(BaseDir, RelativeDataDir);

        public const string DefaultArguments =// typeof(DatabaseEngineConfig).GetProperty(nameof(Arguments)).GetCustomAttribute<DefaultValueAttribute>().Value as string[];
            "--no-defaults " +
            "--user=root " +
            "--ssl=false " +
            "--port=12484 " +
            "--basedir=mysql " +
            "--datadir=data " +
            "--log-syslog=false " +
            //"--bind-address=::1 " +
            "--bind-address=localhost " +
            "--pid-file=process.pid " +
            "--secure-file-priv=null " +
            "--log-error-verbosity=3 " +
            "--lower-case-table-names=2 " +
            "--max-allowed-packet=4194304 " +
            "--character-set-server=utf8mb4 " +
            "--explicit-defaults-for-timestamp " +
            "--innodb-buffer-pool-size=536870912 " +
            "--collation-server=utf8mb4_general_ci";
        string arguments = DefaultArguments;
        [DefaultValue(DefaultArguments)]
        [Description("TODO:")]
        public string Arguments
        {
            get => arguments;
            set => SetProperty(ref arguments, value);
        }

        public const string DefaultRelativeProcessPidFile = "process.pid";
        string relativeProcessPidFile = DefaultRelativeProcessPidFile;
        [DefaultValue(DefaultRelativeProcessPidFile)]
        [Description("TODO:")]
        public string RelativeProcessPidFile
        {
            get => relativeProcessPidFile;
            set => SetProperty(ref relativeProcessPidFile, value);
        }

        [JsonIgnore]
        public string ProcessPidFile => Path.Combine(DataDir, RelativeProcessPidFile);

        public const string DefaultRelativeServerFile = "bin/mysqld";
        string relativeServerFile = DefaultRelativeServerFile;
        [DefaultValue(DefaultRelativeServerFile)]
        [Description("TODO:")]
        public string RelativeServerFile
        {
            get => relativeServerFile;
            set => SetProperty(ref relativeServerFile, value);
        }

        [JsonIgnore]
        public string ServerFile => Path.Combine(BaseDir, RelativeServerFile);

        public const string DefaultRelativeServerDumpFile = "bin/mysqlpump";
        string relativeServerDumpFile = DefaultRelativeServerDumpFile;
        [DefaultValue(DefaultRelativeServerDumpFile)]
        [Description("TODO:")]
        public string RelativeServerDumpFile
        {
            get => relativeServerDumpFile;
            set => SetProperty(ref relativeServerDumpFile, value);
        }

        [JsonIgnore]
        public string ServerDumpFile => Path.Combine(BaseDir, RelativeServerDumpFile);
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
