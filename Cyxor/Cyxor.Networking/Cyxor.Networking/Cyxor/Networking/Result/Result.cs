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
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Cyxor.Networking
{
    using Serialization;

    /// <summary>
    /// This struct is inmutable.
    /// </summary>
    /// <remarks>
    /// The <see cref="InnerResult"/> is only valid if it is not a <see cref="ResultCode.Success"/>.
    /// That is, in a nested hierarchy of results, an <see cref="InnerResult"/> with a code of 
    /// <see cref="ResultCode.Success"/> marks the end of the chain. As you can see, while a single
    /// <see cref="Result"/> can denote a <see cref="ResultCode.Success"/>, further nested results
    /// represent error codes. A similar mechanism to <see cref="Exception.InnerException"/>.
    /// As well, a successful <see cref="Result"/> can't contain an <see cref="InnerResult"/>.
    /// </remarks>
    public struct Result : ISerializable, IComparable, IComparable<Result>, IEquatable<Result>
    {
        public static readonly Result Success = default;
        public static readonly Task<Result> TaskSuccess = Utilities.Task.FromResult(Success);

        public ResultCode Code { get; private set; }

        string comment;
        public string Comment
        {
            get => comment ?? Exception?.Message ?? Comments[Code];
            private set => comment = value;
        }

        [CyxorIgnore]
        [JsonProperty]
        public object Tag { get; set; }

        [JsonIgnore]
        Serializer Data { get; set; }

        [CyxorIgnore]
        [JsonProperty]
        byte[] Buffer
        {
            get => Data?.DeserializeBytes();
            set => Data = new Serializer(value);
        }

        //public object GetModel(Type type) => Data?.DeserializeRawObject(type);
        public object GetModel(Type type) => Data?.ToObject(type);
        public T GetModel<T>(T value) => Data != null ? Data.ToObject(value) : default;
        public T GetModel<T>(IBackingSerializer serializer) => Data != null ? Data.ToObject<T>(serializer) : default;
        public T GetModel<T>(T value, IBackingSerializer serializer) => Data != null ? Data.ToObject(value, serializer) : default;
        public T GetModel<T>() => Data != null ? typeof(T) == typeof(string) ? Data.ToObject(default(T)) : Data.ToObject<T>() : default;
        //public T GetModel<T>() => Data.ToObject(default(T));

        public void PopulateObject<T>(T value)
        {
            if (Data != null)
                Data.ToObject(value);
        }

        void SetModel(object model, IBackingSerializer backingSerializer)
            => Data = model != null ? new Serializer(model, backingSerializer) : null;

        [JsonIgnore]
        public string Description
        {
            get
            {
                try
                {
                    if (Code == ResultCode.SocketError)
                        return SocketErrors[SocketError];

                    return Exception?.ToString() ?? Data?.ToString();
                }
                catch
                {
                    return null;
                }
            }
        }

        [JsonIgnore]
        public Exception Exception { get; }

        [JsonIgnore]
        public SocketError SocketError
        {
            get
            {
                var networkError = default(SocketError);

                if (Code == ResultCode.SocketError)
                {
                    try { networkError = (SocketError)Enum.Parse(typeof(SocketError), Comment); }
                    catch { networkError = default; }
                }

                return networkError;
            }
        }

        [JsonIgnore]
        public string SocketErrorDescription => SocketErrors[SocketError];

        InnerResult innerResult;
        [JsonIgnore]
        public Result InnerResult
        {
            get => innerResult?.Result ?? Exception?.InnerException != null ? new Result(ResultCode.Exception, exception: Exception.InnerException) : Success;
            private set
            {
                if (value == Success)
                    throw new ArgumentException("The value must have a non success code.");

                if (innerResult != null)
                    throw new InvalidOperationException("InnerResult must be null before assigning a value to it.");

                innerResult = new InnerResult(value);
            }
        }

        /// <summary>
        /// Allows you to correctly combine two results independently of their <see cref="Result.Code"/>.
        /// </summary>
        /// <param name="result1">The first <see cref="Result"/>.</param>
        /// <param name="result2">The second <see cref="Result"/>.</param>
        /// <returns></returns>
        public static Result Combine(Result result1, Result result2)
        {
            if (result2 == Success)
                return result1;
            else if (result1 == Success)
                return result2;
            else return new Result(result1, result2);
        }

        public Result(Result result, Result innerResult) :
            this(result.Code, result.Comment, result.Exception, result.InnerResult, result.Data)
        {
            if (result || innerResult)
                throw new ArgumentException();

            var ir = this.innerResult;

            if (ir == null)
                InnerResult = innerResult;
            else
            {
                while (ir.Result.innerResult != null)
                    ir = ir.Result.innerResult;

                ir.Result.InnerResult = innerResult;
            }
        }

        public Result(ResultCode code = ResultCode.Success, string comment = null, Exception exception = null, Result innerResult = default(Result), object model = null, IBackingSerializer backingSerializer = null) : this()
        {
            Code = code;
            Comment = comment;
            Exception = exception;
            this.innerResult = null;
            SetModel(model, backingSerializer);

            if (innerResult != Success && innerResult.Code == ResultCode.Success)
                throw new ArgumentException("When passing an innerResult its code must be different than success.");
            else if (innerResult != Success)
                InnerResult = innerResult;
        }

        public Result(Result result, ResultCode code = ResultCode.Success, string comment = null, Exception exception = null, Result innerResult = default(Result), object model = null, IBackingSerializer backingSerializer = null)
            : this(result.Code, result.Comment, result.Exception, result.InnerResult, result.Data)
        {
            if (code != ResultCode.Success)
                Code = code;

            if (model != null)
                SetModel(model, backingSerializer);

            if (comment != null)
                Comment = comment;

            if (exception != null)
                Exception = exception;

            if (innerResult != default)
            {
                this.innerResult = null;

                if (innerResult != Success && innerResult.Code == ResultCode.Success)
                    throw new ArgumentException("When passing an innerResult its code must be different than success.");
                else if (innerResult != Success)
                    InnerResult = innerResult;
            }
        }

        public static implicit operator bool (Result result) => result.Code == ResultCode.Success;
        public static implicit operator ResultCode(Result result) => result.Code;

        public static bool operator ==(Result result, Result other) => result.Equals(other);
        public static bool operator !=(Result result, Result other) => !result.Equals(other);

        public static implicit operator Result(bool result) => new Result(!result ? ResultCode.Error : ResultCode.Success);

        public override string ToString()
        {
            var description = Description;

            if (string.IsNullOrEmpty(Comment) && string.IsNullOrEmpty(description))
                return Code.ToString();
            else if (string.IsNullOrEmpty(description))
                return $"{Code}: {Comment}";
            else
                return $"{Code}: {Comment} =>{Environment.NewLine}{description}";
        }

        public string ToJson()
        {
            var data = default(object);
            var description = Description;

            try
            {
                if (description == null && (Data?.Int32Length ?? 0) > 0)
                    description = JsonConvert.SerializeObject(Data.ToByteArray());

                if (description != null)
                    data = JsonConvert.DeserializeObject(description);
            }
            catch { }

            return JsonConvert.SerializeObject(new
            {
                Code,
                Comment,
                Data = data,
            }, Formatting.Indented);
        }

        public string ToXml()
            => JsonConvert.SerializeXNode(JsonConvert.DeserializeXNode(ToJson()));

        public override int GetHashCode() => Code.GetHashCode();

        public void Serialize(Serializer serializer)
        {
            serializer.Serialize(Code);
            serializer.Serialize(Comment);
            serializer.Serialize(Data);
        }

        public void Deserialize(Serializer serializer)
        {
            Code = serializer.DeserializeEnum<ResultCode>();
            Comment = serializer.DeserializeString();
            Data = serializer.DeserializeSerializer();
        }

        public Result LightCopy() => new Result(Code, Comment);

        public bool Equals(Result other) => CompareTo(other) == 0;

        public override bool Equals(object other) => CompareTo(other) == 0;

        public int CompareTo(object other)
        {
            if (other == null)
                return 1;

            if (!(other is Result))
                return -1;

            return CompareTo((Result)other);
        }

        public int CompareTo(Result other)
        {
            var compareResult = Code.CompareTo(other.Code);

            if (compareResult == 0)
            {
                compareResult = string.Compare(Comment, other.Comment);

                if (compareResult == 0)
                {
                    // TODO: Decide what to do with data buffer and inner result.
                    //Equals(Data.Buffer, other.Data.Buffer);
                }
            }

            return compareResult;
        }

        static readonly SortedDictionary<ResultCode, string> Comments = new SortedDictionary<ResultCode, string>()
        {
            [ResultCode.Error] = default,
            [ResultCode.Success] = default,
            [ResultCode.SocketError] = default,

            [ResultCode.Exception] = default,
            [ResultCode.ExceptionInLogEvent] = default,
            [ResultCode.ExceptionInUserCode] = default,

            [ResultCode.AccountAlreadyDeleted] = ResultComment.AccountAlreadyDeleted,
            [ResultCode.AccountCreationCanceled] = ResultComment.AccountCreationCanceled,
            [ResultCode.AccountDeletionCanceled] = ResultComment.AccountDeletionCanceled,
            [ResultCode.AccountEmailTaken] = ResultComment.AccountEmailTaken,
            [ResultCode.AccountHashTaken] = ResultComment.AccountHashTaken,
            [ResultCode.AccountNameTaken] = ResultComment.AccountNameTaken,
            [ResultCode.AccountNotExist] = ResultComment.AccountNotExist,
            [ResultCode.AccountResetCanceled] = ResultComment.AccountResetCanceled,
            [ResultCode.AccountResetRefused] = ResultComment.AccountResetRefused,
            [ResultCode.AccountSecurityOutOfRange] = ResultComment.AccountSecurityOutOfRange,

            [ResultCode.OperationCanceled] = ResultComment.AsyncOperationCanceled,
            [ResultCode.OperationFaulted] = ResultComment.AsyncOperationFaulted,
            [ResultCode.OperationTimedOut] = ResultComment.AsyncOperationTimedOut,
            [ResultCode.OperationInProgress] = ResultComment.AsyncOperationInProgress,

            [ResultCode.AuthenticationFailed] = ResultComment.AuthenticationFailed,

            [ResultCode.ClientConnectionCanceled] = ResultComment.ClientConnectionCanceled,
            [ResultCode.ClientDisconnectionError] = ResultComment.ClientDisconnectionError,
            [ResultCode.ClientDisconnectionLocal] = ResultComment.ClientDisconnectionLocal,
            [ResultCode.ClientDisconnectionNone] = ResultComment.ClientDisconnectionNone,

            [ResultCode.ClientNotConnected] = ResultComment.ClientNotConnected,

            [ResultCode.ClientDisconnectionRemote] = ResultComment.ClientDisconnectionRemote,
            [ResultCode.ClientLoginTimeout] = ResultComment.ClientLoginTimeout,
            [ResultCode.ClientReconnected] = ResultComment.ClientReconnected,
            [ResultCode.ClientUpdateRequestCanceled] = ResultComment.ClientUpdateRequestCanceled,

            [ResultCode.CommandArgumentsMismatch] = ResultComment.CommandArgumentsMismatch,
            [ResultCode.CommandCanceled] = ResultComment.CommandCanceled,
            [ResultCode.CommandParseError] = ResultComment.CommandParseError,
            [ResultCode.CommandSecurityViolation] = ResultComment.CommandSecurityViolation,
            [ResultCode.CommandSyntaxError] = ResultComment.CommandSyntaxError,

            [ResultCode.ConnectionAlreadyTerminated] = ResultComment.ConnectionAlreadyTerminated,

            [ResultCode.EmailInvalidFormat] = ResultComment.EmailInvalidFormat,
            [ResultCode.EmailNullOrEmpty] = ResultComment.EmailNullOrEmpty,

            [ResultCode.FileTransferRequestCanceled] = ResultComment.FileTransferRequestCanceled,

            [ResultCode.NameMaxLengthViolation] = ResultComment.NameMaxLengthViolation,
            [ResultCode.NameMinLengthViolation] = ResultComment.NameMinLengthViolation,
            [ResultCode.NameNullOrEmpty] = ResultComment.NameNullOrEmpty,
            [ResultCode.NameRegexViolation] = ResultComment.NameRegexViolation,
            [ResultCode.NameUnavailable] = ResultComment.NameUnavailable,

            [ResultCode.NetworkAddressInvalid] = ResultComment.NetworkAddressInvalid,
            [ResultCode.NetworkIPUnsupported] = ResultComment.NetworkIPUnsupported,
            [ResultCode.NetworkPortOutOfRange] = ResultComment.NetworkPortOutOfRange,
            [ResultCode.NetworkUnavailable] = ResultComment.NetworkUnavailable,

            [ResultCode.NodeConnectionFailed] = ResultComment.NodeConnectionFailed,
            [ResultCode.NodeDisconnectionCanceled] = ResultComment.NodeDisconnectionCanceled,
            [ResultCode.NodeDisconnectionFailed] = ResultComment.NodeDisconnectionFailed,
            [ResultCode.NodeNotConnected] = ResultComment.NodeNotConnected,

            [ResultCode.PacketQueryNotSupported] = ResultComment.PacketQueryNotSupported,
            [ResultCode.PacketSendingInProgress] = ResultComment.PacketSendingInProgress,
            [ResultCode.PacketSendMultiConnectionsErrors] = ResultComment.PacketSendMultiConnectionsErrors,

            [ResultCode.ProtocolError] = ResultComment.ProtocolError,
            [ResultCode.ProtocolErrorCrypto] = ResultComment.ProtocolErrorCrypto,
            [ResultCode.ProtocolErrorData] = ResultComment.ProtocolErrorData,
            [ResultCode.ProtocolErrorSize] = ResultComment.ProtocolErrorSize,
            [ResultCode.ProtocolErrorType] = ResultComment.ProtocolErrorType,

            [ResultCode.ServerDatabaseDisabled] = ResultComment.ServerDatabaseDisabled,
            [ResultCode.ServerShutdown] = ResultComment.ServerShutdown,

            [ResultCode.SRPClientNullOrEmptyA] = ResultComment.SRPClientNullOrEmptyA,
            [ResultCode.Srp_M_Mismatch] = ResultComment.Srp_M_Mismatch,

            [ResultCode.SynchronizationContextNull] = ResultComment.SynchronizationContextNull,

            [ResultCode.UpdateCompleted] = ResultComment.UpdateCompleted,
            [ResultCode.UpdateFailed] = ResultComment.UpdateFailed,

            [ResultCode.BadRequest] = ResultComment.BadRequest,
            [ResultCode.NotFound] = ResultComment.NotFound,
        };

        static readonly SortedDictionary<SocketError, string> SocketErrors = new SortedDictionary<SocketError, string>()
        {
            [SocketError.SocketError] = "An unspecified Socket error has occurred.",
            [SocketError.Success] = "The Socket operation succeeded.",
            [SocketError.OperationAborted] = "The overlapped operation was aborted due to the closure of the Socket.",
            [SocketError.IOPending] = "The application has initiated an overlapped operation that cannot be completed immediately.",
            [SocketError.Interrupted] = "A blocking System.Net.Sockets.Socket call was canceled.",
            [SocketError.AccessDenied] = "An attempt was made to access Socket in a way that is forbidden by its access permissions.",
            [SocketError.Fault] = "An invalid pointer address was detected by the underlying socket provider.",
            [SocketError.InvalidArgument] = "An invalid argument was supplied to a Socket member.",
            [SocketError.TooManyOpenSockets] = "There are too many open sockets in the underlying socket provider",
            [SocketError.WouldBlock] = "An operation on a nonblocking socket cannot be completed immediately.",
            [SocketError.InProgress] = "A blocking operation is in progress.",
            [SocketError.AlreadyInProgress] = "The nonblocking Socket already has an operation in progress.",
            [SocketError.NotSocket] = "Socket operation was attempted on a non-socket.",
            [SocketError.DestinationAddressRequired] = "A required address was omitted from an operation on a System.Net.Sockets.Socket.",
            [SocketError.MessageSize] = "The datagram is too long.",
            [SocketError.ProtocolType] = "The protocol type is incorrect for this Socket.",
            [SocketError.ProtocolOption] = "An unknown, invalid, or unsupported option or level was used with a Socket.",
            [SocketError.ProtocolNotSupported] = "The protocol is not implemented or has not been configured.",
            [SocketError.SocketNotSupported] = "The support for the specified socket type does not exist in this address family.",
            [SocketError.OperationNotSupported] = "The address family is not supported by the protocol family.",
            [SocketError.ProtocolFamilyNotSupported] = "The protocol family is not implemented or has not been configured.",
            [SocketError.AddressFamilyNotSupported] = "The address family specified is not supported. This error is returned " +
                "if the IPv6 address family was specified and the IPv6 stack is not installed on the local machine. This error " +
                "is returned if the IPv4 address family was specified and the IPv4 stack is not installed on the local machine.",
            [SocketError.AddressAlreadyInUse] = "Only one use of an address is normally permitted.",
            [SocketError.AddressNotAvailable] = "The selected IP address is not valid in this context.",
            [SocketError.NetworkDown] = "The network is not available.",
            [SocketError.NetworkUnreachable] = "No route to the remote host exists.",
            [SocketError.NetworkReset] = "The application tried to set System.Net.Sockets.SocketOptionName.KeepAlive on a connection that has already timed out.",
            [SocketError.ConnectionAborted] = "The connection was aborted by the .NET Framework or the underlying socket provider.",
            [SocketError.ConnectionReset] = "The connection was reset by the remote peer.",
            [SocketError.NoBufferSpaceAvailable] = "No free buffer space is available for a System.Net.Sockets.Socket operation.",
            [SocketError.IsConnected] = "The System.Net.Sockets.Socket is already connected.",
            [SocketError.NotConnected] = "The application tried to send or receive data, and the System.Net.Sockets.Socket is not connected.",
            [SocketError.Shutdown] = "A request to send or receive data was disallowed because the System.Net.Sockets.Socket has already been closed.",
            [SocketError.TimedOut] = "The connection attempt timed out, or the connected host has failed to respond.",
            [SocketError.ConnectionRefused] = "The remote host is actively refusing a connection.",
            [SocketError.HostDown] = "The operation failed because the remote host is down.",
            [SocketError.HostUnreachable] = "There is no network route to the specified host.",
            [SocketError.ProcessLimit] = "Too many processes are using the underlying socket provider.",
            [SocketError.SystemNotReady] = "The network subsystem is unavailable.",
            [SocketError.VersionNotSupported] = "The version of the underlying socket provider is out of range.",
            [SocketError.NotInitialized] = "The underlying socket provider has not been initialized.",
            [SocketError.Disconnecting] = "A graceful shutdown is in progress.",
            [SocketError.TypeNotFound] = "The specified class was not found.",
            [SocketError.HostNotFound] = "No such host is known. The name is not an official host name or alias.",
            [SocketError.TryAgain] = "The name of the host could not be resolved. Try again later.",
            [SocketError.NoRecovery] = "The error is unrecoverable or the requested database cannot be located.",
            [SocketError.NoData] = "The requested name or IP address was not found on the name server.",
        };
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
