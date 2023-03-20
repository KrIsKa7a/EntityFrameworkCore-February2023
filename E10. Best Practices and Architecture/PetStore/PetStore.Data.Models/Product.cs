// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Product : BaseDeletableModel<string>
    {
        public Product()
        {
            Id = Guid.NewGuid().ToString();

            Stores = new HashSet<Store>();
            Orders = new HashSet<Order>();
        }

        [Required]
        [MaxLength(ProductValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Store> Stores { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
