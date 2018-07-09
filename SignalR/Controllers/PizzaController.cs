using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.Helper;

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

        [HttpGet("LongPolling/{orderNo:int}")]
        public IActionResult GetUpdateForOrderLongPolling(int orderNo)
        {
            CheckResult result;
            //Loop to keep checking for an update until there is one
            do
            {
                result = _orderChecker.GetUpdate(orderNo);
                //Wait 3 seconds after each check
                Thread.Sleep(3000);
            } while (!result.New);
            return new ObjectResult(result);
        }

        [HttpGet("SSE/{orderNo:int}")]
        public async void GetOrderUpdateUsingSSE(int orderNo)
        {
            Response.ContentType = "text/event-stream";
            CheckResult result;
            do
            {
                result = _orderChecker.GetUpdate(orderNo);
                Thread.Sleep(3000);

                if (!result.New) continue;

                await HttpContext.Response.WriteAsync("event: message\n"
                    + "data: " + result.Update + "\n\n");

                await HttpContext.Response.Body.FlushAsync();

            } while (!result.Finished);

            Response.Body.Close();
        }
    }
}
