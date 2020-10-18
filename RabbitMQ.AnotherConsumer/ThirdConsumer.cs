using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Models;
using Console = System.Console;

namespace RabbitMQ.AnotherConsumer
{
   public class ThirdConsumer: IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context) {
            // message received.
            Console.WriteLine("From Third Consumer, message {0} on {1}", context.Message.ProductName, context.ReceiveContext.InputAddress);
            await Task.CompletedTask;
        }
    }
}
