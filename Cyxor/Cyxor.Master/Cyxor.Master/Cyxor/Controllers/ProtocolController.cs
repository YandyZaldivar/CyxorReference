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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

namespace Cyxor.Controllers
{
    using Models;
    using Networking;
    using Networking.Config;

    class ProtocolController : DbContextMasterController
    {
        [Action(typeof(AuthRequest))]
        async Task<AuthResponse> Auth(AuthRequest model)
        {
            var authResponse = new AuthResponse();

            var account = await MasterDbContext.Accounts.SingleOrDefaultAsync(p => p.Name == model.I);

            if (account == null)
                authResponse.Result = new Result(ResultCode.AuthenticationFailed);
            else if (model.Schema == AuthenticationSchema.Basic)
            {
                if (!Node.Config.AuthenticationMode.HasFlag(AuthenticationSchema.Basic))
                    authResponse.Result = new Result(ResultCode.AuthenticationFailed);
                else
                {
                    //var bytes = Convert.FromBase64String(authRequest.A);
                    //var hashBytes = SHA256.Create().ComputeHash(bytes);
                    //var passwordHash = Encoding.UTF8.GetString(hashBytes);

                    if (account.Password != model.PasswordHash)
                        authResponse.Result = new Result(ResultCode.AuthenticationFailed);
                    else
                        Connection.Account = new AccountApiModel { Id = account.Id, Name = account.Name, SecurityLevel = account.SecurityLevel };
                }
            }
            else
                authResponse.Result = new Result(ResultCode.AuthenticationFailed);

            return authResponse;
        }

        /*
        async void ClientCommand(Link link)
        {
            var result = await Server.Commands.ExecuteAsync(link.TcpBox.Buffer.ToString()).ConfigureAwait(false);

            using (var packet = new Packet(link) { Id = PackeId.Command })
            {
                packet.Buffer.WriteRaw(result);
                await packet.SendAsync().ConfigureAwait(false);
            }
        }

        //if (clientConnectingEventArgs.NameTaken)
        //{
        //    var existLink = Server.Connections.Find(packet.Connection.Name).Link;

        //    if (existLink != null)
        //        await existLink.DisconnectAsync(new Result(ResultCode.ClientReconnected,
        //            string.Format("A client with the same login has connected from: ", packet.Connection.RemoteEndPoint.Address.ToString()))).ConfigureAwait(false);
        //}

        async void Noob(Link link)
        {
#pragma warning disable 4014

            if (!Server.Config.Database.Enabled || link.Connection.State != ConnectionState.Accepted)
            {
                link.DisconnectAsync(new Result(ResultCode.ProtocolError, string.Format(Utilities.Strings.InvalidInternalOperation, link.Connection.State)));
                return;
            }

            link.Connection.State = ConnectionState.Registering;

            var accountRsa = link.TcpBox.Buffer.ToSerializable<AccountRSA>();
            var accountAes = new AccountAes { Key = link.Noob.Aes.Key, IV = link.Noob.Aes.IV };

            link.Noob.Rsa.ImportParameters(new RSAParameters { Modulus = accountRsa.Modulus, Exponent = accountRsa.Exponent });

            using (var packet = new Packet(link) { Id = MasterPackeId.Noob, Internal = true })
            {
                packet.Buffer.WriteRaw(link.Noob.Rsa.Encrypt(accountAes.Buffer.ToByteArray(), true));

                // TODO: Fix
                //if (await packet.QueryAsync(PackeId.Noob, 30000).ConfigureAwait(false))
                if (await packet.QueryAsync(0).ConfigureAwait(false))
                {
                    byte[] decryptedData;

                    using (var decryptor = link.Noob.Aes.CreateDecryptor())
                        decryptedData = decryptor.TransformFinalBlock(packet.Buffer.Data, 0, packet.Buffer.Length);

                    var buffer = new Buffer();
                    buffer.WriteRaw(decryptedData);

                    var noobAccount = new AccountCreate { Buffer = buffer };

                    var result = noobAccount.Validate(Node);

                    if (result)
                    {
                        var account = new Account { Name = noobAccount.Name };
                        account.CreationSource = AccountCreationSource.Free;
                        account.ContactInfo.Email = noobAccount.Email;

                        result = await Server.Database.Account.CreateAsync(account).ConfigureAwait(false);
                    }

                    link.DisconnectAsync(result);

                    //EndPacket(link, result);
                    return;
                }
                else
                    //EndPacket(link, new Result(ResultCode.AsyncOperationTimedOut));
                    link.DisconnectAsync(new Result(ResultCode.AsyncOperationTimedOut));
            }

#pragma warning restore 4014
        }

        async void Reset(Link link)
        {
#pragma warning disable 4014

            if (!Server.Config.Database.Enabled || link.Connection.State != ConnectionState.Accepted)
            {
                link.DisconnectAsync(new Result(ResultCode.ProtocolError, string.Format(Utilities.Strings.InvalidInternalOperation, link.Connection.State)));
                return;
            }

            link.Connection.State = ConnectionState.Registering;

            var accountRsa = link.TcpBox.Buffer.ToSerializable<AccountRSA>();

            var accountAes = new AccountAes { Key = link.Noob.Aes.Key, IV = link.Noob.Aes.IV };

            link.Noob.Rsa.ImportParameters(new RSAParameters { Modulus = accountRsa.Modulus, Exponent = accountRsa.Exponent });

            using (var packet = new Packet(link) { Id = MasterPackeId.Reset, Internal = true })
            {
                packet.Buffer.WriteRaw(link.Noob.Rsa.Encrypt(accountAes.Buffer.ToByteArray(), true));

                // TODO: Fix
                //if (await packet.QueryAsync(PackeId.Reset, 30000).ConfigureAwait(false))
                if (await packet.QueryAsync(0).ConfigureAwait(false))
                {
                    byte[] decryptedData;

                    using (var decryptor = link.Noob.Aes.CreateDecryptor())
                        decryptedData = decryptor.TransformFinalBlock(packet.Buffer.Data, 0, packet.Buffer.Length);

                    var buffer = new Buffer();
                    buffer.WriteRaw(decryptedData);

                    var resetAccount = new Protocol.AccountReset { Buffer = buffer };

                    var result = resetAccount.Validate(Node);

                    if (result)
                        result = await Server.Database.Account.ResetAsync(new Model.AccountReset { Email = resetAccount.Email }).ConfigureAwait(false);

                    //EndPacket(link, result);
                    link.DisconnectAsync(result);
                    return;
                }
                else
                    //EndPacket(link, new Result(ResultCode.AsyncOperationTimedOut));
                    link.DisconnectAsync(new Result(ResultCode.AsyncOperationTimedOut));
            }

#pragma warning restore 4014
        }

        // TODO: Update
        async void Auth(Link link)
        {
#pragma warning disable 4014

            if (link.Connection.State != ConnectionState.Accepted)
            {
                link.DisconnectAsync(new Result(ResultCode.ProtocolError, string.Format(Utilities.Strings.InvalidInternalOperation, link.Connection.State)));
                return;
            }

            var authRequest = link.TcpBox.Buffer.ToSerializable<AuthRequest>();

            var result = authRequest.Validate(Node);

            if (!result)
            {
                link.DisconnectAsync(result);
                return;
            }

            var srpServer = default(SrpServer);

            using (var dbContext = Server.Config.Database.CreateDbContext<DbContext>())
            {
                var account = dbContext.Accounts.SingleOrDefault(acc => acc.Name == authRequest.I);

                if (account != null)
                    if (account.ExpirationTime != null)
                        if (account.ExpirationTime < (DateTime.Now - account.JoinDate).TotalHours)
                        {
                            Server.Database.CheckExpirationTime(account.Id.ToString(), ignoreDisconnected: false);
                            account = null;
                        }

                if (account == null)
                {
                    // If we get here is because the account name doesn't exist but we don't want the client knows
                    // what is wrong: the name or the password. So we create a fake random password at server side
                    // and the login process will fail anyway because session keys won't match.

                    var passwordBytes = new byte[32];
                    RandomNumberGenerator.Create().GetBytes(passwordBytes);
                    var password = (SecureString)null;

                    unsafe
                    {
                        fixed (char* chars = new System.Numerics.BigInteger(passwordBytes).ToString("X"))
                            password = new SecureString(chars, passwordBytes.Length * 2);
                    }

                    password.MakeReadOnly();
                    Array.Clear(passwordBytes, 0, passwordBytes.Length);

                    var srpAccount = new SrpAccount(authRequest.I, password, Node.Config.Srp.InternalConfig);
                    account = new Account { Name = authRequest.I, Password = password, SRP_s = srpAccount.s, SRP_v = srpAccount.v };
                }

                var srp_s = account.SRP_s;
                var srp_v = account.SRP_v;

                if (authRequest.LoginReset)
                    if (account.Reset != null)
                    {
                        srp_s = account.Reset.SRP_s;
                        srp_v = account.Reset.SRP_v;
                    }
                    else
                        result = new Result(ResultCode.AccountResetRefused);

                var srpAccountPassword = new SrpAccount(account.Name, srp_s, srp_v);
                srpServer = new SrpServer(srpAccountPassword, Node.Config.Srp.InternalConfig);
                link.Crypto = new LinkCrypto(srpServer.Calculate(authRequest.A));
                //link.Connection.Account = account;
                account.Connection = link.Connection;
            }

            var authResult = new AuthResponse();
            {
                if ((authResult.Result = result))
                {
                    authResult.s = srpServer.s;
                    authResult.B = srpServer.B;

                    link.Connection.State = ConnectionState.Authenticating;
                }
            }

            // TODO: Fix
            //using (var packet = new Packet(link) { Id = PackeId.Auth, Internal = true, QueryReply = true })
            using (var packet = new Packet(link) { Id = PackeId.Auth, Internal = true })
            {
                packet.Buffer.WriteRaw(authResult);
                await packet.SendAsync().ConfigureAwait(false);
            }

            if (!result)
                link.DisconnectAsync(new Result(ResultCode.AuthenticationFailed));

#pragma warning restore 4014
        }

        void ChangeName(Link link)
        {
            // TODO:
        }

        async void UpdateProfile(Link link)
        {
            var profile = link.TcpBox.Buffer.ReadObject<AccountProfile>();
            var updateProfileMode = link.TcpBox.Buffer.ReadEnum<ClientAccountProfileUpdateMode>();
            var result = await Server.Database.Account.UpdateProfileAsync(link.Connection.Name, profile, updateProfileMode).ConfigureAwait(false);

            //TODO: Fix
            //using (var packet = new Packet(link) { Id = PackeId.ProfileUpdate, Internal = true, Message = result, QueryReply = true })
            using (var packet = new Packet(link) { Id = MasterPackeId.ProfileUpdate, Internal = true, Message = result })
            {
                packet.Buffer.WriteRaw(result);
                await packet.SendAsync().ConfigureAwait(false);
            }
        }

        async void ChangePassword(Link link)
        {
            var newPassword = link.TcpBox.Buffer.ToString();
            var result = await Server.Database.Account.ChangePasswordAsync(link.Connection.Name, newPassword).ConfigureAwait(false);

            //TODO: Fix
            //using (var packet = new Packet(link) { Id = PackeId.PasswordChange, Internal = true, QueryReply = true })
            using (var packet = new Packet(link) { Id = MasterPackeId.PasswordChange, Internal = true })
            {
                packet.Buffer.WriteRaw(result);
                await packet.SendAsync().ConfigureAwait(false);
            }
        }

        async void ChangeSecurity(Link link)
        {
            var newSecurity = link.TcpBox.Buffer.ReadInt32();
            var result = await Server.Database.Account.ChangeSecurityAsync(link.Connection.Name, newSecurity).ConfigureAwait(false);

            //TODO: Fix
            //using (var packet = new Packet(link) { Id = PackeId.SecurityChange, Internal = true, QueryReply = true })
            using (var packet = new Packet(link) { Id = MasterPackeId.SecurityChange, Internal = true })
            {
                packet.Buffer.WriteRaw(result);
                await packet.SendAsync().ConfigureAwait(false).ConfigureAwait(false);
            }
        }
        */
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
