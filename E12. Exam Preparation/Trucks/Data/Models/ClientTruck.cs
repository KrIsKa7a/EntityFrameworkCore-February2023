namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClientTruck
    {
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; } = null!;

        [ForeignKey(nameof(Truck))]
        public int TruckId { get; set; }

        public virtual Truck Truck { get; set; } = null!;
    }
}
