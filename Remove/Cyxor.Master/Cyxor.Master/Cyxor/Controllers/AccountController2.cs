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
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Microsoft.EntityFrameworkCore;

namespace Cyxor.Controllers
{
    using Data;
    using Models;
    using Networking;
    using Events = Networking.Events;

    /*
    public class AccountController2 : AccountBaseController//, IDisposable
    {
        //protected Master Master => Node as Master;

        //MasterConnection masterConnection;
        //protected new MasterConnection Connection => masterConnection ?? (masterConnection = base.Connection as MasterConnection);

        protected virtual MasterDbContext MasterDbContext { get; set; }
        //protected MasterDbContext MasterDbContext => masterDbContext ?? (masterDbContext = Master.GetService<MasterDbContext>());

        //public void Dispose() => MasterDbContext.Dispose();

        [ScopeInitializer]
        public void Initialize(MasterDbContext dbContext) => MasterDbContext = dbContext;

        [Action(ApiId.AddAccount, @internal: true, Security = -1)]
        public async Task<Result> Add(AccountApiModel accountApiModel)
        {
            var result = Result.Success;

            var bytes = Encoding.UTF8.GetBytes(accountApiModel.Password);
            var passwordHash = Convert.ToBase64String(SHA256.Create().ComputeHash(bytes));

            var account = new Account { Name = accountApiModel.Name, PasswordHash = passwordHash };

            MasterDbContext.Accounts.Add(account);

            await MasterDbContext.SaveChangesAsync();

            return result;
        }

        //[Action(Id.AddAccount, @internal: true, Security = -1)]
        public async Task<Result> Add2(AccountApiModel accountApiModel)
        {
            var result = Result.Success;

            var namePreview = false;
            var emailPreview = false;
            var hashPreview = false;

            var account = default(Account);

            try
            {
                var bytes = Encoding.UTF8.GetBytes(accountApiModel.Password);
                var passwordHash = Convert.ToBase64String(SHA256.Create().ComputeHash(bytes));

                account = new Account { Name = accountApiModel.Name, PasswordHash = passwordHash };

                if (!(result = Node.Database.ValidateServer()))
                    return result;

                if (!(result = Utilities.Models.Validate(Node, account)))
                    return result;

                if (!Node.Database.NamesPreview.TryAdd(account.Name, account))
                    return new Result(ResultCode.AccountNameTaken);

                namePreview = true;

                if (!Node.Database.HashesPreview.TryAdd(account.GetHashCode(), account))
                    return new Result(ResultCode.AccountHashTaken);

                hashPreview = true;

                if (account.Profile.Email != null)
                {
                    if (!Node.Database.EmailsPreview.TryAdd(account.Profile.Email, account))
                        return new Result(ResultCode.AccountEmailTaken);

                    emailPreview = true;
                }

                var previousAccount = await MasterDbContext.Accounts.AsNoTracking().SingleOrDefaultAsync(p => p.Name == account.Name).ConfigureAwait(false);

                if (previousAccount != null)
                    return new Result(ResultCode.AccountNameTaken);

                previousAccount = await MasterDbContext.Accounts.AsNoTracking().SingleOrDefaultAsync(p => p.GetHashCode() == account.GetHashCode()).ConfigureAwait(false);

                if (previousAccount != null)
                    return new Result(ResultCode.AccountHashTaken);

                if (account.Profile.Email != null)
                {
                    previousAccount = await MasterDbContext.Accounts.AsNoTracking().SingleOrDefaultAsync(p => p.Profile.Email == account.Profile.Email).ConfigureAwait(false);

                    if (previousAccount != null)
                        return new Result(ResultCode.AccountEmailTaken);
                }

                account = MasterDbContext.Accounts.Add(account).Entity;

                MasterDbContext.Profiles.Add(account.Profile);

                var accountCreatingEventArgs = Node.Events.Post(new Events.Server.AccountCreatingEventArgs(Node, account, Connection, null));// customData));

                await accountCreatingEventArgs.ConfigureAwait(false);

                if (accountCreatingEventArgs.Cancel)
                    return new Result(ResultCode.AccountCreationCanceled, accountCreatingEventArgs.CancelReason);

                //if (account.Password != null && account.Password.Length > 0)
                //{
                //    var srpAccount = new SrpAccount(account.Name, account.Reset.Password, Master.Config.Srp.InternalConfig);

                //    account.SRP_s = srpAccount.s;
                //    account.SRP_v = srpAccount.v;
                //}

                //if (account.Reset. .Code != null && account.ActivationInfo.Code.Length > 0)


                //{
                //    var srpActivation = new SrpAccount(account.Name, account.Reset.Password, Master.Config.Srp.InternalConfig);
                //    account.Reset.SRP_s = srpActivation.s;
                //    account.Reset.SRP_v = srpActivation.v;

                //    dbContext.Resets.Add(account.Reset);
                //}


                // TODO: Enable this
                //await MasterDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                await Node.Events.Post(new Networking.Events.Server.AccountCreatedEventArgs(Node, account, Connection, null)).ConfigureAwait(false);

                return new Result(comment: $"Account '{account.Name}' successfully created.");
                //}

            }
            catch (Exception ex)
            {
                return new Result(ResultCode.Exception, exception: ex);
            }
            finally
            {
                if (namePreview)
                    Node.Database.NamesPreview.TryRemove(account.Name, out account);

                if (hashPreview)
                    Node.Database.HashesPreview.TryRemove(account.GetHashCode(), out account);

                if (emailPreview)
                    Node.Database.EmailsPreview.TryRemove(account.Profile.Email, out account);
            }
        }

        [Command("account create", Arguments = "$name [$password] [$email]",
            Security = 3,
            Description = "Create account and set password and/or email to it. The password and the email are optional " +
                          "but not both. A password can only be specified when the command is executed directly on the server " +
                          "or when executed by an account with a security level equal or greater than 3.")]
        public Task<Result> AccountCreate(CommandArgs args)
        {
            var result = Result.Success;

            var name = args["$name"];
            var email = args["$email"];
            var password = args["$password"];

            if (email != null)
            {
                if (!Utilities.IsValidEmailFormat(email))
                    return Utilities.Task.FromResult(new Result(ResultCode.EmailInvalidFormat));
            }


            //if (masterConnection?.Account == null)
            //    accountOrigin = OriginValue.Server;
            //else if (masterConnection.Account.Security >= 3)
            //    accountOrigin = OriginValue.Poweruser;

            //var newAccount = new Account { Name = name, InsecurePassword = password, AccountOrigin = accountOrigin };
            //masterConnection.Account.Profile.Email = email;

            //var newAccount = new Account { Name = name, PasswordHash = password };

            //return master.Database.Account.CreateAsync(newAccount, masterConnection);

            var accountApiModel = new AccountApiModel { Name = name, Password = password };

            if (!(result = Utilities.Models.Validate(Node, accountApiModel)))
                return Task.FromResult(result);

            return Add(accountApiModel);
        }

        [Action(ApiId.RemoveAccount, @internal: true, Security = -1)]
        public async Task<Result> Remove(NameOrIdApiModel nameOrIdApiModel)
        {
            var result = Result.Success;

            if (!(result = Validate(nameOrIdApiModel)))
                return result;

            try
            {
                //if (!isId)
                //    if (!(result = Master.Config.Names.Validate(ref accountNameOrId)))
                //        return result;

                var account = await MasterDbContext.Accounts.SingleOrDefaultAsync(p => nameOrIdApiModel.IsId ? p.Id == nameOrIdApiModel.Id : p.Name == nameOrIdApiModel.Name).ConfigureAwait(false);

                if (account == null)
                    return new Result(ResultCode.AccountNotExist);

                var accountDeleting = Node.Events.Post(new Events.Server.AccountDeletingEventArgs(Node, account, Connection));

                await accountDeleting.ConfigureAwait(false);

                if (accountDeleting.Cancel)
                    return new Result(ResultCode.AccountDeletionCanceled, accountDeleting.CancelReason);

                MasterDbContext.Accounts.Remove(account);

                var connection = Node.Connections.Find(account.Name);

                if (connection != null)
                    await connection.DisconnectAsync("Your account has been deleted.").ConfigureAwait(false);

                await MasterDbContext.SaveChangesAsync().ConfigureAwait(false);

                await Node.Events.Post(new Events.Server.AccountDeletedEventArgs(Node, account, Connection)).ConfigureAwait(false);

                return new Result(comment: $"Account '{account.Name}' deleted.");
            }
            catch (Exception exc)
            {
                return new Result(ResultCode.Exception, exception: exc);
            }
        }

        [Command("account remove",
            Arguments = "$nameOrId",
            Description = "Remove the account with the specified name or id.")]
        public Task<Result> Remove(CommandArgs args) => Remove(new NameOrIdApiModel { NameOrId = args["$nameOrId"] });

        [Command("acc rr yy",
            Arguments = "$nameOrId",
            Description = "Remove the account with the specified name or id.")]
        public Task<Result> RRR(CommandArgs args) => Remove(new NameOrIdApiModel { NameOrId = args["$nameOrId"] });

        [Action(ApiId.UpdateAccount, @internal: true, Security = -1)]
        public async Task<Result> AccountUpdateSecurity(Packet packet, AccountApiModel addAccount)
        {
            var result = Result.Success;

            await MasterDbContext.SaveChangesAsync();

            return result;
        }

        [Command("master account update security",
            Arguments = "$nameOrId $security",
            Description = "Updates the security level for the given account.")]
        public async Task<Result> AccountUpdateSecurity(CommandArgs args)
        {
            var master = args.Server as Master;

            var name = args["$nameOrId"];

            var security = default(int);

            if (!int.TryParse(args["$security"], out security))
                return new Result(ResultCode.CommandSyntaxError);

            return await master.Database.Account.ChangeSecurityAsync(name, security).ConfigureAwait(false);
        }
    }
    */
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
