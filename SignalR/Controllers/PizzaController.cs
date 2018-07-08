using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalR.Controllers
{
    public class PizzaController : Controller
    {
        [HttpPost]
        public IActionResult OrderPizza()
        {
            return Accepted(1); //Order Id
        }
    }
}
