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
using System.Security.Cryptography;

namespace Cyxor.Networking
{
    public sealed class LinkNoob : IDisposable
    {
        bool disposed;

        public bool Authenticated { get; set; }
        public Aes Aes { get; private set; }
        //public RSACryptoServiceProvider Rsa { get; private set; }

        internal LinkNoob()
        {
            Aes = Aes.Create();
            Aes.Mode = CipherMode.CBC;
            Aes.Padding = PaddingMode.PKCS7;

            //Rsa = new RSACryptoServiceProvider();
        }

        internal void Dispose()
        {
            if (!disposed)
            {
#if !(NET35)
                Aes.Dispose();
                //Rsa.Dispose();
#endif

                disposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        internal void Reset()
        {
            Authenticated = false;
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
