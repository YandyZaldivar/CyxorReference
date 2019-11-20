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
using System.ComponentModel;

namespace Cyxor.Networking.Config
{
    public class ConsoleConfig : ConfigProperty
    {
        public const bool DefaultHideBaseHeader = false;
        bool hideBaseHeader = DefaultHideBaseHeader;
        [Description("TODO:")]
        [DefaultValue(DefaultHideBaseHeader)]
        public bool HideBaseHeader
        {
            get => hideBaseHeader;
            set => SetProperty(ref hideBaseHeader, value);
        }

        public const int DefaultMargenLength = 4;
        int margenLength = DefaultMargenLength;
        [Description("TODO:")]
        [DefaultValue(DefaultMargenLength)]
        public int MargenLength
        {
            get => margenLength;
            set => SetProperty(ref margenLength, value);
        }

        public const int DefaultOutputSpeedDelay = 5;
        int outputSpeedDelay = DefaultOutputSpeedDelay;
        [Description("TODO:")]
        [DefaultValue(DefaultOutputSpeedDelay)]
        public int OutputSpeedDelay
        {
            get => outputSpeedDelay;
            set => SetProperty(ref outputSpeedDelay, value);
        }

        public const string DefaultInputEncodingName = "utf-8";
        string inputEncodingName = DefaultInputEncodingName;
        [Description("TODO:")]
        [DefaultValue(DefaultInputEncodingName)]
        public string InputEncodingName
        {
            get => inputEncodingName;
            set => SetProperty(ref inputEncodingName, value);
        }

        public const string DefaultOutputEncodingName = "utf-8";
        string outputEncodingName = DefaultOutputEncodingName;
        [Description("TODO:")]
        [DefaultValue(DefaultOutputEncodingName)]
        public string OutputEncodingName
        {
            get => outputEncodingName;
            set => SetProperty(ref outputEncodingName, value);
        }

        public const ConsoleColor DefaultOperationHeaderForegroundColor = ConsoleColor.DarkCyan;
        ConsoleColor operationHeaderForegroundColor = DefaultOperationHeaderForegroundColor;
        [Description("TODO:")]
        [DefaultValue(DefaultOperationHeaderForegroundColor)]
        public ConsoleColor OperationHeaderForegroundColor
        {
            get => operationHeaderForegroundColor;
            set => SetProperty(ref operationHeaderForegroundColor, value);
        }

        public const ConsoleColor DefaultOperationForegroundColor = ConsoleColor.DarkGreen;
        ConsoleColor operationForegroundColor = DefaultOperationForegroundColor;
        [Description("TODO:")]
        [DefaultValue(DefaultOperationForegroundColor)]
        public ConsoleColor OperationForegroundColor
        {
            get => operationForegroundColor;
            set => SetProperty(ref operationForegroundColor, value);
        }

        public const ConsoleColor DefaultCommandSuccessHeaderForegroundColor = ConsoleColor.Green;
        ConsoleColor commandSuccessHeaderForegroundColor = DefaultCommandSuccessHeaderForegroundColor;
        [Description("TODO:")]
        [DefaultValue(DefaultCommandSuccessForegroundColor)]
        public ConsoleColor CommandSuccessHeaderForegroundColor
        {
            get => commandSuccessHeaderForegroundColor;
            set => SetProperty(ref commandSuccessHeaderForegroundColor, value);
        }

        //public const ConsoleColor DefaultCommandSuccessForegroundColor = ConsoleColor.DarkGreen;
        public const ConsoleColor DefaultCommandSuccessForegroundColor = ConsoleColor.Cyan;
        ConsoleColor commandSuccessForegroundColor = DefaultCommandSuccessForegroundColor;
        [Description("TODO:")]
        [DefaultValue(DefaultCommandSuccessForegroundColor)]
        public ConsoleColor CommandSuccessForegroundColor
        {
            get => commandSuccessForegroundColor;
            set => SetProperty(ref commandSuccessForegroundColor, value);
        }

        public const ConsoleColor DefaultCommandErrorHeaderForegroundColor = ConsoleColor.Red;
        ConsoleColor commandErrorHeaderForegroundColor = DefaultCommandErrorHeaderForegroundColor;
        [Description("TODO:")]
        [DefaultValue(DefaultCommandErrorHeaderForegroundColor)]
        public ConsoleColor CommandErrorHeaderForegroundColor
        {
            get => commandErrorHeaderForegroundColor;
            set => SetProperty(ref commandErrorHeaderForegroundColor, value);
        }

        public const ConsoleColor DefaultCommandErrorForegroundColor = ConsoleColor.DarkRed;
        ConsoleColor commandErrorForegroundColor = DefaultCommandErrorForegroundColor;
        [Description("TODO:")]
        [DefaultValue(DefaultCommandErrorForegroundColor)]
        public ConsoleColor CommandErrorForegroundColor
        {
            get => commandErrorForegroundColor;
            set => SetProperty(ref commandErrorForegroundColor, value);
        }

        public const ConsoleColor DefaultCommandHelpHeaderForegroundColor = ConsoleColor.Yellow;
        ConsoleColor commandHelpHeaderForegroundColor = DefaultCommandHelpHeaderForegroundColor;
        [Description("TODO:")]
        [DefaultValue(DefaultCommandHelpHeaderForegroundColor)]
        public ConsoleColor CommandHelpHeaderForegroundColor
        {
            get => commandHelpHeaderForegroundColor;
            set => SetProperty(ref commandHelpHeaderForegroundColor, value);
        }

        public const ConsoleColor DefaultCommandHelpForegroundColor = ConsoleColor.DarkYellow;
        ConsoleColor commandHelpForegroundColor = DefaultCommandHelpForegroundColor;
        [Description("TODO:")]
        [DefaultValue(DefaultCommandHelpForegroundColor)]
        public ConsoleColor CommandHelpForegroundColor
        {
            get => commandHelpForegroundColor;
            set => SetProperty(ref commandHelpForegroundColor, value);
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
