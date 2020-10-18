using System;
using System.Threading.Tasks;
using MassTransit;
using Models;

namespace RabbitMQ.AnotherConsumer
{
    public class AnotherOrderConsumer :
      IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context) {
            // message received.
            
            Console.WriteLine("From Another Consumer Command Received {0} on {1}", context.Message.ProductName,
                context.ReceiveContext.InputAddress);

            await Task.CompletedTask;
        }
    }
}
