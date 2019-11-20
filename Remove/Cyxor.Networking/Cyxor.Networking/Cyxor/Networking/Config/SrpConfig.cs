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
using System.ComponentModel;

using Newtonsoft.Json;

namespace Cyxor.Networking.Config
{
    using Security;
    using Serialization;

    public class SrpConfig : ConfigProperty
    {
        [CyxorIgnore]
        [JsonIgnore]
        public Security.SrpConfig InternalConfig { get; } = new Security.SrpConfig();

        public SrpConfig()
        {

        }

        //public const bool DefaultEnabled = false;
        //bool enabled = DefaultEnabled;
        //[Description("TODO:")]
        //[DefaultValue(DefaultEnabled)]
        //public bool Enabled
        //{
        //    get => enabled;
        //    set => SetProperty(ref enabled, value);
        //}

        [Description("TODO:")]
        [DefaultValue(Security.SrpConfig.DefaultProtocolVersion)]
        public SrpProtocolVersion ProtocolVersion
        {
            get => InternalConfig.ProtocolVersion;
            set
            {
                var protocolVersion = InternalConfig.ProtocolVersion;
                SetProperty(ref protocolVersion, value);
                InternalConfig.ProtocolVersion = protocolVersion;
            }
        }

        [Description("TODO:")]
        [DefaultValue(Security.SrpConfig.DefaultGenerator)]
        public string Generator
        {
            get => InternalConfig.Generator;
            set
            {
                var generator = InternalConfig.Generator;
                SetProperty(ref generator, value);
                InternalConfig.Generator = generator;
            }
        }

        [Description("TODO:")]
        [DefaultValue(Security.SrpConfig.DefaultPrime)]
        public string Prime
        {
            get => InternalConfig.Prime;
            set
            {
                var prime = InternalConfig.Prime;
                SetProperty(ref prime, value);
                InternalConfig.Prime = prime;
            }
        }

        [Description("TODO:")]
        [DefaultValue(Security.SrpConfig.DefaultHashAlgorithm)]
        public SrpHashAlgorithm HashAlgorithm
        {
            get => InternalConfig.HashAlgorithm;
            set
            {
                var hashAlgorithm = InternalConfig.HashAlgorithm;
                SetProperty(ref hashAlgorithm, value);
                InternalConfig.HashAlgorithm = hashAlgorithm;
            }
        }

        [Description("TODO:")]
        [DefaultValue(Security.SrpConfig.DefaultSymmetricAlgorithm)]
        public SrpSymmetricAlgorithm SymmetricAlgorithm
        {
            get => InternalConfig.SymmetricAlgorithm;
            set
            {
                var symmetricAlgorithm = InternalConfig.SymmetricAlgorithm;
                SetProperty(ref symmetricAlgorithm, value);
                InternalConfig.SymmetricAlgorithm = symmetricAlgorithm;
            }
        }

        [Description("TODO:")]
        [DefaultValue(Security.SrpConfig.DefaultRandomNumberGenerator)]
        public SrpRandomNumberGenerator RandomNumberGenerator
        {
            get => InternalConfig.RandomNumberGenerator;
            set
            {
                var randomNumberGenerator = InternalConfig.RandomNumberGenerator;
                SetProperty(ref randomNumberGenerator, value);
                InternalConfig.RandomNumberGenerator = randomNumberGenerator;
            }
        }

        byte[] symmetricIV = default(byte[]);
        [Description("TODO:")]
        [DefaultValue(default(byte[]))]
        //[Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public byte[] SymmetricIV
        {
            get => symmetricIV;
            set => SetProperty(ref symmetricIV, value);
        }

        byte[] symmetricKey = default(byte[]);
        [Description("TODO:")]
        [DefaultValue(default(byte[]))]
        public byte[] SymmetricKey
        {
            get => symmetricKey;
            set => SetProperty(ref symmetricKey, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
