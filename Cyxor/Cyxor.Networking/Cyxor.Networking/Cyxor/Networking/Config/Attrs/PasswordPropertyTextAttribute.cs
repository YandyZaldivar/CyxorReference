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

#if NETSTANDARD1_3 || UAP10_0

using System;

namespace Cyxor.Networking.Config
{
    /// <summary>
    /// Indicates that a configuration property can be modified even when the associated network <see cref="Node"/>
    /// is connected.
    /// <para>
    /// This attribute is applied to certain network configuration properties that makes sense to changed when the
    /// <see cref="Node"/> instance is connected. For example, it is not meaningful to change the connection port
    /// while connected and trying to do this should raise an exception. This is the default behavior and the
    /// correct procedure with these properties is first stop the connection and then modify their values.
    /// </para>
    /// <para>
    /// Applying this attribute to a property will allow you to achieve the opposite behavior. The usage of this 
    /// attribute stores metadata than can be retrieved to identify the marked properties. One possible scenario 
    /// is to show them as modifiable in a property grid control even when the <see cref="Node"/> is connected.
    /// </para>
    /// <para>
    /// For derived <c>Cyxor</c> configuration classes we recommend the use of this attribute when appropriate.
    /// See the <see cref= "NodeConfig"/> class for more information.
    /// </para>
    /// See also <seealso cref="ConfigExpandableObjectConverter"/> 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PasswordPropertyTextAttribute : Attribute
    {
        /// <summary>
        /// Specifies that the property this attribute is bound to is read-only and cannot be modified
        /// in the server explorer. This <see langword='static '/>field is read-only.
        /// </summary>
        public static readonly PasswordPropertyTextAttribute Yes = new PasswordPropertyTextAttribute(true);

        /// <devdoc>
        ///    <para>
        ///       Specifies that the property this attribute is bound to is read/write and can
        ///       be modified at design time. This <see langword='static '/>field is read-only.
        ///    </para>
        /// </devdoc>
        public static readonly PasswordPropertyTextAttribute No = new PasswordPropertyTextAttribute(false);

        /// <devdoc>
        ///    <para>
        ///       Specifies the default value for the <see cref='System.ComponentModel.ReadOnlyAttribute'/> , which is <see cref='System.ComponentModel.ReadOnlyAttribute.No'/>, that is,
        ///       the property this attribute is bound to is read/write. This <see langword='static'/> field is read-only.
        ///    </para>
        /// </devdoc>
        public static readonly PasswordPropertyTextAttribute Default = No;

        /// <devdoc>
        ///    <para>
        ///       Initializes a new instance of the <see cref='System.ComponentModel.ReadOnlyAttribute'/> class.
        ///    </para>
        /// </devdoc>
        public PasswordPropertyTextAttribute(bool password)
        {
            IsPassword = password;
        }

        /// <summary>
        /// Gets a value indicating whether the property this attribute is bound to is read-only.
        /// </summary>
        public bool IsPassword { get; private set; }

        /// <internalonly/>
        /// <devdoc>
        /// </devdoc>
        public override bool Equals(object value)
        {
            if (this == value)
                return true;

            var other = value as PasswordPropertyTextAttribute;

            return other != null && other.IsPassword == IsPassword;
        }

        /// <summary>
        /// Returns the hashcode for this object.
        /// </summary>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Determines if this attribute is the default.
        /// </summary>
        //public override bool IsDefaultAttribute() => IsConnectedModifiable == Default.IsConnectedModifiable;
    }
}

#endif

/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
