using MassTransit;
using RabbitMqESBMassTransitShared.Messages;

string rabbitMqUri = "amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig";

string queueName = "example-queue2";


//RabbitMQ üzerinden operasyon yürüteceğimizi bildiriyoruz.
IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMqUri);
});

await bus.StartAsync();

//Mesaji send etme örneği

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(
    new($"{rabbitMqUri}/{queueName}"));

Console.Write("Gönderilecek Mesaj : ");
string message = Console.ReadLine();
await sendEndpoint.Send<IMessage>(new ExampleMessage()
{
    Text = message
});

Console.Read();

