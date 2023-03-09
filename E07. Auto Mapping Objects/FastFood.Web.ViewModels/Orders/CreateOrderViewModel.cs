namespace FastFood.Web.ViewModels.Orders
{
    using System.Collections.Generic;

    public class CreateOrderViewModel
    {
        public List<int> Items { get; set; } = null!;

        public List<int> Employees { get; set; } = null!;
    }
}
