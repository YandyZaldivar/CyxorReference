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
    internal static class ResultComment
    {
        internal const string

            AccountNotExist = "Account not exist.",
            AccountNameTaken = "The account name is already in use.",
            AccountHashTaken = "The account name hash conflicts with another account.",
            AccountEmailTaken = "Another account is already using the same email address.",
            AccountResetRefused = "The account is not flagged for resetting.",
            AccountResetCanceled = "The account reset was canceled.",
            AccountAlreadyDeleted = "The account has been already deleted.",
            AccountCreationCanceled = "The account creation was canceled.",
            AccountDeletionCanceled = "The account deletion was canceled.",
            AccountSecurityOutOfRange = "The account security level is out of range.",

            AsyncOperationFaulted = "An exception was thrown in the asynchronous operation process.",
            AsyncOperationTimedOut = "The time alloted for the asynchronous operation has expired.",
            AsyncOperationCanceled = "The asynchronous operation was canceled by the user.",
            AsyncOperationInProgress = "The asynchronous operation is in progress.",

            AuthenticationFailed = "Invalid user name or password.",

            ClientReconnected = "Client with the same login reconnected.",
            ClientLoginTimeout = "The time alloted for the login process has expired.",
            ClientConnectionCanceled = "Client connection canceled.",
            ClientUpdateRequestCanceled = "The client has canceled the update request.",

            ClientNotConnected = "The specified client is not connected.",

            ClientDisconnectionNone = "Client disconnection not initiated.",
            ClientDisconnectionLocal = "Client disconnection initiated locally.",
            ClientDisconnectionError = "Client disconnection initiated by a socket error.",
            ClientDisconnectionRemote = "Client disconnection initiated by the remote side.",

            NotFound = "404 Not Found",
            CommandCanceled = "Command canceled.",
            CommandParseError = "Cannot parse command. Every token must be divided by a white space and optionally enclosed in quotes.",
            CommandSyntaxError = "Command syntax error.",
            CommandSecurityViolation = "You don't have enough permissions to execute that command.",
            CommandArgumentsMismatch = "The number of arguments provided for this command is incorrect.",

            ConnectionAlreadyTerminated = "The connection is already terminated and the internal object recycled.",

            EmailNullOrEmpty = "The email cannot be null or empty.",
            EmailInvalidFormat = "The email format is invalid.",

            FileTransferRequestCanceled = "The file transfer request was canceled.",

            NameUnavailable = "That name is unavailable.",
            NameNullOrEmpty = "The name can't be null or empty",
            NameRegexViolation = "The name contains invalid characters or in the wrong order.",
            NameMinLengthViolation = "The name is too short.",
            NameMaxLengthViolation = "The name is too long.",

            NetworkUnavailable = "Network unavailable. Consider using the loop-back address for testing purposes.",
            NetworkIPUnsupported = "Network Internet Protocol unsupported.",
            NetworkAddressInvalid = "Cannot resolve host or parse IP address.",
            NetworkPortOutOfRange = "The port number is out of range.",

            NodeNotConnected = "The node is not connected.",
            NodeConnectionFailed = "Connection failed because the network node is currently connected or a connection or disconnection is in progress.",
            NodeDisconnectionFailed = "Disconnection failed because the network node is currently disconnected or a connection or disconnection is in progress.",
            NodeDisconnectionCanceled = "Disconnection canceled.",

            PacketQueryNotSupported = "Queries can't be performed on box with multiple connections.",
            PacketSendingInProgress = "The send operation on the packet has already been initiated.",
            PacketSendMultiConnectionsErrors = "There were sending errors on some or all destination connections.",
            //PacketSendMultiConnectionsSuccess = "The packet has been successfully sent to all destination connections",

            ProtocolError = "Unexpected data received from remote end point.",
            ProtocolErrorData = "Unexpected data received from remote end point.",
            ProtocolErrorType = "Unexpected data type received from remote end point.",
            ProtocolErrorSize = "Unexpected data size received from remote end point.",
            ProtocolErrorCrypto = "Cannot decrypt data using the current session key.",

            ServerShutdown = "Server shutdown.",
            ServerDatabaseDisabled = "The server database is disabled.",

            Srp_M_Mismatch = "The calculated Srp M doesn't match with the remote side.",
            SRPClientNullOrEmptyA = "The client ephemeral value A cannot be null or empty.",

            SynchronizationContextNull = "Cannot provide a null synchronization context when EventDispatching is set to Synchronized.",

            UpdateFailed = "Update failed.",
            UpdateCompleted = "Update completed.",

            //HTTP Status Code

            BadRequest = "Bad Request";
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
