using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace Receive.Model
{
    class MyConsumer : IBasicConsumer
    {
        IModel IBasicConsumer.Model
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<ConsumerEventArgs> IBasicConsumer.ConsumerCancelled
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        void IBasicConsumer.HandleBasicCancel(string consumerTag)
        {
            throw new NotImplementedException();
        }

        void IBasicConsumer.HandleBasicCancelOk(string consumerTag)
        {
            throw new NotImplementedException();
        }

        void IBasicConsumer.HandleBasicConsumeOk(string consumerTag)
        {
            throw new NotImplementedException();
        }

        void IBasicConsumer.HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            Console.WriteLine(body);
        }

        void IBasicConsumer.HandleModelShutdown(object model, ShutdownEventArgs reason)
        {
            throw new NotImplementedException();
        }
    }
}
