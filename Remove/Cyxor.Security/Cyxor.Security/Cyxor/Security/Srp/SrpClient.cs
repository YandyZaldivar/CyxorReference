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
using System.Globalization;
using System.Security.Cryptography;

namespace Cyxor.Security
{
    using static Utilities;

    public sealed class SrpClient : Srp
    {
        internal override void Internal() => throw new InvalidOperationException();

        SecureString p;
        BigInteger a;
        public new string A => base.A.ToString("X");

        public SrpClient(string username, SecureString password, SrpConfig config = default(SrpConfig))
            : base(username, config)
        {
            p = password;
            a = Crypto.Random(RandomNumberGenerator, Config.RandomBitCount);
            base.A = BigInteger.ModPow(g, a, N);
        }

        public SymmetricAlgorithm Calculate(string serverSalt, string serverB)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(SrpServer));
            else if (Computed)
                throw new InvalidOperationException("Values already computed.");

            s = BigInteger.Parse(serverSalt, NumberStyles.HexNumber);
            B = BigInteger.Parse(serverB, NumberStyles.HexNumber) % N;

            if (B == BigInteger.Zero)
                throw new ArgumentException(nameof(serverB));

            var b = Crypto.GetLoginBytes(I, p);

            u = Crypto.HashPair(base.A, B, N, HashAlgorithm, true, Config.Prime.Length / 2);
            var x = Crypto.HashPair(s, new BigInteger(b), N, HashAlgorithm);

            Array.Clear(b, 0, b.Length);

            var tmp = (B - k * BigInteger.ModPow(g, x, N)) % N;
            tmp = tmp.Sign == -1 ? tmp + N : tmp;
            K = BigInteger.ModPow(tmp, a + u * x, N);

            K = Crypto.Hash(K, N, HashAlgorithm);

            m = CalculateClientM();

            return GetSymmetricAlgorithm();
        }

        public override bool Verify(string serverM) =>
            CalculateServerM() == BigInteger.Parse(serverM, NumberStyles.HexNumber);
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
