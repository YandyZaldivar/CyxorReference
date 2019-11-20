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

namespace Cyxor.Networking.Events
{
    using Config;
    using Extensions;

    public sealed class MessageLoggedEventArgs : ActionEventArgs
    {
        public override int EventId => Node.NodeEventsId.MessageLogged;

        readonly string MessageFormat;

        public int TaskId { get; }
        public string Trace { get; }
        public Result Result { get; }
        public string Message { get; }
        public int IndentLevel { get; }
        public DateTime LogTime { get; }
        public Exception Exception { get; }
        public LogCategory Category { get; }
        public bool IsCommandResult { get; }
        public bool ContainsJsonMessage { get; }
        public bool FromSynchronizationContextThread { get; }

        internal MessageLoggedEventArgs(Node node, string message, params object[] args)
           : this(node, LogCategory.Message, 0, default(Exception), message, args)
        { }

        internal MessageLoggedEventArgs(Node node, Exception exception)
           : this(node, LogCategory.Fatal, 0, exception, message: null, args: null)
        { }

        internal MessageLoggedEventArgs(Node node, LogCategory category, Exception exception)
           : this(node, category, 0, exception, message: null, args: null)
        { }

        internal MessageLoggedEventArgs(Node node, LogCategory category, int indentLevel, Exception exception)
           : this(node, category, indentLevel, exception, message: null, args: null)
        { }

        internal MessageLoggedEventArgs(Node node, LogCategory category, string message, params object[] args)
           : this(node, category, 0, default(Exception), message, args)
        { }

        internal MessageLoggedEventArgs(Node node, LogCategory category, int indentLevel, string message, params object[] args)
           : this(node, category, indentLevel, default(Exception), message, args)
        { }

        internal MessageLoggedEventArgs(Node node, Result result)
           : this(node, result, isCommandResult: false)
        { }

        internal MessageLoggedEventArgs(Node node, Result result, bool isCommandResult)
           : this(node, result.Exception != null ? LogCategory.Fatal : result.Code != ResultCode.Success ? LogCategory.Error : LogCategory.Success, 0, result.Exception, result.ToString(), args: null)
        {
            Result = result;
            IsCommandResult = isCommandResult;
        }

        internal MessageLoggedEventArgs(Node node, LogCategory category, int indentLevel, Exception exception, string message, params object[] args)
           : base(node)
        {
            Category = category;
            Exception = exception;
            LogTime = DateTime.Now;
            IndentLevel = indentLevel;
            TaskId = Task.CurrentId != null ? (int)Task.CurrentId : -1;

            if (exception == null && Category == LogCategory.Fatal)
                Trace = Environment.StackTrace;

            if (node.Config.EventDispatching == EventDispatching.Synchronized)
                if (node.Config.SynchronizationContext != null)
                    FromSynchronizationContextThread = node.Config.SynchronizationContextManagedThreadId == Thread.CurrentThread.ManagedThreadId;

            if (args == null || args.Length == 0)
                Message = message;
            else if (message != null)
                Message = string.Format(message, args);

            if (Message == null && Exception != null)
                Message = Exception.GetType().Name + ".";

            MessageFormat = string.Format("[{0}] {2}: {3}", LogTime.ToString("HH:mm:ss"), TaskId, category, Message);

            if (Exception != null)
                //MessageFormat = string.Format(MessageFormat + "{0}Exception: {1}", Environment.NewLine, Exception.ToString());
                MessageFormat = $"{MessageFormat}{Environment.NewLine}Exception: {Exception.ToString()}";
            else if (!string.IsNullOrEmpty(Trace))
                //MessageFormat = string.Format(MessageFormat + "{0}StackTrace: {1}", Environment.NewLine, Trace);
                MessageFormat = $"{MessageFormat}{Environment.NewLine}StackTrace: {Trace}";
        }

        public override string ToString() => MessageFormat;

        internal override void Action()
        {
            if (Node.Config.FileLog.Enabled)
                if (Node.Config.FileLog.TextWriter != null)
                    Node.Config.FileLog.TextWriter.WriteLineAsync(ToString());

            base.Action();

            //Node.DisconnectAsync(MessageFormat, ShutdownSequence.Abortive);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
