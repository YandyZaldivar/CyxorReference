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

    public class MasterConnection : Connection
    {
        AccountApiModel account;
        public AccountApiModel Account
        {
            get { return account; }
            internal set
            {
                (account as IDisposable)?.Dispose();

                account = value;
                Name = account.Name;
            }
        }

        public override string Name
        {
            get
            {
                if (account != null)
                    return base.Name = account.Name;

                return base.Name;
            }
            protected set
            {
                if (base.Name != value)
                {
                    base.Name = value;

                    if (account != null)
                    {
                        account.Name = base.Name;
                        HashCode = account.GetHashCode();
                    }
                    else
                        HashCode = Utilities.HashCode.GetFrom(base.Name);
                }
            }
        }

        protected override void Dispose()
        {
            if (!Disposed)
            {
                (account as IDisposable)?.Dispose();
                base.Dispose();
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
