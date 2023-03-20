// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Store : BaseDeletableModel<string>
    {
        public Store()
        {
            Id = Guid.NewGuid().ToString();

            Pets = new HashSet<Pet>();
            Products = new HashSet<Product>();
            Services = new HashSet<Service>();
        }

        [Required]
        [MaxLength(StoreValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(StoreValidationConstants.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Address))]
        public string AddressId { get; set; } = null!;

        public virtual Address Address { get; set; } = null!;

        public virtual ICollection<Pet> Pets { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
