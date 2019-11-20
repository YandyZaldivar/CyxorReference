// { Alimatic.Datadin } - Backend
// Copyright (C) 2018 Alimatic
// Author:  Yandy Zaldivar

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Datadin.Produccion.Models
{
    public class Frequency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(16, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
// { Alimatic.Datadin } - Backend
