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
using System.Numerics;
using System.Security.Cryptography;

namespace Cyxor.Security
{
    using static Utilities;

    public abstract class Srp : IDisposable
    {
        internal abstract void Internal();

        protected bool Computed;
        protected bool Disposed;

        public string I { get; }

        protected BigInteger u;
        protected BigInteger s;
        protected BigInteger A;
        protected BigInteger B;
        protected BigInteger K;
        protected BigInteger m;

        protected readonly BigInteger k;

        public BigInteger N => Config.N;
        public string Prime => Config.Prime;

        public BigInteger g => Config.g;
        public string Generator => Config.Generator;

        public string M => m.ToString("X");

        protected HashAlgorithm HashAlgorithm { get; }
        protected RandomNumberGenerator RandomNumberGenerator { get; }

        public SrpConfig Config { get; set; }

        internal Srp(string username, SrpConfig config)
        {
            I = username;

            if (String.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            Config = config == null ? new SrpConfig() : config;
            Config.MakeReadOnly();         

            switch (Config.RandomNumberGenerator)
            {
                case SrpRandomNumberGenerator.RNGCSP: RandomNumberGenerator = RandomNumberGenerator.Create(); break;
            }

            switch (Config.HashAlgorithm)
            {
                case SrpHashAlgorithm.MD5: HashAlgorithm = MD5.Create(); break;
                case SrpHashAlgorithm.SHA1: HashAlgorithm = SHA1.Create(); break;
                case SrpHashAlgorithm.SHA256: HashAlgorithm = SHA256.Create(); break;
                case SrpHashAlgorithm.SHA384: HashAlgorithm = SHA384.Create(); break;
                case SrpHashAlgorithm.SHA512: HashAlgorithm = SHA512.Create(); break;
            }

            k = Crypto.HashPair(N, g, N, HashAlgorithm, padded: true, padLength: Prime.Length / 2);
        }

        public abstract bool Verify(string M);

        protected BigInteger CalculateServerM()
        {
            if (Disposed)
                throw new ObjectDisposedException(this is SrpClient ? nameof(SrpClient) : nameof(SrpServer));

            var bufferA = A.ToByteArray();
            var bufferMC = CalculateClientM().ToByteArray();
            var bufferK = K.ToByteArray();

            var bytes = new byte[bufferA.Length + bufferMC.Length + bufferK.Length];

            Buffer.BlockCopy(bufferA, 0, bytes, 0, bufferA.Length);
            Buffer.BlockCopy(bufferMC, 0, bytes, bufferA.Length, bufferMC.Length);
            Buffer.BlockCopy(bufferK, 0, bytes, bufferA.Length + bufferMC.Length, bufferK.Length);

            return new BigInteger(HashAlgorithm.ComputeHash(bytes));
        }

        protected BigInteger CalculateClientM()
        {
            if (Disposed)
                throw new ObjectDisposedException(this is SrpClient ? nameof(SrpClient) : nameof(SrpServer));

            var HH = Crypto.Hash(N, N, HashAlgorithm);
            var Hg = Crypto.Hash(g, N, HashAlgorithm);
            var HI = Crypto.Hash(new BigInteger(Encoding.UTF8.GetBytes(I)), N, HashAlgorithm);

            var bufferN = (HH ^ Hg).ToByteArray();
            var bufferI = HI.ToByteArray();
            var buffers = s.ToByteArray();
            var bufferA = A.ToByteArray();
            var bufferB = B.ToByteArray();
            var bufferK = K.ToByteArray();

            var offset1 = bufferN.Length;
            var offset2 = offset1 + bufferI.Length;
            var offset3 = offset2 + buffers.Length;
            var offset4 = offset3 + bufferA.Length;
            var offset5 = offset4 + bufferB.Length;

            var bytes = new byte[offset5 + bufferK.Length];

            Buffer.BlockCopy(bufferN, 0, bytes, 0, bufferN.Length);
            Buffer.BlockCopy(bufferI, 0, bytes, offset1, bufferI.Length);
            Buffer.BlockCopy(buffers, 0, bytes, offset2, buffers.Length);
            Buffer.BlockCopy(bufferA, 0, bytes, offset3, bufferA.Length);
            Buffer.BlockCopy(bufferB, 0, bytes, offset4, bufferB.Length);
            Buffer.BlockCopy(bufferK, 0, bytes, offset5, bufferK.Length);

            return new BigInteger(HashAlgorithm.ComputeHash(bytes));
        }

        protected SymmetricAlgorithm GetSymmetricAlgorithm()
        {
            var symmetricAlgorithm = Crypto.CreateSymmetricAlgorithm(Config.SymmetricAlgorithm);

            symmetricAlgorithm.KeySize = Config.SymmetricAlgorithmKeySize;
            symmetricAlgorithm.BlockSize = Config.SymmetricAlgorithmBlockSize;
            symmetricAlgorithm.Mode = Config.SymmetricAlgorithmCipherMode;
            symmetricAlgorithm.Padding = Config.SymmetricAlgorithmPaddingMode;

            var pbkdf2 = new Rfc2898DeriveBytes(K.ToString("X"), u.ToByteArray(), 7);

#if !NET20 && !NET35
            using (pbkdf2)
#endif
            {
                symmetricAlgorithm.Key = pbkdf2.GetBytes(symmetricAlgorithm.KeySize / 8);
                symmetricAlgorithm.IV = pbkdf2.GetBytes(symmetricAlgorithm.BlockSize / 8);
            }

            return symmetricAlgorithm;
        }

        public override string ToString()
        {
            var name = this is SrpClient ? "Client" : "Server";

            var generator = Config.Generator;
            var randomBits = Config.RandomBitCount;
            var primeSize = Config.N.ToByteArray().Length;
            primeSize = (primeSize % 2 != 0 ? primeSize - 1 : primeSize) * 8;
            var protocol = Config.ProtocolVersion;
            var crypto = Config.SymmetricAlgorithm;
            var keySize = Config.SymmetricAlgorithmKeySize;
            var blockSize = Config.SymmetricAlgorithmBlockSize;

            var value = $"{protocol} {name} {{ g = {generator}, N = {primeSize} bits, Rng: {randomBits} bits, " +
                        $"{Config.HashAlgorithm}, {crypto} {{ KeySize = {keySize}, BlockSize = {blockSize} }} }}";

            return value;
        }

        public void Dispose()
        {
            if (Disposed)
                return;

#if !NET20 && !NET35
            HashAlgorithm.Dispose();
            RandomNumberGenerator.Dispose();
#endif
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
