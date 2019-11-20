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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace Cyxor.Models
{
    using Networking;
    using Serialization;
    using Networking.Config;

    //[Model("authenticate", Description = "TODO:")]
    public sealed class AuthRequest : ISerializable, IValidatable
    {
        [Required]
        public string I { get; set; }

        [Required]
        public string A { get; set; }

        public AuthenticationSchema Schema { get; set; }

        public string Credentials => $"{I.ToLowerInvariant()}:{A}";

        public override string ToString() => $"{Schema} {Credentials}";

        public string Base64Credentials => Convert.ToBase64String(Encoding.UTF8.GetBytes(Credentials));

        public string PasswordHash => Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Base64Credentials)));

        public void Serialize(Serializer serializer)
            => serializer.Serialize($"{Schema} {Base64Credentials}");

        public void Deserialize(Serializer serializer)
        {
            var token = serializer.DeserializeString();
            var tokens = token.Split(new char[] { ' ' });

            Schema = (AuthenticationSchema)Enum.Parse(typeof(AuthenticationSchema), tokens[0]);
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(tokens[1]));

            I = credentials.Substring(0, credentials.IndexOf(':'));
            A = credentials.Substring(I.Length + 1);
        }

        public IEnumerable<ValidationError> Validate(Node node)
        {
            var name = I;
            var result = node.Config.Names.Validate(ref name);

            if (result)
                I = name;
            else
                yield return new ValidationError { ErrorMessage = result.Comment, MemberNames = new string[] { nameof(I) } };
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
