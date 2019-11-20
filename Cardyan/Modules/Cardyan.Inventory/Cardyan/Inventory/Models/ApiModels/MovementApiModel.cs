//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace Cardyan.Inventory.Models
//{
//    using Cyxor.Models;
//    using Cyxor.Networking;

//    public class MovementApiModel : IValidatable
//    {
//        [Key]
//        public int Id { get; set; }

//        [StringLength(126)]
//        public string Code { get; set; }

//        [StringLength(16380)]
//        public string Description { get; set; }

//        public int WarehouseId { get; set; }

//        public int? LinkedWarehouseId { get; set; }

//        public int? LinkedMovementId { get; set; }

//        public int TypeId { get; set; }

//        public int? AssociateId { get; set; }

//        public bool AutoTransference { get; set; }

//        public DateTime DateTime { get; set; } = DateTime.Now;

//        [AutoMapper.IgnoreMap]
//        public IEnumerable<MovementProductApiModel> Products { get; set; }

//        public bool IsTransference() => LinkedWarehouseId != null || LinkedMovementId != null;

//        public IEnumerable<ValidationError> Validate(Node node)
//        {
//            var movementType = MovementType.Items.SingleOrDefault(p => p.Id == TypeId);

//            if (movementType == null)
//            {
//                yield return new ValidationError
//                {
//                    ErrorMessage = $"[{nameof(TypeId)} = {TypeId}] " +
//                    $"can't be found in the database.",
//                    MemberNames = new string[] { nameof(TypeId) }
//                };

//                yield break;
//            }

//            var movementTypeValue = movementType.Value;

//            if (IsTransference() && AssociateId != null)
//                yield return new ValidationError
//                {
//                    ErrorMessage = $"'{nameof(AssociateId)} = {AssociateId}' " +
//                    $"must be null when performing a transference between warehouses.",
//                    MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(AssociateId) }
//                };

//            if (!IsTransference() && AutoTransference == true)
//                yield return new ValidationError
//                {
//                    ErrorMessage = $"{nameof(AutoTransference)} " +
//                    $"can only be true when performing a transference between warehouses.",
//                    MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(AutoTransference) }
//                };
//            else if (IsTransference() && movementTypeValue == MovementTypeValue.In && AutoTransference == true)
//                yield return new ValidationError
//                {
//                    ErrorMessage = $"{nameof(AutoTransference)} " +
//                    $"can only be true when performing an 'Out' transference.",
//                    MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(AutoTransference) }
//                };
//            else if (LinkedWarehouseId != null && movementTypeValue == MovementTypeValue.In)
//                yield return new ValidationError
//                {
//                    ErrorMessage = $"{nameof(LinkedWarehouseId)} " +
//                    $"can only be set when performing an 'Out' transference.",
//                    MemberNames = new string[] { nameof(LinkedWarehouseId), nameof(TypeId) }
//                };

//            if (LinkedMovementId != null && movementTypeValue != MovementTypeValue.In)
//                yield return new ValidationError
//                {
//                    ErrorMessage = $"If {nameof(MovementApiModel)}.{nameof(LinkedMovementId)} is set, " +
//                    $"the {nameof(MovementApiModel)}.{nameof(TypeId)} must represent an 'In' movement.",
//                    MemberNames = new string[] { nameof(LinkedMovementId), nameof(TypeId) }
//                };
//            else if (LinkedMovementId != null)
//            {
//                if (LinkedWarehouseId != null)
//                    yield return new ValidationError
//                    {
//                        ErrorMessage = $"{nameof(LinkedWarehouseId)} " +
//                        $"must be null when performing an 'In' transference.",
//                        MemberNames = new string[] { nameof(LinkedMovementId), nameof(TypeId), nameof(LinkedWarehouseId) }
//                    };

//                if ((Products?.Count() ?? 0) != 0)
//                    yield return new ValidationError
//                    {
//                        ErrorMessage = $"{nameof(Products)} " +
//                        $"must be empty when performing an 'In' transference.",
//                        MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(Products) }
//                    };
//            }
//            else
//            {
//                if ((Products?.Count() ?? 0) == 0)
//                    yield return new ValidationError
//                    {
//                        ErrorMessage = $"{nameof(Products)} can't be empty.",
//                        MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(Products) }
//                    };
//                else
//                {
//                    var i = 0;

//                    foreach (var product in Products)
//                    {
//                        if (product.Count <= 0)
//                            yield return new ValidationError
//                            {
//                                ErrorMessage = $"{nameof(Products)}[{i}].{nameof(product.Count)} " +
//                                $"must be greater than 0 when performing a movement.",
//                                MemberNames = new string[] { $"{nameof(Products)}.{nameof(product.Count)}" }
//                            };

//                        if (movementTypeValue == MovementTypeValue.Out)
//                        {
//                            if (product.Price != null)
//                                yield return new ValidationError
//                                {
//                                    ErrorMessage = $"{nameof(Products)}[{i}].{nameof(product.Price)} " +
//                                                                $"must be null when performing an 'Out' movement.",
//                                    MemberNames = new string[] { $"{nameof(Products)}.{nameof(product.Price)}", nameof(TypeId) }
//                                };
//                        }
//                        else if (product.Price == null)
//                            yield return new ValidationError
//                            {
//                                ErrorMessage = $"{nameof(Products)}[{i}].{nameof(product.Price)} " +
//                                                            $"can't be null when performing an 'In' movement.",
//                                MemberNames = new string[] { $"{nameof(Products)}.{nameof(product.Price)}" }
//                            };

//                        i++;
//                    }
//                }
//            }
//        }
//    }
//}
