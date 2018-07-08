using Microsoft.AspNetCore.Mvc;
using SignalR.Helper;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalR.Controllers
{
    public class PizzaController : Controller
    {
        private readonly OrderChecker _orderChecker;

        public PizzaController(OrderChecker orderChecker)
        {
            _orderChecker = orderChecker;
        }

        [HttpPost]
        public IActionResult OrderPizza()
        {
            return Accepted(1); //Order Id
        }

        [HttpGet]
        public IActionResult GetUpdateForOrder(int orderNo)
        {
            var result = _orderChecker.GetUpdate(orderNo);
            if (result.New)
                return new ObjectResult(result);
            return NoContent();
        }
    }
}
