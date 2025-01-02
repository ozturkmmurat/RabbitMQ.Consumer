using MassTransit;
using RabbitMqESBMassTransitConsumer.Consumers;

string rabbitMqUri = "amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig";

string queueName = "example-queue2";

//RabbitMQ üzerinden operasyon yürüteceğimizi bildiriyoruz.
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMqUri);
    //Consumer görmesini sağlıyoruz.
    factory.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<ExampleMessageConsumer>();
    });
});

//Bus'ın çalışmasını sağlıyoruz.
await bus.StartAsync();

Console.Read();