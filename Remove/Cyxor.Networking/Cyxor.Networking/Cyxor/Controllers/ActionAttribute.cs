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
using System.Reflection;

namespace Cyxor.Controllers
{
    using Models;
    using Extensions;

    public enum HttpContentFormat
    {
        Json,
        Xml,
        String,
        Binary,
    }

    //public class HttpContent
    //{
    //    public string Type { get; set; }
    //    public string Charset { get; set; }
    //    public HttpContentFormat Format { get; set; }
    //}

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ActionAttribute : Attribute
    {
        int _maximumBytes = Serialization.Utilities.EncodedInteger.OneByteCap;

        public bool Hide { get; set; }
        public int Security { get; set; }
        public string[] Roles { get; set; }
        public bool IsFullRoute { get; set; }

        public string HttpContentType { get; set; }
        public HttpContentFormat HttpContentFormat { get; set; }

        public int MaximumBytes => ModelAttribute?.MaximumBytes ?? _maximumBytes;

        string route;
        public string Route
        {
            get => ModelAttribute?.Route ?? route;
            set => route = value.ToLowerInvariant();
        }

        string _description;
        public string Description
        {
            get => ModelAttribute?.Description ?? _description;
            set => _description = value;
        }

        Type modelType;
        public Type ModelType
        {
            get => modelType;
            set => ModelAttribute = (modelType = value).GetTypeInfo().GetCustomAttribute<ModelAttribute>(inherit: true);
        }

        public ModelAttribute ModelAttribute { get; private set; }

        //public ActionAttribute(object api)
        //    => _id = api is string route ? Utilities.HashCode.GetFrom(_route = route.ToLowerInvariant()) : api.GetHashCode();

        public ActionAttribute() { }

        public ActionAttribute(string api)
            => route = api.ToLowerInvariant();

        public ActionAttribute(string api, string description) : this(api)
            => _description = description;

        public ActionAttribute(Type modelType)
            => ModelAttribute = (ModelType = modelType).GetTypeInfo().GetCustomAttribute<ModelAttribute>(inherit: true);
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
