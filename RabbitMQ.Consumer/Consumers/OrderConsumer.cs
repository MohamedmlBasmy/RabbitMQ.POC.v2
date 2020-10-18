using System;
using System.Threading.Tasks;
using MassTransit;
using Models;

namespace RabbitMQ.Consumer.Consumers
{
    public class OrderConsumer :
      IConsumer<OrderRequest>
    {
        public async Task Consume(ConsumeContext<OrderRequest> context) {
            // message received.
            //var data = context.Message.ProductName
            //await Console.Out.WriteLineAsync($"Updating customer: {data}");

            throw new Exception("Very bad things happened");
        }
    }
}
