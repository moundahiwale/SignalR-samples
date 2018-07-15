using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR.Helper;

namespace SignalR.Hubs
{
    public class PizzaHub : Hub
    {
        private readonly OrderChecker _orderChecker;

        public PizzaHub(OrderChecker orderChecker)
        {
            _orderChecker = orderChecker;
        }

        public async Task GetUpdateForOrder(int orderId)
        {
            CheckResult result;
            do 
            {
                result = _orderChecker.GetUpdate(orderId);
                Thread.Sleep(1000);
                if(result.New)
                    // Other options Clients.All, Group, Other (all except caller), etc
                    await Clients.Caller.SendAsync("ReceiveOrderUpdate", result.Update);
            } while (!result.Finished);

            await Clients.Caller.SendAsync("Finished");
        }
    }
}