using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cardyan.Inventory.Models
{
    using Cyxor.Models;
    using Cyxor.Networking;

    public class Movement : KeyApiModel<int>
    {
        [StringLength(126, MinimumLength = 1)]
        public string Code { get; set; }

        [StringLength(16380)]
        public string Description { get; set; }

        public int WarehouseId { get; set; }

        [ForeignKey(nameof(WarehouseId))]
        public Warehouse Warehouse { get; set; }

        public int? LinkedWarehouseId { get; set; }

        [ForeignKey(nameof(LinkedWarehouseId))]
        public Warehouse LinkedWarehouse { get; set; }

        public int? LinkedMovementId { get; set; }

        [ForeignKey(nameof(LinkedMovementId))]
        public Movement LinkedMovement { get; set; }

        public int TypeId { get; set; }

        [ForeignKey(nameof(TypeId))]
        public MovementType Type { get; set; }

        public int? AssociateId { get; set; }

        [ForeignKey(nameof(AssociateId))]
        public Associate Associate { get; set; }

        [NotMapped]
        public bool AutoTransference { get; set; }

        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [InverseProperty(nameof(MovementProduct.Movement))]
        public HashSet<MovementProduct> Products { get; set; } = new HashSet<MovementProduct>();

        [InverseProperty(nameof(MovementTag.Movement))]
        public HashSet<MovementTag> Tags { get; set; } = new HashSet<MovementTag>();

        public bool IsTransference() => LinkedWarehouseId != null || LinkedMovementId != null;

        // TODO: Validate if destination Warehouse has the products associated,
        // consider doing this in the controller!?
        public IEnumerable<ValidationError> Validate(Node node)
        {
            var movementType = MovementType.Items.SingleOrDefault(p => p.Id == TypeId);

            if (movementType == null)
            {
                yield return new ValidationError
                {
                    ErrorMessage = $"[{nameof(TypeId)} = {TypeId}] " +
                    $"can't be found in the database.",
                    MemberNames = new string[] { nameof(TypeId) }
                };

                yield break;
            }

            var movementTypeValue = movementType.Value;

            if (IsTransference() && AssociateId != null)
                yield return new ValidationError
                {
                    ErrorMessage = $"'{nameof(AssociateId)} = {AssociateId}' " +
                    $"must be null when performing a transference between warehouses.",
                    MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(AssociateId) }
                };

            if (!IsTransference() && AutoTransference == true)
                yield return new ValidationError
                {
                    ErrorMessage = $"{nameof(AutoTransference)} " +
                    $"can only be true when performing a transference between warehouses.",
                    MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(AutoTransference) }
                };
            else if (IsTransference() && movementTypeValue == MovementTypeValue.In && AutoTransference == true)
                yield return new ValidationError
                {
                    ErrorMessage = $"{nameof(AutoTransference)} " +
                    $"can only be true when performing an 'Out' transference.",
                    MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(AutoTransference) }
                };
            else if (LinkedWarehouseId != null && movementTypeValue == MovementTypeValue.In)
                yield return new ValidationError
                {
                    ErrorMessage = $"{nameof(LinkedWarehouseId)} " +
                    $"can only be set when performing an 'Out' transference.",
                    MemberNames = new string[] { nameof(LinkedWarehouseId), nameof(TypeId) }
                };

            if (LinkedMovementId != null && movementTypeValue != MovementTypeValue.In)
                yield return new ValidationError
                {
                    ErrorMessage = $"If {nameof(Movement)}.{nameof(LinkedMovementId)} is set, " +
                    $"the {nameof(Movement)}.{nameof(TypeId)} must represent an 'In' movement.",
                    MemberNames = new string[] { nameof(LinkedMovementId), nameof(TypeId) }
                };
            else if (LinkedMovementId != null)
            {
                if (LinkedWarehouseId != null)
                    yield return new ValidationError
                    {
                        ErrorMessage = $"{nameof(LinkedWarehouseId)} " +
                        $"must be null when performing an 'In' transference.",
                        MemberNames = new string[] { nameof(LinkedMovementId), nameof(TypeId), nameof(LinkedWarehouseId) }
                    };

                if ((Products?.Count() ?? 0) != 0)
                    yield return new ValidationError
                    {
                        ErrorMessage = $"{nameof(Products)} " +
                        $"must be empty when performing an 'In' transference.",
                        MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(Products) }
                    };
            }
            else
            {
                if ((Products?.Count() ?? 0) == 0)
                    yield return new ValidationError
                    {
                        ErrorMessage = $"{nameof(Products)} can't be empty.",
                        MemberNames = new string[] { nameof(LinkedMovementId), nameof(LinkedWarehouseId), nameof(Products) }
                    };
                else
                {
                    var i = 0;

                    foreach (var product in Products)
                    {
                        if (product.Count <= 0)
                            yield return new ValidationError
                            {
                                ErrorMessage = $"{nameof(Products)}[{i}].{nameof(product.Count)} " +
                                $"must be greater than 0 when performing a movement.",
                                MemberNames = new string[] { $"{nameof(Products)}.{nameof(product.Count)}" }
                            };

                        if (movementTypeValue == MovementTypeValue.Out)
                        {
                            if (product.Price != null)
                                yield return new ValidationError
                                {
                                    ErrorMessage = $"{nameof(Products)}[{i}].{nameof(product.Price)} " +
                                                                $"must be null when performing an 'Out' movement.",
                                    MemberNames = new string[] { $"{nameof(Products)}.{nameof(product.Price)}", nameof(TypeId) }
                                };
                        }
                        else if (product.Price == null)
                            yield return new ValidationError
                            {
                                ErrorMessage = $"{nameof(Products)}[{i}].{nameof(product.Price)} " +
                                                            $"can't be null when performing an 'In' movement.",
                                MemberNames = new string[] { $"{nameof(Products)}.{nameof(product.Price)}" }
                            };

                        i++;
                    }
                }
            }
        }
    }
}
