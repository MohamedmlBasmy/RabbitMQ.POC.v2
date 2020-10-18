using MassTransit;
using MassTransit.BusConfigurators;
using MassTransit.Log4NetIntegration.Logging;
using MassTransit.RabbitMqTransport;
using MassTransit.RabbitMqTransport.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace RabbitMQ.Configuration
{
    public class BusInitializer : IBusInitializer
    {
        public IBusControl CreateBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null) {

            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(RabbitMQConstants.RabbitMQUri), hst =>
                {
                    hst.Username(RabbitMQConstants.UserName);
                    hst.Password(RabbitMQConstants.Password);
                });
            });
        }
    }
}
