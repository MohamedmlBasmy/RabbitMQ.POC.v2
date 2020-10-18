using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace RabbitMQ.POC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IBusControl _bus;
        public OrderController(IBusControl bus) {
            _bus = bus;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderRequest order) {

            await _bus.StartAsync();

            var endPoint = await _bus.GetSendEndpoint(new Uri("queue:order-queue"));

            //Topology cannot alter the destination of a message,
            //only the properties of the message delivery itself.
            //Determining the path of a message is routing, which is handled separately.
            
            await endPoint.Send(order);

            //await _bus.Publish<Order>(order, x=>x.SetRoutingKey("A"));

            //await _bus.StopAsync();

            return Ok();
        }
    }
}
