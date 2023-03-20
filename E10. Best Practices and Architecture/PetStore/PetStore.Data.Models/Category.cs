// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            Pets = new HashSet<Pet>();
            Products = new HashSet<Product>();
        }

        [Required]
        [MaxLength(CategoryValidationConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Pet> Pets { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
