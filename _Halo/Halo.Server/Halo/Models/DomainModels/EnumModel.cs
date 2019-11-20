/*
  { Halo.Server } - Sistema Nacional de Vigilancia a la Morbilidad Materna Extremadamente Grave
  Copyright (C) 2017 Halo
  Authors:  Mayli Sanchez
            Yandy Zaldivar
*/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;

namespace Halo.Models
{
#if NET35
    using StringLengthAttribute = Cyxor.Models.StringLengthAttribute;
#endif

#if NET35 || NET40
    using Cyxor.Extensions;
#endif

    using Cyxor.Models;
    using Cyxor.Serialization;

    public class EnumModel<TEnum> : IIdNombreApiModel where TEnum : struct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 2)]
        public string Valor { get; set; } = Utilities.Enum.GetConstantOrDefault<TEnum>().ToString();

        [StringLength(128)]
        public string Nombre
        {
            get => typeof(TEnum).GetField(Valor).GetCustomAttribute<DisplayAttribute>()?.Name ?? Valor;
            set { }
        }

        [NotMapped]
        [JsonIgnore]
        public TEnum Tipo
        {
            get => (TEnum)Enum.Parse(typeof(TEnum), Valor);
            set => Valor = value.ToString();
        }

        public static IEnumerable<TModel> List<TModel>() where TModel : EnumModel<TEnum>
        {
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                var model = Activator.CreateInstance<TModel>();

                model.Id = (int)value;
                model.Tipo = (TEnum)Enum.Parse(typeof(TEnum), value.ToString());

                yield return model;
            }
        }
    }
}
/* { Halo.Server } */
