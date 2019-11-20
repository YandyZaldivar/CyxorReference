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

    public class SrpConfig : IDisposable
    {
        public static readonly SrpConfig Default = new SrpConfig();

        bool ReadOnly;
        internal void MakeReadOnly() => ReadOnly = true;

        SymmetricAlgorithm Algorithm;

        public const string DefaultGenerator = "2";
        public const SrpHashAlgorithm DefaultHashAlgorithm = SrpHashAlgorithm.SHA256;
        public const SrpProtocolVersion DefaultProtocolVersion = SrpProtocolVersion.SRP6a;

#if !NET20
        public const SrpSymmetricAlgorithm DefaultSymmetricAlgorithm = SrpSymmetricAlgorithm.Aes;
#else
        public const SrpSymmetricAlgorithm DefaultSymmetricAlgorithm = SrpSymmetricAlgorithm.Rijndael;
#endif

        public const SrpRandomNumberGenerator DefaultRandomNumberGenerator = SrpRandomNumberGenerator.RNGCSP;

        public const int DefaultSymmetricAlgorithmKeySize = 256;
        public const int DefaultSymmetricAlgorithmBlockSize = 128;
        public const CipherMode DefaultSymmetricAlgorithmCipherMode = CipherMode.CBC;
        public const PaddingMode DefaultSymmetricAlgorithmPaddingMode = PaddingMode.None;

        public const int DefaultRandomBitCount = 512;

        public const string DefaultPrime = DefaultPrime1024;

        public const string DefaultPrime1024 = "EEAF0AB9ADB38DD69C33F80AFA8FC5E86072618775FF3C0B9EA2314C9C256576" +
                                               "D674DF7496EA81D3383B4813D692C6E0E0D5D8E250B98BE48E495C1D6089DAD1" +
                                               "5DC7D7B46154D6B6CE8EF4AD69B15D4982559B297BCF1885C529F566660E57EC" +
                                               "68EDBC3C05726CC02FD4CBF4976EAA9AFD5138FE8376435B9FC61D2FC0EB06E3";

        public const string DefaultPrime2048 = "AC6BDB41324A9A9BF166DE5E1389582FAF72B6651987EE07FC3192943DB56050" + 
                                               "A37329CBB4A099ED8193E0757767A13DD52312AB4B03310DCD7F48A9DA04FD50" +
                                               "E8083969EDB767B0CF6095179A163AB3661A05FBD5FAAAE82918A9962F0B93B8" +
                                               "55F97993EC975EEAA80D740ADBF4FF747359D041D5C33EA71D281E446B14773B" +
                                               "CA97B43A23FB801676BD207A436C6481F1D2B9078717461A5B9D32E688F87748" +
                                               "544523B524B0D57D5EA77A2775D2ECFA032CFBDBF52FB3786160279004E57AE6" +
                                               "AF874E7303CE53299CCC041C7BC308D82A5698F3A8D0C38271AE35F8E9DBFBB6" +
                                               "94B5C803D89F7AE435DE236D525F54759B65E372FCD68EF20FA7111F9E4AFF73";

        public BigInteger N { get; private set; }
        public BigInteger g { get; private set; }

        int randomBitCount = Math.Min(DefaultRandomBitCount, DefaultPrime.Length * 2);
        public int RandomBitCount
        {
            get { return randomBitCount; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                randomBitCount = value;
            }
        }

        SrpProtocolVersion protocolVersion = SrpProtocolVersion.SRP6a;
        public SrpProtocolVersion ProtocolVersion
        {
            get { return protocolVersion; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                protocolVersion = value;
            }
        }

        public SrpConfig()
        {
            N = BigInteger.Parse(prime, NumberStyles.HexNumber);

            if (N.Sign == -1)
                N = BigInteger.Parse("0" + prime, NumberStyles.HexNumber);

            g = BigInteger.Parse(generator, NumberStyles.HexNumber);

            if (g.Sign == -1)
                g = BigInteger.Parse("0" + generator, NumberStyles.HexNumber);

            Algorithm = Crypto.CreateSymmetricAlgorithm(symmetricAlgorithm);

            symmetricAlgorithmKeySize = Algorithm.KeySize;
            symmetricAlgorithmBlockSize = Algorithm.BlockSize;
        }

        string generator = DefaultGenerator;
        public string Generator
        {
            get { return generator; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                else if (value == string.Empty)
                    throw new ArgumentException();

                g = BigInteger.Parse(value, NumberStyles.HexNumber);

                if (g.Sign == -1)
                    g = BigInteger.Parse("0" + value, NumberStyles.HexNumber);

                generator = value;
            }
        }

        string prime = DefaultPrime;
        public string Prime
        {
            get { return prime; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                else if (value == string.Empty || value.Length < 64 || value.Length % 2 != 0)
                    throw new FormatException();

                N = BigInteger.Parse(value, NumberStyles.HexNumber);

                if (N.Sign == -1)
                    N = BigInteger.Parse("0" + value, NumberStyles.HexNumber);

                prime = value;

                randomBitCount = Math.Min(DefaultRandomBitCount, prime.Length * 2);
            }
        }

        SrpRandomNumberGenerator randomNumberGenerator = SrpRandomNumberGenerator.RNGCSP;
        public SrpRandomNumberGenerator RandomNumberGenerator
        {
            get { return randomNumberGenerator; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                randomNumberGenerator = value;
            }
        }

        SrpHashAlgorithm hashAlgorithm = SrpHashAlgorithm.SHA256;
        public SrpHashAlgorithm HashAlgorithm
        {
            get { return hashAlgorithm; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                hashAlgorithm = value;
            }
        }

        SrpSymmetricAlgorithm symmetricAlgorithm = DefaultSymmetricAlgorithm;
        public SrpSymmetricAlgorithm SymmetricAlgorithm
        {
            get { return symmetricAlgorithm; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                if (symmetricAlgorithm == value)
                    return;
#if !NET20 && !NET35
                else
                    Algorithm.Dispose();
#endif

                Algorithm = Crypto.CreateSymmetricAlgorithm(symmetricAlgorithm = value);

                try
                {
                    Algorithm.KeySize = SymmetricAlgorithmKeySize;
                    Algorithm.BlockSize = SymmetricAlgorithmBlockSize;
                    Algorithm.Mode = SymmetricAlgorithmCipherMode;
                    Algorithm.Padding = SymmetricAlgorithmPaddingMode;
                }
                catch
                {
                    symmetricAlgorithmKeySize = Algorithm.KeySize;
                    symmetricAlgorithmBlockSize = Algorithm.BlockSize;
                    SymmetricAlgorithmCipherMode = Algorithm.Mode;
                    SymmetricAlgorithmPaddingMode = Algorithm.Padding;
                }
            }
        }

        int symmetricAlgorithmKeySize = DefaultSymmetricAlgorithmKeySize;
        public int SymmetricAlgorithmKeySize
        {
            get { return symmetricAlgorithmKeySize; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                if (symmetricAlgorithmKeySize == value)
                    return;

                Algorithm.KeySize = symmetricAlgorithmKeySize = value;
            }
        }

        public KeySizes[] GetSymmetricAlgorithmLegalKeySizes => Algorithm.LegalKeySizes;

        int symmetricAlgorithmBlockSize = DefaultSymmetricAlgorithmBlockSize;
        public int SymmetricAlgorithmBlockSize
        {
            get { return symmetricAlgorithmBlockSize; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                if (symmetricAlgorithmBlockSize == value)
                    return;

                Algorithm.BlockSize = symmetricAlgorithmBlockSize = value;
            }
        }

        public KeySizes[] GetSymmetricAlgorithmLegalBlockSizes => Algorithm.LegalBlockSizes;

        CipherMode cipherMode = DefaultSymmetricAlgorithmCipherMode;
        public CipherMode SymmetricAlgorithmCipherMode
        {
            get { return cipherMode; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                if (cipherMode == value)
                    return;

                Algorithm.Mode = cipherMode = value;
            }
        }

        PaddingMode paddingMode = DefaultSymmetricAlgorithmPaddingMode;
        public PaddingMode SymmetricAlgorithmPaddingMode
        {
            get { return paddingMode; }
            set
            {
                if (ReadOnly)
                    throw new InvalidOperationException("This config instance is readonly.");

                if (paddingMode == value)
                    return;

                Algorithm.Padding = paddingMode = value;
            }
        }

        void IDisposable.Dispose()
        {
#if !NET20 && !NET35
            Algorithm?.Dispose();
#endif
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
