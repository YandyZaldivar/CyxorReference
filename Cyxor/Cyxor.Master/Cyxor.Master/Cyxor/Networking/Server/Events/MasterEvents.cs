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
    using Events.Server;

    public partial class Master
    {
        public class MasterEvents : ServerEvents
        {
            Master MasterServer;

            protected internal MasterEvents(Master masterServer) : base(masterServer)
            {
                MasterServer = masterServer;
            }

            public event EventHandler<DbLoadCompletedEventArgs> DbLoadCompleted;
            public event EventHandler<DbLoadProgressChangedEventArgs> DbLoadProgressChanged;

            public event EventHandler<AccountResetEventArgs> AccountReset;
            public event EventHandler<AccountCreatedEventArgs> AccountCreated;
            public event EventHandler<AccountDeletedEventArgs> AccountDeleted;
            public event EventHandler<AccountDeletingEventArgs> AccountDeleting;
            public event EventHandler<AccountCreatingEventArgs> AccountCreating;
            public event EventHandler<AccountResettingEventArgs> AccountResetting;
            public event EventHandler<AccountProfileUpdatedEventArgs> AccountProfileUpdated;
            public event EventHandler<AccountPasswordChangedEventArgs> AccountPasswordChanged;
            public event EventHandler<AccountSecurityChangedEventArgs> AccountSecurityChanged;

            public override void RaiseEvent<TActionEventArgs>(TActionEventArgs e, bool detached = false)
            {
                if (!detached && !Node.Config.OverrideEvents)
                    return;

                switch (e.EventId)
                {
                    case MasterEventsId.DbLoadCompleted: RaiseEvent(DbLoadCompleted, e as DbLoadCompletedEventArgs); break;
                    case MasterEventsId.DbLoadProgressChanged: RaiseEvent(DbLoadProgressChanged, e as DbLoadProgressChangedEventArgs); break;

                    case MasterEventsId.AccountCreating: RaiseEvent(AccountCreating, e as AccountCreatingEventArgs); break;
                    case MasterEventsId.AccountCreated: RaiseEvent(AccountCreated, e as AccountCreatedEventArgs); break;
                    case MasterEventsId.AccountResetting: RaiseEvent(AccountResetting, e as AccountResettingEventArgs); break;
                    case MasterEventsId.AccountReset: RaiseEvent(AccountReset, e as AccountResetEventArgs); break;
                    case MasterEventsId.AccountDeleting: RaiseEvent(AccountDeleting, e as AccountDeletingEventArgs); break;
                    case MasterEventsId.AccountDeleted: RaiseEvent(AccountDeleted, e as AccountDeletedEventArgs); break;
                    case MasterEventsId.AccountProfileUpdated: RaiseEvent(AccountProfileUpdated, e as AccountProfileUpdatedEventArgs); break;
                    case MasterEventsId.AccountPasswordChanged: RaiseEvent(AccountPasswordChanged, e as AccountPasswordChangedEventArgs); break;
                    case MasterEventsId.AccountSecurityChanged: RaiseEvent(AccountSecurityChanged, e as AccountSecurityChangedEventArgs); break;

                    default: base.RaiseEvent(e, detached); break;
                }
            }

            public override void OnEvent<TActionEventArgs>(TActionEventArgs e)
            {
                switch (e.EventId)
                {
                    case MasterEventsId.DbLoadCompleted: MasterServer.OnDbLoadCompleted(e as DbLoadCompletedEventArgs); break;
                    case MasterEventsId.DbLoadProgressChanged: MasterServer.OnDbLoadProgressChanged(e as DbLoadProgressChangedEventArgs); break;

                    case MasterEventsId.AccountReset: MasterServer.OnAccountReset(e as AccountResetEventArgs); break;
                    case MasterEventsId.AccountCreated: MasterServer.OnAccountCreated(e as AccountCreatedEventArgs); break;
                    case MasterEventsId.AccountDeleted: MasterServer.OnAccountDeleted(e as AccountDeletedEventArgs); break;
                    case MasterEventsId.AccountCreating: MasterServer.OnAccountCreating(e as AccountCreatingEventArgs); break;
                    case MasterEventsId.AccountDeleting: MasterServer.OnAccountDeleting(e as AccountDeletingEventArgs); break;
                    case MasterEventsId.AccountResetting: MasterServer.OnAccountResetting(e as AccountResettingEventArgs); break;
                    case MasterEventsId.AccountProfileUpdated: MasterServer.OnAccountProfileUpdated(e as AccountProfileUpdatedEventArgs); break;
                    case MasterEventsId.AccountPasswordChanged: MasterServer.OnAccountPasswordChanged(e as AccountPasswordChangedEventArgs); break;
                    case MasterEventsId.AccountSecurityChanged: MasterServer.OnAccountSecurityChanged(e as AccountSecurityChangedEventArgs); break;

                    default: base.OnEvent(e); break;
                }
            }

            public override bool IsSubscribed(int eventId)
            {
                switch (eventId)
                {
                    case MasterEventsId.DbLoadCompleted: return DbLoadCompleted != null;
                    case MasterEventsId.DbLoadProgressChanged: return DbLoadProgressChanged != null;

                    case MasterEventsId.AccountReset: return AccountReset != null;
                    case MasterEventsId.AccountDeleted: return AccountDeleted != null;
                    case MasterEventsId.AccountCreated: return AccountCreated != null;
                    case MasterEventsId.AccountDeleting: return AccountDeleting != null;
                    case MasterEventsId.AccountCreating: return AccountCreating != null;
                    case MasterEventsId.AccountResetting: return AccountResetting != null;
                    case MasterEventsId.AccountProfileUpdated: return AccountProfileUpdated != null;
                    case MasterEventsId.AccountPasswordChanged: return AccountPasswordChanged != null;
                    case MasterEventsId.AccountSecurityChanged: return AccountSecurityChanged != null;

                    default: return base.IsSubscribed(eventId);
                }
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
