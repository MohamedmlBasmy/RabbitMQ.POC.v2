using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;

namespace RabbitMQ.Consumer.Consumers
{
    public class LogFaultMessagesConsumer : IConsumer<OrderRequest>
    {
        private readonly ILogger<LogFaultMessagesConsumer> serviceLogger;

        public async Task Consume(ConsumeContext<OrderRequest> context) {
            var originalMessage = context.Message;

            await Console.Out.WriteLineAsync($"discarding message:{JsonConvert.SerializeObject(originalMessage)}");
        }
    }
}