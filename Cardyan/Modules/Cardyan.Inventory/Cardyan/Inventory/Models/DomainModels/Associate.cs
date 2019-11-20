using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;

    public class Associate : KeyApiModel<int>
    {
        [StringLength(126, MinimumLength = 1)]
        public string Code { get; set; }

        [Required]
        [StringLength(126, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(126, MinimumLength = 2)]
        public string LastName { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        [StringLength(126)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone2 { get; set; }

        [StringLength(126)]
        public string Address { get; set; }

        [InverseProperty(nameof(Movement.Associate))]
        public HashSet<Movement> Movements { get; } = new HashSet<Movement>();

        [InverseProperty(nameof(AssociateTag.Associate))]
        public HashSet<AssociateTag> Tags { get; } = new HashSet<AssociateTag>();
    }
}
