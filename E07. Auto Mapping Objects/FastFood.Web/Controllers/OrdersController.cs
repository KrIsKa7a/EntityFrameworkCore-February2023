namespace FastFood.Web.Controllers
{
    using System;
    using AutoMapper;
    using FastFood.Data;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext _context;
        private readonly IMapper _mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            //var viewOrder = new CreateOrderViewModel
            //{
            //    Items = _context.Items.Select(x => x.Id).ToList(),
            //    Employees = _context.Employees.Select(x => x.Id).ToList(),
            //};

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            return RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            throw new NotImplementedException();
        }
    }
}
