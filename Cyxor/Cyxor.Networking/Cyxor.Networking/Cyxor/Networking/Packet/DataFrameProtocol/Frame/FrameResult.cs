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

namespace Cyxor.Networking
{
    /// <summary>
    /// The result after reading a WebSocket protocol frame.
    /// </summary>
    class FrameResult
    {
        /// <summary>
        /// The frame is final, either Fin is present or it contains a control Opcode.
        /// </summary>
        public const int Ok = 0;

        /// <summary>
        /// There is an error in the frame, mostly related to a bad combination of the
        /// protocol structure values such as not present masking in clients or masking
        /// present in server and also when Fin is combined with an incorrect control Opcode.
        /// </summary>
        public const int Error = -1;

        /// <summary>
        /// The frame header data is not completed, need to read more bytes.
        /// </summary>
        public const int Header = -2;

        /// <summary>
        /// The frame is completed but it expects a continuation frame.
        /// </summary>
        public const int Partial = -3;
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
