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
    public enum NetworkErrorMal
    {
      // System.Net.Sockets.SocketError
      SocketError = -1,
      Success = 0,
      OperationAborted = 995,
      IOPending = 997,
      Interrupted = 10004,
      AccessDenied = 10013,
      Fault = 10014,
      InvalidArgument = 10022,
      TooManyOpenSockets = 10024,
      WouldBlock = 10035,
      InProgress = 10036,
      AlreadyInProgress = 10037,
      NotSocket = 10038,
      DestinationAddressRequired = 10039,
      MessageSize = 10040,
      ProtocolType = 10041,
      ProtocolOption = 10042,
      ProtocolNotSupported = 10043,
      SocketNotSupported = 10044,
      OperationNotSupported = 10045,
      ProtocolFamilyNotSupported = 10046,
      AddressFamilyNotSupported = 10047,
      AddressAlreadyInUse = 10048,
      AddressNotAvailable = 10049,
      NetworkDown = 10050,
      NetworkUnreachable = 10051,
      NetworkReset = 10052,
      ConnectionAborted = 10053,
      ConnectionReset = 10054,
      NoBufferSpaceAvailable = 10055,
      IsConnected = 10056,
      NotConnected = 10057,
      Shutdown = 10058,
      TimedOut = 10060,
      ConnectionRefused = 10061,
      HostDown = 10064,
      HostUnreachable = 10065,
      ProcessLimit = 10067,
      SystemNotReady = 10091,
      VersionNotSupported = 10092,
      NotInitialized = 10093,
      Disconnecting = 10101,
      TypeNotFound = 10109,
      HostNotFound = 11001,
      TryAgain = 11002,
      NoRecovery = 11003,
      NoData = 11004,

      // Windows.Networking.Sockets.SocketErrorStatus
      Unknown = -2,
      //OperationAborted = 1,
      HttpInvalidServerResponse = 2,
      ConnectionTimedOut = 3,
      //AddressFamilyNotSupported = 4,
      SocketTypeNotSupported = 5,
      //HostNotFound = 6,
      NoDataRecordOfRequestedType = 7,
      NonAuthoritativeHostNotFound = 8,
      ClassTypeNotFound = 9,
      //AddressAlreadyInUse = 10,
      CannotAssignRequestedAddress = 11,
      //ConnectionRefused = 12,
      NetworkIsUnreachable = 13,
      UnreachableHost = 14,
      NetworkIsDown = 15,
      NetworkDroppedConnectionOnReset = 16,
      SoftwareCausedConnectionAbort = 17,
      ConnectionResetByPeer = 18,
      HostIsDown = 19,
      NoAddressesFound = 20,
      TooManyOpenFiles = 21,
      MessageTooLong = 22,
      CertificateExpired = 23,
      CertificateUntrustedRoot = 24,
      CertificateCommonNameIsIncorrect = 25,
      CertificateWrongUsage = 26,
      CertificateRevoked = 27,
      CertificateNoRevocationCheck = 28,
      CertificateRevocationServerOffline = 29,
      CertificateIsInvalid = 30,
   }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
