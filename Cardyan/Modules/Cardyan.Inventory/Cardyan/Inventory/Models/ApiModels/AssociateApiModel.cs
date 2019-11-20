//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace Cardyan.Inventory.Models
//{
//    public class AssociateApiModel
//    {
//        [Key]
//        public int Id { get; set; }

//        [StringLength(126, MinimumLength = 1)]
//        public string Code { get; set; }

//        [StringLength(126, MinimumLength = 2)]
//        public string Name { get; set; }

//        [StringLength(126, MinimumLength = 2)]
//        public string LastName { get; set; }

//        [StringLength(16380)]
//        public string Description { get; set; }

//        //public int TypeId { get; set; }

//        [StringLength(126)]
//        [DataType(DataType.EmailAddress)]
//        public string Email { get; set; }

//        [StringLength(24)]
//        [DataType(DataType.PhoneNumber)]
//        public string Phone { get; set; }

//        [StringLength(24)]
//        [DataType(DataType.PhoneNumber)]
//        public string Phone2 { get; set; }

//        [StringLength(126)]
//        public string Address { get; set; }
//    }
//}
