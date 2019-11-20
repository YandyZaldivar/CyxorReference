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
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Cyxor.Networking.Config
{
    public class NameConfig : ConfigProperty
    {
        public const int DefaultMinLength = 2;
        public const int DefaultMaxLength = 64;
        public const string DefaultRegexPattern = "^[a-zA-Z]{1}[a-zA-Z0-9.@]{1,64}$";
        public const string DefaultUnavailableNameViolationMessage = "That name is unavailable.";
        public const string DefaultMinLengthViolationMessage = "Names must be at least 2 characters.";
        public const string DefaultMaxLengthViolationMessage = "Names can only have a maximum of 64 characters.";
        public const string DefaultRegexViolationMessage = "Names can only have digits and letters from A to Z, the symbols '.' or '@' and must start with a letter.";

        public static string[] DefaultUnavailableNames { get; } = (typeof(NameConfig).GetProperty(nameof(UnavailableNames)).GetCustomAttributes(typeof(DefaultValueAttribute), inherit: false).Single() as DefaultValueAttribute).Value as string[];

        public NameConfig() { }

        int minLength = DefaultMinLength;
        [DefaultValue(DefaultMinLength)]
        [Description("TODO:")]
        public int MinLength
        {
            get => minLength;
            set => SetProperty(ref minLength, value);
        }

        string minLengthViolationMessage = DefaultMinLengthViolationMessage;
        [DefaultValue(DefaultMinLengthViolationMessage)]
        [Description("TODO:")]
        public string MinLengthViolationMessage
        {
            get => minLengthViolationMessage;
            set => SetProperty(ref minLengthViolationMessage, value);
        }

        int maxLength = DefaultMaxLength;
        [DefaultValue(DefaultMaxLength)]
        [Description("TODO:")]
        public int MaxLength
        {
            get => maxLength;
            set => SetProperty(ref maxLength, value);
        }

        string maxLengthViolationMessage = DefaultMaxLengthViolationMessage;
        [DefaultValue(DefaultMaxLengthViolationMessage)]
        [Description("TODO:")]
        public string MaxLengthViolationMessage
        {
            get => maxLengthViolationMessage;
            set => SetProperty(ref maxLengthViolationMessage, value);
        }

        string[] unavailableNames = DefaultUnavailableNames;
        [DefaultValue(new string[] { })]
        [Description("TODO:")]
        public string[] UnavailableNames
        {
            get => unavailableNames;
            set => SetProperty(ref unavailableNames, value);
        }

        string unavailableNameViolationMessage = DefaultUnavailableNameViolationMessage;
        [DefaultValue(DefaultUnavailableNameViolationMessage)]
        [Description("TODO:")]
        public string UnavailableNameViolationMessage
        {
            get => unavailableNameViolationMessage;
            set => SetProperty(ref unavailableNameViolationMessage, value);
        }

        string regexPattern = DefaultRegexPattern;
        [DefaultValue(DefaultRegexPattern)]
        [Description("TODO:")]
        public string RegexPattern
        {
            get => regexPattern;
            set => SetProperty(ref regexPattern, value);
        }

        string regexViolationMessage = DefaultRegexViolationMessage;
        [DefaultValue(DefaultRegexViolationMessage)]
        [Description("TODO:")]
        public string RegexViolationMessage
        {
            get => regexViolationMessage;
            set => SetProperty(ref regexViolationMessage, value);
        }

        public override Result Validate()
        {
            var name = RootConfig.Name;

            var result = Validate(ref name);

            if (result)
                RootConfig.Name = name;

            return result;
        }

        public Result Validate(ref string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result.Success;
            else if (name.Length < MinLength)
                return new Result(ResultCode.NameMinLengthViolation, MinLengthViolationMessage);
            else if (name.Length > MaxLength)
                return new Result(ResultCode.NameMaxLengthViolation, MaxLengthViolationMessage);
            else
            {
                foreach (var unavailableName in UnavailableNames)
                    if (name.IndexOf(unavailableName, StringComparison.OrdinalIgnoreCase) >= 0)
                        return new Result(ResultCode.NameUnavailable, UnavailableNameViolationMessage);

                if (!new Regex(regexPattern).IsMatch(name))
                    return new Result(ResultCode.NameRegexViolation, RegexViolationMessage);

                name = name[0].ToString().ToUpperInvariant() + name.Substring(1).ToLowerInvariant();
                return Result.Success;
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
