﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Configuration.Contracts
{
    public class Order
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
    }
}
