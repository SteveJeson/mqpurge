using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive.Model
{
    class SubConsumer : DefaultBasicConsumer
    {
        public SubConsumer(IModel model) : base(model)
        {
            var consumer = new EventingBasicConsumer(model);
        }
    }
}
