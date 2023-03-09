namespace FastFood.Web.ViewModels.Orders
{
    public class OrderAllViewModel
    {
        public int OrderId { get; set; }

        public string Customer { get; set; } = null!;

        public string Employee { get; set; } = null!;

        public string DateTime { get; set; } = null!;
    }
}
