using System;
using Microsoft.AspNetCore.SignalR.Client;
using SignalR.Models;

namespace SignalRConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press a key to start listening!");
            Console.ReadKey();
            var connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/pizzahub")
                .Build();

            // On is generic and we have to specify the type we are expecting - Order
            connection.On<Order>("NewOrder", (order) =>
                Console.WriteLine($"Somebody ordered a {order.Product}"));
            
            // If something goes wrong in connection.StartAsync() an exception is thrown
            // you can catch as normal/could await this
            // Since it is in the method have to do .GetAwaiter().GetResult();
            connection.StartAsync().GetAwaiter().GetResult();

            Console.WriteLine("Listening.. Press a key to quit");
            Console.ReadKey();
        }
    }
}
