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

//#if NET35

// This was setup because some problems with Unity3D not accepting Newtonsoft.Json for .NET35.
// The solution was to use the .NET20 version by overwriting the dll in Unity3D.

//namespace Newtonsoft.Json
//{
//    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
//    class JsonIgnoreAttribute : Attribute
//    {

//    }
//}

//#else

using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cyxor.Networking
{
    using Extensions;

    class JsonCommentSerializerXX
    {
        class JsonCommentWriter : JsonTextWriter
        {
            bool IsLastComment;
            StringBuilder SBuilder;
            JsonCommentSerializerXX Serializer;

            public JsonCommentWriter(StringWriter textWriter, JsonCommentSerializerXX serializer) : base(textWriter)
            {
                Serializer = serializer;
                SBuilder = textWriter.GetStringBuilder();
            }

            public override void WritePropertyName(string name)
            {
                base.WritePropertyName(name);
                ReorderLastComment();
                WriteDescription();
            }

            public override void WritePropertyName(string name, bool escape)
            {
                base.WritePropertyName(name, escape);
                ReorderLastComment();
                WriteDescription();
            }

            public void WriteDescription()
            {
                var description = GetDescription();

                if (description != null)
                    WriteComment(description);

                IsLastComment = description != null ? true : false;
            }

            public void ReorderLastComment()
            {
                if (!IsLastComment)
                    return;

                var text = SBuilder.ToString();

                var index1 = text.LastIndexOf("/*");
                var index2 = text.LastIndexOf("*/") + 3;

                if (index1 != -1 && text[index2] != '{' && text[index2] != '[')
                {
                    var index3 = text.IndexOf(Environment.NewLine, index2);

                    var commentToken = text.Substring(index1, index2 - index1 - 1);
                    var valueToken = text.Substring(index2, index3 - index2);

                    SBuilder.Replace(valueToken, commentToken, index2, valueToken.Length);
                    SBuilder.Replace(commentToken, valueToken, index1, commentToken.Length);
                }
            }

            public string GetDescription()
            {
                var obj = Serializer.Obj;
                var type = Serializer.Obj.GetType();
                var memberInfo = default(MemberInfo);
                var propertyNames = Path.Split('.');

                for (var i = 0; i < propertyNames.Length; i++)
                {
                    memberInfo = type.GetProperties().FirstOrDefault(p => p.Name == propertyNames[i]);

                    if (memberInfo == null)
                        memberInfo = type.GetFields().FirstOrDefault(p => p.Name == propertyNames[i]);

                    var tmpObj = obj;

                    if (memberInfo is PropertyInfo propertyInfo)
                    {
                        obj = propertyInfo.GetValue(obj, index: null);
                        type = obj?.GetType() ?? propertyInfo.PropertyType;
                    }
                    else if (memberInfo is FieldInfo fieldInfo)
                    {
                        obj = fieldInfo.GetValue(obj);
                        type = obj?.GetType() ?? fieldInfo.FieldType;
                    }

                    obj = obj ?? tmpObj;
                }

                var attributes = memberInfo?.GetCustomAttributes(inherit: true);
                var attribute = attributes?.SingleOrDefault(p => p.GetType() == typeof(DescriptionAttribute));

                return (attribute as DescriptionAttribute)?.Description;
            }
        }

        //NodeConfig Config { get; }
        object Obj { get; set; }
        StringBuilder SBuilder { get; }
        JsonSerializer Serializer { get; }
        JsonCommentWriter TextWriter { get; }

        public static JsonCommentSerializerXX Instance { get; } = new JsonCommentSerializerXX();

        public JsonCommentSerializerXX()//(NodeConfig config)
        {
            //Config = config;
            SBuilder = new StringBuilder();
            Serializer = new JsonSerializer { Formatting = Formatting.Indented };
            Serializer.Converters.Add(new BinaryConverter());
            Serializer.Converters.Add(new StringEnumConverter());
            TextWriter = new JsonCommentWriter(new StringWriter(SBuilder), this);
        }

        public virtual string Serialize(object obj, bool excludeDefaultValues = false)
        {
            Obj = obj;

            SBuilder.Clear();

            if (excludeDefaultValues)
                Serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            else
                Serializer.DefaultValueHandling = DefaultValueHandling.Include;

            Serializer.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;

            Serializer.Serialize(TextWriter, obj);
            TextWriter.ReorderLastComment();
            return SBuilder.ToString();
        }

        public virtual void Save(string path, object obj, bool excludeDefaultValues) => File.WriteAllText(path, Serialize(obj, excludeDefaultValues));

        public void Load(string value, object obj)
        {
            var settings = new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error };
            JsonConvert.PopulateObject(value, obj, settings);
        }

        public void LoadFromFile(string path, object obj) => Load(File.ReadAllText(path), obj);
    }
}

//#endif

/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
