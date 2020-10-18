using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using Serilog.Core;

namespace RabbitMQ.Consumer.Consumers
{
    public class FaultConsumer : IConsumer<Fault<OrderRequest>>
    {
        private readonly ILogger<FaultConsumer> serviceLogger;
        public async Task Consume(ConsumeContext<Fault<OrderRequest>> context) {
            // log the original message and the exception message to the database 
            var originalMessage = context.Message.Message;
            var exceptions = context.Message.Exceptions;

            Array.ForEach(exceptions, ex => serviceLogger.Log(LogLevel.Error, $"received exception:{ex.Message}"));

            await Console.Out.WriteLineAsync($"discarding message:{JsonConvert.SerializeObject(originalMessage)}");
        }
    }
}
