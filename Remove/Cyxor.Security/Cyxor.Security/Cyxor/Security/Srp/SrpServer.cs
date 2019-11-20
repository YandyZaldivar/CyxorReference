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
using System.Globalization;
using System.Security.Cryptography;

namespace Cyxor.Security
{
    using static Utilities;

    public sealed class SrpServer : Srp
    {
        internal override void Internal() { throw new InvalidOperationException(); }

        BigInteger v;
        BigInteger b;

        public new string s => base.s.ToString("X");
        public new string B => base.B.ToString("X");

        public SrpServer(SrpAccount account, SrpConfig config = default(SrpConfig))
            : base(account?.I, config)
        {
            v = BigInteger.Parse(account.v, NumberStyles.HexNumber);
            base.s = BigInteger.Parse(account.s, NumberStyles.HexNumber);

            b = Crypto.Random(RandomNumberGenerator, Config.RandomBitCount);
            base.B = (k * v + BigInteger.ModPow(g, b, N)) % N;

            if (base.s == BigInteger.Zero || base.B == BigInteger.Zero)
                throw new InvalidOperationException();
        }

        public SymmetricAlgorithm Calculate(string clientA)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(SrpServer));
            else if (Computed)
                throw new InvalidOperationException("Values already computed.");

            A = BigInteger.Parse(clientA, NumberStyles.HexNumber) % N;

            if (A == BigInteger.Zero)
                throw new ArgumentException(nameof(clientA));

            u = Crypto.HashPair(A, base.B, N, HashAlgorithm, true, Config.Prime.Length / 2);
            K = BigInteger.ModPow(A * BigInteger.ModPow(v, u, N), b, N);

            K = Crypto.Hash(K, N, HashAlgorithm);

            m = CalculateServerM();

            return GetSymmetricAlgorithm();
        }

        public override bool Verify(string clientM) =>
            CalculateClientM() == BigInteger.Parse(clientM, NumberStyles.HexNumber);
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
