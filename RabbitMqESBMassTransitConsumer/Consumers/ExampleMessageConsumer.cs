using MassTransit;
using RabbitMqESBMassTransitShared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqESBMassTransitConsumer.Consumers
{
    public class ExampleMessageConsumer : IConsumer<IMessage>
    {
        // Ne zaman ilgili kuyruga IMessage türünden mesaj gelirse
        //Consumer Consume metodunu tetikleyecek ve context
        //Uzerinden mesajı elde edip içerisine işlememizi sağlayacak
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Console.WriteLine($"Gelen Mesaj : {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
