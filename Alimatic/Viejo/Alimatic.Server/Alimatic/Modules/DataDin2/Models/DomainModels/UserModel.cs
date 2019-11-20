/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.DataDin2.Models
{
    using Cyxor.Models;

    public class UserModel : KeyApiModel<int, int>
    {
        [Key]
        public int UserId { get => Id1; set => Id1 = value; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [Key]
        public virtual int ModelId { get => Id2; set => Id2 = value; }

        [ForeignKey(nameof(ModelId))]
        public virtual Model Model { get; set; }
    }
}
/* { Alimatic.Server } */
