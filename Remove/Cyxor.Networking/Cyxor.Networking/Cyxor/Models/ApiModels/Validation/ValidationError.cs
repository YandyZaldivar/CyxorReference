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

using System.Collections.Generic;

namespace Cyxor.Models
{
    using Serialization;

    /// <summary>
    /// Represents a container for the errors of a validation request.
    /// </summary>
    public sealed class ValidationError
    {
        /// <summary>
        /// Gets the collection of member names that indicate which fields have validation errors.
        /// </summary>
        /// <returns>
        /// The collection of member names that indicate which fields have validation errors.
        /// </returns>
        public IEnumerable<string> MemberNames { get; set; }

        /// <summary>
        /// Gets the error message for the validation.
        /// </summary>
        /// <returns>
        /// The error message for the validation.
        /// </returns>
        public string ErrorMessage { get; set; }

        public void Serialize(Serializer serializer)
        {
            serializer.Serialize(ErrorMessage);
            serializer.Serialize(MemberNames);
        }

        public void Deserialize(Serializer serializer)
        {
            ErrorMessage = serializer.DeserializeString();
            MemberNames = serializer.DeserializeIEnumerable<string>();
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
