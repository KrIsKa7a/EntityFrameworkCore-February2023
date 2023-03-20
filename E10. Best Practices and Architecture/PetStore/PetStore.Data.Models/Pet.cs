﻿// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Pet : BaseDeletableModel<string>
    {
        public Pet()
        {
            Id = Guid.NewGuid().ToString();
        }

        [MaxLength(PetValidationConstants.NameMaxLength)]
        public string? Name { get; set; }

        public int Age { get; set; }

        [Required]
        public string Breed { get; set; } = null!;

        public decimal Price { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        [ForeignKey(nameof(Owner))]
        public string? ClientId { get; set; }

        public virtual Client? Owner { get; set; }

        [Required]
        [ForeignKey(nameof(Store))]
        public string StoreId { get; set; } = null!;

        public virtual Store Store { get; set; } = null!;
    }
}
