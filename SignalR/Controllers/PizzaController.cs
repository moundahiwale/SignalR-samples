using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.Helper;
using SignalR.Hubs;
using SignalR.Models;

namespace SignalR.Controllers
{
    public class PizzaController : Controller
    {
        private readonly OrderChecker _orderChecker;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<PizzaHub> _pizzahub; 
        public PizzaController(OrderChecker orderChecker, IHttpContextAccessor httpContextAccessor, IHubContext<PizzaHub> pizzahub)
        {
            _orderChecker = orderChecker;
            _httpContextAccessor = httpContextAccessor;
            _pizzahub = pizzahub;
        }

        [HttpPost]
        public IActionResult OrderPizza()
        {
            return Accepted(1); //Order Id
        }

        [HttpPost]
        public async Task<IActionResult> OrderPizzaSignalR([FromBody] Order order)
        {
            // When injecting hub here, doesn't have context of the caller Clients.Caller & Other
            // Better approach would to be create OrderPizza method in the hub and let clients call it using
            // Signal R instead of an AJAX call. Following is to show hubs can be accessed from everywhere
            await _pizzahub.Clients.All.SendAsync("NewOrder", order);
            // In PROD/actual app, order will be written to the DB, etc.
            return Accepted(1); 
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

        [HttpGet("WS/{orderNo:int}")]
        public async void GetOrderUpdateUsingWebSockets(int orderNo)
        {
            var context = _httpContextAccessor.HttpContext;
            if(context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await sendEvents(webSocket, orderNo);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Done",
                    CancellationToken.None);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        private async Task sendEvents(WebSocket webSocket, int orderNo)
        {
            CheckResult result;

            do
            {
                result = _orderChecker.GetUpdate(orderNo);
                Thread.Sleep(2000);

                if(!result.New) continue;

                var jsonMessage = $"\"{result.Update}\"";
                await webSocket.SendAsync(buffer: new ArraySegment<byte>(
                    array: Encoding.ASCII.GetBytes(jsonMessage),
                    offset: 0,
                    count: jsonMessage.Length),
                    messageType: WebSocketMessageType.Text,
                    endOfMessage: true,
                    cancellationToken: CancellationToken.None);
            } while (!result.Finished);
        }
    }
}
