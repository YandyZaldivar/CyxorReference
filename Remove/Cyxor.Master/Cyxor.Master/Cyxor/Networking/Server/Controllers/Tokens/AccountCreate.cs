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
    using Models;

    //public partial class Master
    //{
    //    public partial class MasterCommands
    //    {
    //        /*
    //        protected virtual MasterCommand AccountCreate { get; } = new MasterCommand
    //        {
    //            Name = "account create",
    //            Security = 0,
    //            MinArguments = 2,
    //            MaxArguments = 3,
    //            Description = "Syntax: .account create $account [$password] [$email]" + Environment.NewLine +
    //                         "Create account and set password and/or email to it. The password or the email are optional " +
    //                         "but not both. A password can only be specified when the command is executed directly on the server " +
    //                         "or when executed by an account with a security level equal or greater than 3.",

    //            Action = (args) =>
    //            {
    //                var master = args.Server as Master;
    //                var masterConnection = args.Connection as MasterConnection;

    //                var name = (string)null;
    //                var email = (string)null;
    //                var password = (string)null;
    //                //var accountOrigin = default(OriginValue);

    //                name = args[0];

    //                if (args.Length == 3)
    //                {
    //                    if (masterConnection?.Account?.Security < 3)
    //                        return Utilities.Task.FromResult(new Result(ResultCode.CommandSyntaxError, args.Command.Description));

    //                    if (!Utilities.IsValidEmailFormat(email = args[2]))
    //                        return Utilities.Task.FromResult(new Result(ResultCode.EmailInvalidFormat));

    //                    password = args[1];
    //                }
    //                else if (masterConnection?.Account?.Security < 3)
    //                {
    //                    if (!Utilities.IsValidEmailFormat(email = args[1]))
    //                        return Utilities.Task.FromResult(new Result(ResultCode.EmailInvalidFormat));

    //                    //accountOrigin = OriginValue.Invitation;
    //                }
    //                else if (email == null)
    //                {
    //                    if (Utilities.IsValidEmailFormat(args[1]))
    //                        email = args[1];
    //                    else
    //                        password = args[1];
    //                }

    //                //if (masterConnection?.Account == null)
    //                //    accountOrigin = OriginValue.Server;
    //                //else if (masterConnection.Account.Security >= 3)
    //                //    accountOrigin = OriginValue.Poweruser;

    //                //var newAccount = new Account { Name = name, InsecurePassword = password, AccountOrigin = accountOrigin };
    //                //masterConnection.Account.Profile.Email = email;

    //                var newAccount = new Account { Name = name, PasswordHash = password };

    //                return master.Database.Account.CreateAsync(newAccount, masterConnection);
    //            }
    //        };
    //        */
    //    }
    //}
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
