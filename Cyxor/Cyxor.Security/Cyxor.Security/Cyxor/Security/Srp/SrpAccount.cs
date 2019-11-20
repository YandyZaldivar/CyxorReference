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
using System.Numerics;
using System.Security;
using System.Security.Cryptography;

namespace Cyxor.Security
{
    using static Utilities;

    public sealed class SrpAccount
    {
        public string I { get; }

#pragma warning disable IDE1006
        public string s { get; }
        public string v { get; }
#pragma warning restore IDE1006

        public SrpAccount(string I, string s, string v)
        {
#pragma warning disable IDE0013 // Name can be simplified
            if (String.IsNullOrWhiteSpace(I))
                throw new ArgumentNullException(nameof(I));

            if (String.IsNullOrWhiteSpace(s))
                throw new ArgumentNullException(nameof(s));

            if (String.IsNullOrWhiteSpace(s))
                throw new ArgumentNullException(nameof(v));
#pragma warning restore IDE0013 // Name can be simplified

            this.I = I;
            this.s = s;
            this.v = v;
        }

        public SrpAccount(string username, SecureString password, SrpConfig config = default(SrpConfig))
        {
#pragma warning disable IDE0013 // Name can be simplified
            if (String.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));
#pragma warning restore IDE0013 // Name can be simplified

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (password.Length < 1)
                throw new ArgumentException(nameof(password));

            config = config ?? SrpConfig.Default;

            var hash = default(HashAlgorithm);
            var rng = default(RandomNumberGenerator);

            switch (config.RandomNumberGenerator)
            {
                case SrpRandomNumberGenerator.RNGCSP: rng = RandomNumberGenerator.Create(); break;
            }

            switch (config.HashAlgorithm)
            {
                case SrpHashAlgorithm.MD5: hash = MD5.Create(); break;
                case SrpHashAlgorithm.SHA1: hash = SHA1.Create(); break;
                case SrpHashAlgorithm.SHA256: hash = SHA256.Create(); break;
                case SrpHashAlgorithm.SHA384: hash = SHA384.Create(); break;
                case SrpHashAlgorithm.SHA512: hash = SHA512.Create(); break;
            }

            var b = Crypto.GetLoginBytes(username, password);

            var s = Crypto.Random(rng, bitCount: 256);
            var x = Crypto.HashPair(s, new BigInteger(b), config.N, hash);
            var v = BigInteger.ModPow(config.g, x, config.N);

            Array.Clear(b, 0, b.Length);

#if !NET20 && !NET35
            rng.Dispose();
            hash.Dispose();
#endif

            this.I = username;
            this.s = s.ToString("X");
            this.v = v.ToString("X");
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
