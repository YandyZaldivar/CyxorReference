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

#if NET35 || NET40
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Indicates the type of the async method builder that should be used by a language compiler
    /// to build the attributed type when used as the return type of an async method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    public sealed class AsyncMethodBuilderAttribute : Attribute
    {
        /// <summary>
        /// Gets the System.Type of the associated builder.
        /// </summary>
        public Type BuilderType { get; }

        /// <summary>
        /// Initializes the System.Runtime.CompilerServices.AsyncMethodBuilderAttribute.
        /// </summary>
        /// <param name="builderType">The System.Type of the associated builder.</param>
        public AsyncMethodBuilderAttribute(Type builderType) => BuilderType = builderType;
    }
}
#endif
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
