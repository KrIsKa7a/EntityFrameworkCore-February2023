// ReSharper disable RedundantNameQualifier
namespace PetStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using PetStore.Data.Common.Models;
    using PetStore.Data.Models.Enums;

    public class Order : BaseDeletableModel<string>
    {
        public Order()
        {
            Id = Guid.NewGuid().ToString();

            Products = new HashSet<Product>();
            Services = new HashSet<Service>();
        }

        public DateTime DateTime { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public decimal TotalPrice { get; set; }

        [Required]
        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; } = null!;

        public virtual Client Client { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
