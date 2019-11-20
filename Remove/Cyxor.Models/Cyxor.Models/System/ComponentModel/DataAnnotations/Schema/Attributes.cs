/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

#if NET35

namespace Cyxor.Models
{
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class StringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
    {
        public int MinimumLength { get; set; }

        public StringLengthAttribute(int maximumLength) : base(maximumLength) { }
    }
}

namespace System.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class KeyAttribute : Attribute
    {
        public KeyAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DisplayAttribute : Attribute
    {
        public bool AutoGenerateField { get; set; }
        public Type ResourceType { get; set; }
        public string GroupName { get; set; }
        public string Prompt { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int Order { get; set; }
        public bool AutoGenerateFilter { get; set; }

        public DisplayAttribute() { }

        public bool? GetAutoGenerateField() => null;
        public bool? GetAutoGenerateFilter() => null;
        public string GetDescription() => null;
        public string GetGroupName() => null;
        public string GetName() => null;
        public int? GetOrder() => null;
        public string GetPrompt() => null;
        public string GetShortName() => null;
    }
}

#endif

#if NET35 || NET40

namespace System.Reflection
{
    public static class IntrospectionExtensions
    {
        public static Type GetTypeInfo(this Type type) => type;

        public static TAttribute GetCustomAttribute<TAttribute>(this Type type, bool inherit = false)
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), inherit);

            if (attributes.Length > 0)
                return (TAttribute)attributes[0];

            return default(TAttribute);
        }
    }
}

namespace System.ComponentModel.DataAnnotations.Schema
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ForeignKeyAttribute : Attribute
    {
        public string Name { get; private set; }

        public ForeignKeyAttribute(string name) => Name = name;
    }

    public enum DatabaseGeneratedOption
    {
        None = 0,
        Identity = 1,
        Computed = 2
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DatabaseGeneratedAttribute : Attribute
    {
        public DatabaseGeneratedOption DatabaseGeneratedOption { get; }
        public DatabaseGeneratedAttribute(DatabaseGeneratedOption databaseGeneratedOption)
            => DatabaseGeneratedOption = databaseGeneratedOption;
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InversePropertyAttribute : Attribute
    {
        public string Property { get; }
        public InversePropertyAttribute(string property) => Property = property;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotMappedAttribute : Attribute
    {
        public NotMappedAttribute() { }
    }
}

#endif

/* { Halo.Server } */
