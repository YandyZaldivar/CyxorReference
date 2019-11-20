// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Datadin.Produccion.Models
{
    public class Template
    {
        [Key]
        public int Year { get; set; }

        [Key]
        public int Month { get; set; }

        [Key]
        public int Day { get; set; }

        [Key]
        public int ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]

        public Model Model { get; set; }

        public bool Locked { get; set; }
    }
}
// { Alimatic.Datadin } - Backend
