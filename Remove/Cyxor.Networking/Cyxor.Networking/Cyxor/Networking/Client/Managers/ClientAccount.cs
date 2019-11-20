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
using System.Security.Cryptography;

namespace Cyxor.Networking
{
    /*

    using Cyxor.Networking.Protocol;
    using Cyxor.Serialization;

    public partial class Client
    {
        public sealed class ClientAccount : ClientProperty
        {
            internal ClientAccount(Client client)
               : base(client)
            {

            }

            //public async Task<CommentedResult> ChangeUsernameAsync(string username, string password)
            //{
            //   return CommentedResult.Success;
            //}

            public async Task<Result> ChangePasswordAsync(ClientAccountPassword caPassword)
            {
                using (var packet = new Packet(Client) { Id = PackeId.PasswordChange, Internal = true })
                {
                    packet.Serializer.WriteRaw(caPassword.Password);
                    var result = await packet.QueryAsync(PackeId.PasswordChange, caPassword.MillisecondsTimeOut).ConfigureAwait(false);
                    return result ? packet.Serializer.ToSerializable<Result>() : result;
                }
            }

            public async Task<Result> ChangeSecurityAsync(ClientAccountSecurity caSecurity)
            {
                using (var packet = new Packet(Client) { Id = PackeId.SecurityChange, Internal = true })
                {
                    packet.Serializer.WriteRaw(caSecurity.Security);
                    var result = await packet.QueryAsync(PackeId.SecurityChange, caSecurity.MillisecondsTimeOut).ConfigureAwait(false);
                    return result ? packet.Serializer.ToSerializable<Result>() : result;
                }
            }

            // TODO: Fix
            public async Task<Result> UpdateProfileAsync(ClientAccountProfile caProfile)
            {
                using (var packet = new Packet(Client) { Id = PackeId.ProfileUpdate, Internal = true })
                {
                    //packet.Message.Write(caProfile.Profile);
                    packet.Serializer.Write(caProfile.UpdateMode);
                    var result = await packet.QueryAsync(PackeId.ProfileUpdate, caProfile.MillisecondsTimeOut).ConfigureAwait(false);
                    return result ? packet.Serializer.ToSerializable<Result>() : result;
                }
            }

            public static async Task<Result> ResetAsync(ClientAccountReset caReset)
            {
                return await Utilities.Task.Run(() =>
                {
                    var box = (Box)null;
                    var aes = (Aes)null;
                    var sck = (Socket)null;
                    var rsa = (RSACryptoServiceProvider)null;

                    try
                    {
                        if (string.IsNullOrEmpty(caReset.Email))
                            return new Result(ResultCode.EmailNullOrEmpty);

                        if (!Utilities.IsValidEmailFormat(caReset.Email))
                            return new Result(ResultCode.EmailInvalidFormat);

                        var ipAddress = Utilities.TryParseAddress(caReset.Address);

                        if (ipAddress == null)
                            return new Result(ResultCode.NetworkAddressInvalid);

                        if (caReset.Port < 0 || caReset.Port > ushort.MaxValue)
                            return new Result(ResultCode.NetworkPortOutOfRange);

                        aes = Aes.Create();
                        box = new Box { NodeFree = true };
                        rsa = new RSACryptoServiceProvider();
                        sck = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        sck.SendTimeout = caReset.MillisecondsTimeOut;
                        sck.ReceiveTimeout = caReset.MillisecondsTimeOut;

                        sck.Connect(ipAddress, caReset.Port);

                        var parameters = rsa.ExportParameters(false);

                        box.Internal = true;
                        box.Id = PackeId.Reset;
                        box.Serializer.Write(parameters.Modulus);
                        box.Serializer.Write(parameters.Exponent);

                        var data = box.SerialBuffer.ToByteArray();

                        sck.Send(data);

                        var buffer = new Buffer();
                        buffer.SetLength(256);

                        var bytesCount = sck.Receive(buffer.Data);

                        buffer.SetLength(bytesCount);

                        box.TryLoad(buffer, pop: false);

                        if (box.Id != PackeId.End && box.Id != PackeId.Reset)
                            return new Result(ResultCode.ProtocolErrorType);

                        buffer.Reset();
                        serializer.WriteRaw(rsa.Decrypt(box.Serializer.ToByteArray(), true));
                        buffer.SetPosition(0);

                        if (box.Id == PackeId.End)
                            return buffer.ToSerializable<Result>();
                        else if (box.Serializer.Length != 128)
                            return new Result(ResultCode.ProtocolErrorSize);

                        aes.Key = serializer.ReadBytes();
                        aes.IV = serializer.ReadBytes();

                        var noobAccount = new AccountReset { Email = caReset.Email };

                        box.Reset();

                        box.Internal = true;
                        box.QueryReply = true;
                        box.Id = PackeId.Reset;

                        using (var encryptor = aes.CreateEncryptor())
                            box.Serializer.WriteRaw(encryptor.TransformFinalBlock(noobAccount.SerialBuffer.Data, 0, noobAccount.SerialBuffer.Length));

                        sck.Send(box.SerialBuffer.ToByteArray());

                        buffer.SetLength(8192);
                        buffer.Reset();

                        bytesCount = sck.Receive(buffer.Data);

                        buffer.SetLength(bytesCount);

                        box.TryLoad(buffer, pop: false);

                        if (box.Id != PackeId.End && box.Id != PackeId.Reset)
                            return new Result(ResultCode.ProtocolErrorType);

                        byte[] decryptedData;

                        using (var decryptor = aes.CreateDecryptor())
                            decryptedData = decryptor.TransformFinalBlock(box.Serializer.Data, 0, box.Serializer.Length);

                        buffer.Reset();
                        serializer.WriteRaw(decryptedData);
                        buffer.SetPosition(0);

                        return buffer.ToSerializable<Result>();
                    }
                    catch (Exception ex)
                    {
                        return new Result(ResultCode.Exception, exception: ex);
                    }
                    finally
                    {
                        rsa?.Dispose();
                        aes?.Dispose();
                        sck?.Dispose();
                    }
                }).ConfigureAwait(false);
            }

            public static async Task<Result> CreateAsync(ClientAccountCreate caCreate)
            {
                return await Utilities.Task.Run(() =>
                {
                    var box = (Box)null;
                    var aes = (Aes)null;
                    var sck = (Socket)null;
                    var rsa = (RSACryptoServiceProvider)null;

                    try
                    {
                        if (string.IsNullOrEmpty(caCreate.Name))
                            return new Result(ResultCode.NameNullOrEmpty);

                        if (string.IsNullOrEmpty(caCreate.Email))
                            return new Result(ResultCode.EmailNullOrEmpty);

                        if (!Utilities.IsValidEmailFormat(caCreate.Email))
                            return new Result(ResultCode.EmailInvalidFormat);

                        var ipAddress = Utilities.TryParseAddress(caCreate.Address);

                        if (ipAddress == null)
                            return new Result(ResultCode.NetworkAddressInvalid);

                        if (caCreate.Port < 0 || caCreate.Port > ushort.MaxValue)
                            return new Result(ResultCode.NetworkPortOutOfRange);

                        aes = Aes.Create();
                        box = new Box { NodeFree = true };
                        rsa = new RSACryptoServiceProvider();
                        sck = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        sck.SendTimeout = caCreate.MillisecondsTimeOut;
                        sck.ReceiveTimeout = caCreate.MillisecondsTimeOut;

                        sck.Connect(ipAddress, caCreate.Port);

                        var parameters = rsa.ExportParameters(false);

                        box.Internal = true;
                        box.Id = PackeId.Noob;
                        box.Serializer.Write(parameters.Modulus);
                        box.Serializer.Write(parameters.Exponent);

                        var data = box.SerialBuffer.ToByteArray();

                        sck.Send(data);

                        var buffer = new Buffer();
                        buffer.SetLength(256);

                        var bytesCount = sck.Receive(buffer.Data);

                        buffer.SetLength(bytesCount);

                        box.TryLoad(buffer, pop: false);

                        if (box.Id != PackeId.End && box.Id != PackeId.Noob)
                            return new Result(ResultCode.ProtocolErrorType);

                        buffer.Reset();
                        serializer.WriteRaw(rsa.Decrypt(box.Serializer.ToByteArray(), true));
                        buffer.SetPosition(0);

                        if (box.Id == PackeId.End)
                            return buffer.ToSerializable<Result>();
                        else if (box.Serializer.Length != 128)
                            return new Result(ResultCode.ProtocolErrorSize);

                        aes.Key = serializer.ReadBytes();
                        aes.IV = serializer.ReadBytes();

                        var noobAccount = new AccountCreate { Name = caCreate.Name, Email = caCreate.Email, CustomData = caCreate.CustomData };

                        box.Reset();

                        box.Internal = true;
                        box.QueryReply = true;
                        box.Id = PackeId.Noob;

                        using (var encryptor = aes.CreateEncryptor())
                            box.Serializer.WriteRaw(encryptor.TransformFinalBlock(noobAccount.SerialBuffer.Data, 0, noobAccount.SerialBuffer.Length));

                        sck.Send(box.SerialBuffer.ToByteArray());

                        buffer.SetLength(8192);
                        buffer.Reset();

                        bytesCount = sck.Receive(buffer.Data);

                        buffer.SetLength(bytesCount);

                        box.TryLoad(buffer, pop: false);

                        if (box.Id != PackeId.End && box.Id != PackeId.Noob)
                            return new Result(ResultCode.ProtocolErrorType);

                        byte[] decryptedData;

                        using (var decryptor = aes.CreateDecryptor())
                            decryptedData = decryptor.TransformFinalBlock(box.Serializer.Data, 0, box.Serializer.Length);

                        buffer.Reset();
                        serializer.WriteRaw(decryptedData);
                        buffer.SetPosition(0);

                        return buffer.ToSerializable<Result>();
                    }
                    catch (SocketException ex)
                    {
                        return new Result(ResultCode.SocketError, ex.SocketErrorCode.ToString());
                    }
                    catch (Exception ex)
                    {
                        return new Result(ResultCode.Exception, exception: ex);
                    }
                    finally
                    {
                        rsa?.Dispose();
                        aes?.Dispose();
                        sck?.Dispose();
                    }
                }).ConfigureAwait(false);
            }
        }
    }
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */


/*
            public static async Task<Result> QueryServer(QueryServerData queryServerData)
            {
               return await Utilities.Task.Run(() =>
               {
                  var box = (Box)null;
                  var sck = (Socket)null;
                  var aes = (AesManaged)null;
                  var rsa = (RSACryptoServiceProvider)null;

                  try
                  {
                     if (string.IsNullOrEmpty(accountCreateData.Name))
                        return new Result(ResultCode.NameNullOrEmpty);

                     if (string.IsNullOrEmpty(accountCreateData.Email))
                        return new Result(ResultCode.EmailNullOrEmpty);

                     if (!Utilities.IsValidEmailFormat(accountCreateData.Email))
                        return new Result(ResultCode.EmailInvalidFormat);

                     var ipAddress = Utilities.TryParseAddress(accountCreateData.Address);

                     if (ipAddress == null)
                        return new Result(ResultCode.NetworkAddressInvalid);

                     if (accountCreateData.Port < 0 || accountCreateData.Port > ushort.MaxValue)
                        return new Result(ResultCode.NetworkPortOutOfRange);

                     aes = new AesManaged();
                     box = new Box { NodeFree = true };
                     rsa = new RSACryptoServiceProvider();
                     sck = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                     sck.SendTimeout = accountCreateData.MillisecondsTimeOut;
                     sck.ReceiveTimeout = accountCreateData.MillisecondsTimeOut;

                     sck.Connect(ipAddress, accountCreateData.Port);

                     var parameters = rsa.ExportParameters(false);

                     box.Internal = true;
                     box.Type = BoxType.Noob;
                     box.Message.Write(parameters.Modulus);
                     box.Message.Write(parameters.Exponent);

                     var data = box.SerialBuffer.ToByteArray();

                     sck.Send(data);

                     var buffer = new Buffer();
                     buffer.SetLength(256);

                     var bytesCount = sck.Receive(buffer.Data);

                     buffer.SetLength(bytesCount);

                     box.TryLoad(buffer, pop: false);

                     if (box.Type != BoxType.End && box.Type != BoxType.Noob)
                        return new Result(ResultCode.ProtocolErrorType);

                     buffer.Reset();
                     serializer.WriteRaw(rsa.Decrypt(box.Message.ToByteArray(), true));
                     buffer.Position = 0;

                     if (box.Type == BoxType.End)
                        return buffer.ToResult();
                     else if (box.Message.Length != 128)
                        return new Result(ResultCode.ProtocolErrorSize);

                     aes.Key = serializer.ReadBytes();
                     aes.IV = serializer.ReadBytes();

                     var noobAccount = new AccountCreate { Name = accountCreateData.Name, Email = accountCreateData.Email, CustomData = accountCreateData.CustomData };

                     box.Reset();

                     box.Internal = true;
                     box.QueryReply = true;
                     box.Type = BoxType.Noob;

                     using (var encryptor = aes.CreateEncryptor())
                        box.Message.WriteRaw(encryptor.TransformFinalBlock(noobAccount.SerialBuffer.Data, 0, noobAccount.SerialBuffer.Length));

                     sck.Send(box.SerialBuffer.ToByteArray());

                     buffer.SetLength(8192);
                     buffer.Reset();

                     bytesCount = sck.Receive(buffer.Data);

                     buffer.SetLength(bytesCount);

                     box.TryLoad(buffer, pop: false);

                     if (box.Type != BoxType.End && box.Type != BoxType.Noob)
                        return new Result(ResultCode.ProtocolErrorType);

                     byte[] decryptedData;

                     using (var decryptor = aes.CreateDecryptor())
                        decryptedData = decryptor.TransformFinalBlock(box.Message.Data, 0, box.Message.Length);

                     buffer.Reset();
                     serializer.WriteRaw(decryptedData);
                     buffer.Position = 0;

                     return buffer.ToResult();
                  }
                  catch (SocketException ex)
                  {
                     return new Result(ResultCode.SocketError, ex.SocketErrorCode.ToString());
                  }
                  catch (Exception ex)
                  {
                     return new Result(ResultCode.Exception, exception: ex);
                  }
                  finally
                  {
                     if (rsa != null)
                        rsa.Dispose();

                     if (aes != null)
                        aes.Dispose();

                     if (sck != null)
                        sck.Close();
                  }
               }).ConfigureAwait(false);
            }
            */
