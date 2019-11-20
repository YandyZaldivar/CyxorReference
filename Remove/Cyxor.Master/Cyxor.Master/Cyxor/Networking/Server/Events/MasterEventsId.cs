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
    public partial class Master
    {
        public class MasterEventsId : ServerEventsId
        {
            internal const int DbLoadCompleted = 150;
            internal const int DbLoadProgressChanged = 151;

            internal const int AccountCreating = 160;
            internal const int AccountCreated = 161;
            internal const int AccountResetting = 162;
            internal const int AccountReset = 163;
            internal const int AccountDeleting = 164;
            internal const int AccountDeleted = 165;
            internal const int AccountProfileUpdated = 166;
            internal const int AccountPasswordChanged = 167;
            internal const int AccountSecurityChanged = 168;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
