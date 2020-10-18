using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using RabbitMQ.Consumer.Consumers;

namespace RabbitMQ.Consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddScoped<OrderConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderConsumer>();
                x.AddConsumer<FaultConsumer>();
                x.AddConsumer<LogFaultMessagesConsumer>();
                //x.AddRequestClient<Order>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    // configure health checks for this bus instance
                    cfg.UseHealthCheck(provider);

                    //cfg.Publish<OrderConsumer>(x=>x.ExchangeType = "Direct");

                    cfg.Host("rabbitmq://localhost");

                    cfg.ReceiveEndpoint("order-queue", ep =>
                    {
                        //ep.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));

                        //ep.UseMessageRetry(r => r.Immediate(2));

                        ep.ConfigureConsumer<OrderConsumer>(provider);
                        ep.ConfigureConsumer<FaultConsumer>(provider);
                    });

                    cfg.ReceiveEndpoint("order-queue_error", ep =>
                    {
                        ep.ConfigureConsumer<LogFaultMessagesConsumer>(provider);
                    });

                }));
            });

            services.AddMassTransitHostedService();

            services.AddControllers();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
