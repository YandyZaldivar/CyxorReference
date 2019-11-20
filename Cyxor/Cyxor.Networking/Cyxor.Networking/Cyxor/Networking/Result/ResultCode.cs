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

namespace Cyxor.Networking
{
    public enum ResultCode
    {
        Success = 0,
        Error = 1,

        SocketError = 2,

        AccountNotExist,
        AccountNameTaken,
        AccountHashTaken,
        AccountEmailTaken,
        AccountResetRefused,
        AccountResetCanceled,
        AccountAlreadyDeleted,
        AccountCreationCanceled,
        AccountDeletionCanceled,
        AccountSecurityOutOfRange,

        OperationFaulted,
        OperationTimedOut,
        OperationCanceled,
        OperationInProgress,

        AuthenticationFailed,

        ClientReconnected,
        ClientLoginTimeout,
        ClientConnectionCanceled,
        ClientUpdateRequestCanceled,

        ClientNotConnected,

        ClientDisconnectionNone,
        ClientDisconnectionLocal,
        ClientDisconnectionError,
        ClientDisconnectionRemote,

        CommandCanceled,
        CommandParseError,
        CommandSyntaxError,
        CommandSecurityViolation,
        CommandArgumentsMismatch,

        ConnectionAlreadyTerminated,

        EmailNullOrEmpty,
        EmailInvalidFormat,

        Exception,
        ExceptionInUserCode,
        ExceptionInLogEvent,

        FileTransferRequestCanceled,

        NameUnavailable,
        NameNullOrEmpty,
        NameRegexViolation,
        NameMinLengthViolation,
        NameMaxLengthViolation,

        NetworkUnavailable,
        NetworkIPUnsupported,
        NetworkAddressInvalid,
        NetworkPortOutOfRange,

        NodeNotConnected,
        NodeConnectionFailed,
        NodeDisconnectionFailed,
        NodeDisconnectionCanceled, // TODO: [Obsolete]

        PacketQueryNotSupported,
        PacketSendingInProgress,
        PacketSendMultiConnectionsErrors,

        ProtocolError,
        ProtocolErrorData,
        ProtocolErrorType,
        ProtocolErrorSize,
        ProtocolErrorCrypto,

        ServerShutdown,
        ServerDatabaseDisabled,

        Srp_M_Mismatch,
        SRPClientNullOrEmptyA,

        SynchronizationContextNull,

        UpdateFailed,
        UpdateCompleted,

        //HTTP Status Code

        BadRequest = 400,
        NotFound = 404,
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
