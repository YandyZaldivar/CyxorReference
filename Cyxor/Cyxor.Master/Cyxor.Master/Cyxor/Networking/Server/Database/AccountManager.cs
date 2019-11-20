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
using System.Security;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using Microsoft.EntityFrameworkCore;

namespace Cyxor.Networking
{
    using Data;
    using Models;
    using Security;
    using Protocol;
    using Serialization;

    using Config.Server;

    public partial class Master
    {
        public partial class MasterDatabase : MasterProperty
        {
            public class AccountManager : MasterProperty
            {
                ConcurrentDictionary<string, Account> PreviewEmails = new ConcurrentDictionary<string, Account>();
                ConcurrentDictionary<string, Account> PreviewNames = new ConcurrentDictionary<string, Account>();
                ConcurrentDictionary<int, Account> PreviewHashes = new ConcurrentDictionary<int, Account>();

                internal AccountManager(Master master) : base(master)
                {

                }

                internal async void CheckExpirationTime(string accountNameOrId = null, MasterDbContext masterDbContext = null, Account account = null)
                {
                    var result = Result.Success;

                    var dbContext = masterDbContext;
                    //var dbContext = masterDbContext ?? Master.GetService<MasterDbContext>();

                    if (account != null)
                    {
                        /*
                        if (account.ExpirationHours != null)
                            if (account.ExpirationHours < (DateTime.Now - account.JoinDate).TotalHours)
                                result = await DeleteAsync(account.Id.ToString(), null).ConfigureAwait(false);
                        */
                    }
                    else if (accountNameOrId == null)
                    {
                        foreach (var accountItem in dbContext.Accounts)
                            CheckExpirationTime(null, dbContext, accountItem);
                    }
                    else
                    {
                        var name = default(string);

                        if (!int.TryParse(accountNameOrId, out var id))
                            name = accountNameOrId;

                        account = await dbContext.Accounts.SingleOrDefaultAsync(p => name == null ? p.Id == id : p.Name == name).ConfigureAwait(false);

                        CheckExpirationTime(null, dbContext, account);
                    }
                }

                public async Task<Result> ChangeNameAsync(string accountNameOrId, string newName, string insecurePassword)
                {
                    var secureString = (SecureString)null;

                    unsafe
                    {
                        fixed (char* chars = insecurePassword)
                            secureString = new SecureString(chars, insecurePassword.Length);
                    }

                    secureString.MakeReadOnly();

                    return await ChangePasswordAsync(accountNameOrId, secureString).ConfigureAwait(false);
                }

                public async Task<Result> ChangeNameAsync(string accountNameOrId, string newName, SecureString password)
                {
                    var result = Master.Database.ValidateServer();

                    if (!result)
                        return result;

                    var isId = int.TryParse(accountNameOrId, out var id);

                    return await Utilities.Task.Run(() =>
                    {
                        try
                        {
                            if (!isId)
                                result = Master.Config.Names.Validate(ref accountNameOrId);
                            else
                                result = Result.Success;

                            if (!result)
                                return result;

                            var account = (Account)null;

                            // TODO: Error
                            //var dbContext = Master.Scope.GetService<MasterDbContext>();
                            var dbContext = new MasterDbContext();
                            {
                                if (isId)
                                    account = dbContext.Accounts.SingleOrDefault(acc => acc.Id == id);
                                else
                                    account = dbContext.Accounts.SingleOrDefault(acc => acc.Name == accountNameOrId);

                                if (account == null)
                                    return new Result(ResultCode.AccountNotExist);

                                accountNameOrId = account.Name;

                                var srpAccount = new SrpAccount(newName, password, Master.Config.Srp.InternalConfig);

                                account.Name = srpAccount.I;

                                /*
                                account.SRP_s = srpAccount.s;
                                account.SRP_v = srpAccount.v;
                                */

                                dbContext.SaveChanges();
                            }

                            return new Result(comment: string.Format("The new name for account {0} is {1}.", accountNameOrId, account.Name));
                        }
                        catch (Exception ex)
                        {
                            return new Result(ResultCode.Exception, exception: ex);
                        }
                    }).ConfigureAwait(false);
                }

                public async Task<Result> ChangePasswordAsync(string accountNameOrId, string insecurePassword)
                {
                    var secureString = (SecureString)null;

                    unsafe
                    {
                        fixed (char* chars = insecurePassword)
                            secureString = new SecureString(chars, insecurePassword.Length);
                    }

                    secureString.MakeReadOnly();

                    return await ChangePasswordAsync(accountNameOrId, secureString).ConfigureAwait(false);
                }

                public async Task<Result> ChangePasswordAsync(string accountNameOrId, SecureString password)
                {
                    var result = Master.Database.ValidateServer();

                    if (!result)
                        return result;

                    var isId = int.TryParse(accountNameOrId, out var id);

                    return await Utilities.Task.Run(() =>
                    {
                        try
                        {
                            if (!isId)
                                result = Master.Config.Names.Validate(ref accountNameOrId);
                            else
                                result = Result.Success;

                            if (!result)
                                return result;

                            var account = (Account)null;

                            //var dbContext = Master.Scope.GetService<MasterDbContext>();
                            var dbContext = new MasterDbContext(); // TODO: Error
                            {
                                if (isId)
                                    account = dbContext.Accounts.SingleOrDefault(acc => acc.Id == id);
                                else
                                    account = dbContext.Accounts.SingleOrDefault(acc => acc.Name == accountNameOrId);

                                if (account == null)
                                    return new Result(ResultCode.AccountNotExist);

                                var srpAccount = new SrpAccount(account.Name, password, Master.Config.Srp.InternalConfig);

                                /*
                                account.SRP_s = srpAccount.s;
                                account.SRP_v = srpAccount.v;
                                */

                                dbContext.SaveChanges();
                            }

                            Master.Events.Post(new Events.Server.AccountPasswordChangedEventArgs(Node, account, password));

                            return new Result(comment: string.Format("The password for account {0} was changed.", account.Name));
                        }
                        catch (Exception ex)
                        {
                            return new Result(ResultCode.Exception, exception: ex);
                        }
                    }).ConfigureAwait(false);
                }

                public async Task<Result> ChangeSecurityAsync(string accountNameOrId, int security)
                {
                    var result = Master.Database.ValidateServer();

                    if (!result)
                        return result;

                    var isId = int.TryParse(accountNameOrId, out var id);
                    var previousSecurity = default(int);

                    return await Utilities.Task.Run(() =>
                    {
                        try
                        {
                            if (!isId)
                                result = Master.Config.Names.Validate(ref accountNameOrId);
                            else
                                result = Result.Success;

                            if (!result)
                                return result;

                            var account = (Account)null;

                            //var dbContext = Master.Scope.GetService<MasterDbContext>();
                            var dbContext = new MasterDbContext(); // TODO: Error
                            {
                                if (isId)
                                    account = dbContext.Accounts.SingleOrDefault(acc => acc.Id == id);
                                else
                                    account = dbContext.Accounts.SingleOrDefault(acc => acc.Name == accountNameOrId);

                                if (account == null)
                                    return new Result(ResultCode.AccountNotExist);

                                previousSecurity = account.SecurityLevel;
                                account.SecurityLevel = (byte)security;
                                dbContext.SaveChanges();
                            }

                            Master.Events.Post(new Events.Server.AccountSecurityChangedEventArgs(Node, account, previousSecurity));

                            return new Result(comment: string.Format("Security level of account {0} set to {1}.", account.Name, account.SecurityLevel));
                        }
                        catch (Exception ex)
                        {
                            return new Result(ResultCode.Exception, exception: ex);
                        }
                    }).ConfigureAwait(false);
                }

                //public async Task<Result> UpdateProfileAsync(string accountNameOrId, Profile profile, ClientAccountProfileUpdateMode caProfileUpdateMode = ClientAccountProfileUpdateMode.IgnoreNulls)
                //{
                //    var result = Master.Database.ValidateServer();

                //    if (!result)
                //        return result;

                //    result = Utilities.Models.Validate(Master, profile);

                //    if (!result)
                //        return result;

                //    var isId = int.TryParse(accountNameOrId, out var id);
                //    var previousProfile = (Profile)null;

                //    return await Utilities.Task.Run(() =>
                //    {
                //        try
                //        {
                //            if (!isId)
                //                result = Master.Config.Names.Validate(ref accountNameOrId);
                //            else
                //                result = Result.Success;

                //            if (!result)
                //                return result;

                //            var account = (Account)null;

                //            var dbContext = Master.Scope.GetService<MasterDbContext>();
                //            {
                //                if (isId)
                //                    account = dbContext.Accounts.SingleOrDefault(acc => acc.Id == id);
                //                else
                //                    account = dbContext.Accounts.SingleOrDefault(acc => acc.Name == accountNameOrId);

                //                if (account == null)
                //                    return new Result(ResultCode.AccountNotExist);

                //                if (account.Profile == null)
                //                    account.Profile = dbContext.Profiles.SingleOrDefault(acc => acc.AccountId == account.Id);

                //                previousProfile = account.Profile;

                //                account.Profile = profile;
                //                account.Profile.AccountId = account.Id;

                //                if (caProfileUpdateMode == ClientAccountProfileUpdateMode.IgnoreNulls)
                //                {
                //                    foreach (var propInfo in profile.GetType().GetProperties())
                //                    {
                //                        var value = propInfo.GetValue(profile, null);

                //                        if (value is string)
                //                            value = string.IsNullOrEmpty(value as string) ? null : value;

                //                        if (value == null)
                //                        {
                //                            var previousValue = propInfo.GetValue(previousProfile, null);

                //                            if (previousValue != null)
                //                                propInfo.SetValue(account.Profile, previousValue, null);
                //                            else
                //                                propInfo.SetValue(account.Profile, null, null);
                //                        }
                //                    }
                //                }

                //                dbContext.SaveChanges();
                //            }

                //            Master.Events.Post(new Events.Server.AccountProfileUpdatedEventArgs(Node, account, previousProfile));

                //            return new Result(comment: string.Format("Contact information updated for account {0}.", account.Name));
                //        }
                //        catch (Exception ex)
                //        {
                //            return new Result(ResultCode.Exception, exception: ex);
                //        }
                //    }).ConfigureAwait(false);
                //}

                /*
                internal async Task<Result> ResetAsync(Models.AccountReset reset)
                {
                    var result = Master.Database.ValidateServer();

                    if (!result)
                        return result;

                    result = reset.Validate();

                    if (!result)
                        return result;

                    return await Utilities.Task.Run(async () =>
                    {
                        try
                        {
                            var account = (Account)null;

                            var dbContext = Master.Database.CreateMasterDbContext();
                            {
                                account = dbContext.Accounts.SingleOrDefault(acc => acc.Profile.Email == reset.Email);

                                if (account == null)
                                    return new Result(ResultCode.AccountNotExist);

                                // TODO: How to do transactions in EF 7 ??
                                // var transaction = dbContext.Database.BeginTransaction();

                                account.Reset = reset;

                                dbContext.SaveChanges();

                                var accountResettingEventArgs = Master.Events.Post(new Events.Server.AccountResettingEventArgs(Node, account));

                                await accountResettingEventArgs.ConfigureAwait(false);

                                if (accountResettingEventArgs.Cancel)
                                {
                                    //transaction.Rollback();
                                    return new Result(ResultCode.AccountCreationCanceled, accountResettingEventArgs.CancelReason);
                                }
                                else
                                {
                                    if (account.Reset.Password != null && account.Reset.Password.Length > 0)
                                    {
                                        var srpAccount = new SrpAccount(account.Name, account.Reset.Password, Master.Config.Srp.InternalConfig);

                                        account.Reset.SRP_s = srpAccount.s;
                                        account.Reset.SRP_v = srpAccount.v;
                                    }

                                    dbContext.SaveChanges();
                                    //transaction.Commit();
                                }
                            }

                            await Master.Events.Post(new Events.Server.AccountResetEventArgs(Node, account)).ConfigureAwait(false);

                            return new Result(comment: string.Format("Account reset: {0}", account.Name));
                        }
                        catch (Exception ex)
                        {
                            return new Result(ResultCode.Exception, exception: ex);
                        }
                    }).ConfigureAwait(false);
                }
                */

                //internal Task<Result> CreateAsync(Account account) => CreateAsync(account, null, null, default(CancellationToken));
                //internal Task<Result> CreateAsync(Account account, CancellationToken cancellationToken) => CreateAsync(account, null, null, cancellationToken);
                //internal Task<Result> CreateAsync(Account account, Serializer customData) => CreateAsync(account, null, customData, default(CancellationToken));
                //internal Task<Result> CreateAsync(Account account, Connection rootConnection) => CreateAsync(account, rootConnection, null, default(CancellationToken));
                //internal Task<Result> CreateAsync(Account account, Serializer customData, CancellationToken cancellationToken) => CreateAsync(account, null, customData, cancellationToken);
                //internal Task<Result> CreateAsync(Account account, Connection rootConnection, CancellationToken cancellationToken) => CreateAsync(account, rootConnection, null, cancellationToken);
                //internal Task<Result> CreateAsync(Account account, Connection rootConnection, Serializer customData) => CreateAsync(account, rootConnection, customData, default(CancellationToken));

                //internal async Task<Result> CreateAsync(Account account, Connection rootConnection, Serializer customData, CancellationToken cancellationToken)
                //{
                //    try
                //    {
                //        var result = Result.Success;

                //        if (!(result = Master.Database.ValidateServer()))
                //            return result;

                //        if (result = Utilities.Models.Validate(Master, account))
                //            return result;

                //        try
                //        {
                //            if (!PreviewNames.TryAdd(account.Name, account))
                //                return new Result(ResultCode.AccountNameTaken);

                //            //account.NamePreview = true;

                //            if (!PreviewHashes.TryAdd(account.GetHashCode(), account))
                //                return new Result(ResultCode.AccountHashTaken);

                //            //account.HashPreview = true;

                //            if (account.Profile.Email != null)
                //                if (!PreviewEmails.TryAdd(account.Profile.Email, account))
                //                    return new Result(ResultCode.AccountEmailTaken);

                //            //account.EmailPreview = true;

                //            var dbContext = Master.Scope.GetService<MasterDbContext>();
                //            {
                //                var previousAccount = await dbContext.Accounts.SingleOrDefaultAsync(p => p.Name == account.Name).ConfigureAwait(false);

                //                /*
                //                if (previousAccount != null)
                //                    if (previousAccount.ExpirationHours != null)
                //                        if (previousAccount.ExpirationHours < (DateTime.Now - previousAccount.JoinDate).TotalHours)
                //                        {
                //                            if (!(result = await DeleteAsync(previousAccount.Id.ToString()).ConfigureAwait(false)))
                //                                return result;

                //                            previousAccount = null;
                //                        }
                //                */

                //                if (previousAccount != null)
                //                    return new Result(ResultCode.AccountNameTaken);

                //                if (default(Account) != await dbContext.Accounts.AsNoTracking().SingleOrDefaultAsync(p => p.GetHashCode() == account.GetHashCode()).ConfigureAwait(false))
                //                    return new Result(ResultCode.AccountHashTaken);

                //                if (account.Profile.Email != null)
                //                    if (default(Account) != await dbContext.Accounts.AsNoTracking().SingleOrDefaultAsync(p => p.Profile.Email == account.Profile.Email).ConfigureAwait(false))
                //                        return new Result(ResultCode.AccountEmailTaken);

                //                /*
                //                if (account.Password != null && account.Password.Length > 0)
                //                {
                //                    var srpAccount = new SrpAccount(account.Name, account.Password, Master.Config.Srp.InternalConfig);
                //                    account.SRP_s = srpAccount.s;
                //                    account.SRP_v = srpAccount.v;
                //                }
                //                */

                //                account = dbContext.Accounts.Add(account).Entity;

                //                dbContext.Profiles.Add(account.Profile);

                //                var accountCreatingEventArgs = Master.Events.Post(new Events.Server.AccountCreatingEventArgs(Node, account, rootConnection, customData));

                //                await accountCreatingEventArgs.ConfigureAwait(false);

                //                if (accountCreatingEventArgs.Cancel)
                //                    return new Result(ResultCode.AccountCreationCanceled, accountCreatingEventArgs.CancelReason);

                //                //if (account.Password != null && account.Password.Length > 0)
                //                //{
                //                //    var srpAccount = new SrpAccount(account.Name, account.Reset.Password, Master.Config.Srp.InternalConfig);

                //                //    account.SRP_s = srpAccount.s;
                //                //    account.SRP_v = srpAccount.v;
                //                //}

                //                //if (account.Reset. .Code != null && account.ActivationInfo.Code.Length > 0)
                //                /*
                //                {
                //                    var srpActivation = new SrpAccount(account.Name, account.Reset.Password, Master.Config.Srp.InternalConfig);
                //                    account.Reset.SRP_s = srpActivation.s;
                //                    account.Reset.SRP_v = srpActivation.v;

                //                    dbContext.Resets.Add(account.Reset);
                //                }
                //                */

                //                // TODO: Test with a cancellationToken already canceled.
                //                await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                //            }

                //            await Master.Events.Post(new Events.Server.AccountCreatedEventArgs(Node, account, rootConnection, customData)).ConfigureAwait(false);

                //            return new Result(comment: $"Account '{account.Name}' successfully created.");
                //        }
                //        catch (Exception ex)
                //        {
                //            return new Result(ResultCode.Exception, exception: ex);
                //        }
                //    }
                //    finally
                //    {
                //        //if (account.NamePreview)
                //        {
                //            PreviewNames.TryRemove(account.Name, out account);
                //            //account.NamePreview = false;
                //        }

                //        //if (account.HashPreview)
                //        {
                //            PreviewHashes.TryRemove(account.GetHashCode(), out account);
                //            //account.HashPreview = false;
                //        }

                //        //if (account.EmailPreview)
                //        {
                //            PreviewEmails.TryRemove(account.Profile.Email, out account);
                //            //account.EmailPreview = false;
                //        }
                //    }
                //}

                internal Task<Result> DeleteAsync(string accountNameOrId) => DeleteAsync(accountNameOrId, null, default(CancellationToken));
                internal Task<Result> DeleteAsync(string accountNameOrId, CancellationToken cancellationToken) => DeleteAsync(accountNameOrId, cancellationToken);
                //internal Task<Result> DeleteAsync(string accountNameOrId, AccountDeleteBehavior accountDeleteBehavior) => DeleteAsync(accountNameOrId, null, accountDeleteBehavior, default(CancellationToken));
                internal Task<Result> DeleteAsync(string accountNameOrId, Connection rootConnection) => DeleteAsync(accountNameOrId, rootConnection, default(CancellationToken));
                //internal Task<Result> DeleteAsync(string accountNameOrId, AccountDeleteBehavior accountDeleteBehavior, CancellationToken cancellationToken) => DeleteAsync(accountNameOrId, accountDeleteBehavior, cancellationToken);
                //internal Task<Result> DeleteAsync(string accountNameOrId, Connection rootConnection, CancellationToken cancellationToken) => DeleteAsync(accountNameOrId, rootConnection, cancellationToken);
                //internal Task<Result> DeleteAsync(string accountNameOrId, Connection rootConnection, AccountDeleteBehavior accountDeleteBehavior) => DeleteAsync(accountNameOrId, rootConnection, accountDeleteBehavior, default(CancellationToken));

                internal async Task<Result> DeleteAsync(string accountNameOrId, Connection rootConnection, CancellationToken cancellationToken)
                {
                    var result = Result.Success;

                    var isId = int.TryParse(accountNameOrId, out var id);

                    try
                    {
                        if (!isId)
                            if (!(result = Master.Config.Names.Validate(ref accountNameOrId)))
                                return result;

                        var account = (Account)null;

                        //var dbContext = Master.Scope.GetService<MasterDbContext>();
                        var dbContext = new MasterDbContext(); // TODO: Error
                        {
                            //account = await dbContext.Accounts.Include(p => p.Profile).Include(p => p.Reset).SingleOrDefaultAsync(p => isId ? p.Id == id : p.Name == accountNameOrId).ConfigureAwait(false);

                            if (account == null)
                                return new Result(ResultCode.AccountNotExist);
                            //else if (account.Deleted)
                            //    return new Result(ResultCode.AccountAlreadyDeleted);

                            var accountDeleting = Master.Events.Post(new Events.Server.AccountDeletingEventArgs(Node, account, rootConnection));

                            await accountDeleting.ConfigureAwait(false);

                            if (accountDeleting.Cancel)
                                return new Result(ResultCode.AccountDeletionCanceled, accountDeleting.CancelReason);

                            //if (accountDeleting.Behaviour == AccountDeleteBehavior.MarkAsDeleted)
                            //    account.Deleted = true;
                            //else if (accountDeleting.Behaviour == AccountDeleteBehavior.Destroy)
                            //    dbContext.Accounts.Remove(account);

                            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                            await Master.Connections.Find(account.Name)?.DisconnectAsync(new Result(comment: "Your account has been deleted.")).ConfigureAwait(false);

                            await Master.Events.Post(new Events.Server.AccountDeletedEventArgs(Node, account, rootConnection)).ConfigureAwait(false);
                        }

                        return new Result(comment: $"Account '{account.Name}' deleted.");
                    }
                    catch (Exception ex)
                    {
                        return new Result(ResultCode.Exception, exception: ex);
                    }
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
