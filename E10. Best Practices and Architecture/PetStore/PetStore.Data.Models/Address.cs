// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Common;

    public class Address : BaseDeletableModel<string>
    {
        public Address()
        {
            Id = Guid.NewGuid().ToString();

            Clients = new HashSet<Client>();
        }

        [Required]
        [MaxLength(AddressValidationConstants.TextMaxLength)]
        public string AddressText { get; set; } = null!;

        [Required]
        [MaxLength(AddressValidationConstants.TownNameMaxLength)]
        public string TownName { get; set; } = null!;

        public virtual ICollection<Client> Clients { get; set; }
    }
}
