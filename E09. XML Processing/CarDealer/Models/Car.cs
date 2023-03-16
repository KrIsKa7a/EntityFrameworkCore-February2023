namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            this.Sales = new HashSet<Sale>();
            this.PartsCars = new HashSet<PartCar>();
        }

        public int Id { get; set; }

        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public long TraveledDistance { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }

        public virtual ICollection<PartCar> PartsCars { get; set; }
    }
}
