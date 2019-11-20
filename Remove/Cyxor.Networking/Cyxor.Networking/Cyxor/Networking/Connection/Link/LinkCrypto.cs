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
using System.IO;
using System.Security.Cryptography;

namespace Cyxor.Networking
{
    using Extensions;
    using Serialization;

    sealed partial class Link
    {
        internal sealed class LinkCrypto : LinkProperty, IDisposable
        {
            bool Disposed;

            SymmetricAlgorithm algorithm;
            internal SymmetricAlgorithm Algorithm
            {
                get
                {
                    if (Disposed)
                        throw new ObjectDisposedException(nameof(LinkCrypto));

                    return algorithm;
                }
                set
                {
                    if (Disposed)
                        throw new ObjectDisposedException(nameof(LinkCrypto));

                    if (algorithm != value && algorithm != null)
                        Reset();

                    algorithm = value;

                    if (algorithm != null)
                    {
                        //Encryptor = algorithm.CreateEncryptor();
                        //Decryptor = algorithm.CreateDecryptor();

                        EncryptorMemoryStream = new MemoryStream();
                        DecryptorMemoryStream = new MemoryStream();

                        EncryptorStream = new CryptoStream(EncryptorMemoryStream, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
                        DecryptorStream = new CryptoStream(DecryptorMemoryStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
                    }
                }
            }

            //ICryptoTransform Encryptor;
            //internal ICryptoTransform Encryptor => encryptor ?? (encryptor = Algorithm?.CreateEncryptor());

            //ICryptoTransform Decryptor;
            //internal ICryptoTransform Decryptor => decryptor ?? (decryptor = Algorithm?.CreateDecryptor());

            CryptoStream EncryptorStream;
            CryptoStream DecryptorStream;

            MemoryStream EncryptorMemoryStream;
            MemoryStream DecryptorMemoryStream;

            internal LinkCrypto(Link link) : base(link)
            {

            }

            //internal void Encrypt(byte[] buffer, int offset, int count)
            //{
            //    Encryptor.
            //}

            internal ArraySegment<byte> Encrypt(ArraySegment<byte> arraySegment)
                => Encrypt(arraySegment.Array, arraySegment.Offset, arraySegment.Count);

            internal ArraySegment<byte> Encrypt(byte[] buffer, int offset, int count)
            {
                EncryptorStream.Write(buffer, offset, count);
                var encryptedBuffer = EncryptorMemoryStream.GetBuffer();
                var encryptedArray = new ArraySegment<byte>(encryptedBuffer, 0, (int)EncryptorMemoryStream.Position);
                if (EncryptorMemoryStream.Capacity > Node.Config.IOBufferSize)
                    EncryptorMemoryStream.Capacity = Node.Config.IOBufferSize;
                EncryptorMemoryStream.SetLength(0);
                EncryptorMemoryStream.Position = 0;
                return encryptedArray;
            }

            internal Serializer Decrypt(byte[] buffer, int offset, int count)
            {
                DecryptorMemoryStream.Write(buffer, offset, count);
                var decryptedSerializer = Node.Pools.PopBuffer();
                decryptedSerializer.EnsureCapacity(count);
                DecryptorMemoryStream.Position = 0;
                var bytesRead = 0;
                do { bytesRead += offset = DecryptorStream.Read(decryptedSerializer.Buffer, 0, count); }
                while (offset != 0);
                //while (DecryptorMemoryStream.Position != count);
                decryptedSerializer.SetLength(bytesRead);
                decryptedSerializer.Position = 0;
                DecryptorMemoryStream.Position = 0;
                return decryptedSerializer;
            }

            internal void Reset()
            {
                //encryptor?.Dispose();
                //decryptor?.Dispose();

#if !(NET35)
                Algorithm?.Dispose();
#endif

                EncryptorStream?.Dispose();
                DecryptorStream?.Dispose();

                //encryptor = null;
                //decryptor = null;
                algorithm = null;
            }

            internal void Dispose()
            {
                if (!Disposed)
                {
                    Reset();
                    Disposed = true;
                }
            }

            void IDisposable.Dispose() => Dispose();
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
