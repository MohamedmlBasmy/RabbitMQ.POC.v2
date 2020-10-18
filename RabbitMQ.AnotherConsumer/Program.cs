using System;
using System.Threading.Tasks;
using MassTransit;
using Models;
using RabbitMQ.Client;
using RabbitMQ.Consumer.Consumers;

namespace RabbitMQ.AnotherConsumer
{
   public class Program
    {
        static async Task Main(string[] args) {

            var bus  = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                //cfg.Publish<AnotherOrderConsumer>(x => x.ExchangeType = ExchangeType.Direct);

                cfg.Publish<Order>(config => config.ExchangeType = ExchangeType.Direct);

                cfg.Host("rabbitmq://localhost");

                cfg.ReceiveEndpoint("order-queue-direct-a", ep =>
                {
                    //ep.PrefetchCount = 16;
                    //ep.UseMessageRetry(r => r.Interval(2, 100));
                    ep.ConfigureConsumeTopology = false;

                    ep.Bind<Order>(bind =>
                    {
                        bind.RoutingKey = "A";
                        bind.ExchangeType = ExchangeType.Direct;
                    });

                    ep.Consumer<AnotherOrderConsumer>();
                });

                cfg.ReceiveEndpoint("order-queue-direct-b", ep =>
                {
                    //ep.PrefetchCount = 16;
                    //ep.UseMessageRetry(r => r.Interval(2, 100));
                    ep.ConfigureConsumeTopology = false;

                    ep.Bind<Order>(bind =>
                    {
                        bind.RoutingKey = "B";
                        bind.ExchangeType = ExchangeType.Direct;
                    });

                    ep.Consumer<ThirdConsumer>();
                });


            });

            await bus.StartAsync();

            await bus.Publish<Order>(new Order()
            {
                Id = 6,
                Qty = 25,
                ProductName = "product 1"
            }, x => x.SetRoutingKey("A"));

            await bus.Publish<Order>(new Order()
            {
                Id = 6,
                Qty = 25,
                ProductName = "product 2"
            }, x => x.SetRoutingKey("B"));

            //var endPoint = await bus.GetSendEndpoint(new Uri("queue:order-queue"));
            //await endPoint.Send<Order>(new Order()
            //{
            //    Id = 6,
            //    Qty = 25,
            //    ProductName = "product 1"
            //});

            await bus.StopAsync();
        }
    }
}
