// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Service : BaseDeletableModel<string>
    {
        public Service()
        {
            Id = Guid.NewGuid().ToString();

            Stores = new HashSet<Store>();
            Orders = new HashSet<Order>();
        }

        [Required]
        [MaxLength(ServiceValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ServiceValidationConstants.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public virtual ICollection<Store> Stores { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
