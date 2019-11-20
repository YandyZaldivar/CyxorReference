/*
  { Alimatic.Server } - Servidor de Control Interno de Alimatic
  Copyright (C) 2017 Alimatic
  Authors:  José Ramón Menéndez
            Yandy Zaldivar
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alimatic.Nexus.Models
{
    public enum PermissionValue
    {
        None = 1,
        Read,
        Update,
        Write,
    }

    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 4)]
        public string Name { get; set; }

        [NotMapped]
        public PermissionValue Value
        {
            get => (PermissionValue)Enum.Parse(typeof(PermissionValue), Name);
            set => Name = value.ToString();
        }

        [InverseProperty(nameof(TableRole.Permission))]
        public virtual ModelCollection<TableRole> TableSecurityList { get; set; }

        [InverseProperty(nameof(ColumnRole.Permission))]
        public virtual ModelCollection<ColumnRole> ColumnSecurityList { get; set; }

        public Permission()
        {
            Value = PermissionValue.Read;
            TableSecurityList = new ModelCollection<TableRole>();
            ColumnSecurityList = new ModelCollection<ColumnRole>();
        }
    }
}
/* { Alimatic.Server } */
