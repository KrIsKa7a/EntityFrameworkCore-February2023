namespace CarDealer.Models
{
    public class Supplier
    {
        public Supplier()
        {
            this.Parts = new HashSet<Part>();
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsImporter { get; set; }

        public virtual ICollection<Part> Parts { get; set; }
    }
}
