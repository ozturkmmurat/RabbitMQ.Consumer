using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktifleştirme ve Kanal Açma,
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point-to-Point) Tasarımı
////Kuyruk adını verdik
//string queueeName = "example-p2p-queue";

////Kuyruğu oluşturduk
//channel.QueueDeclare(
//    queue: queueeName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

////Consumer oluşturuyoruz
//EventingBasicConsumer consumer = new(channel);

//channel.BasicConsume(
//    queue: queueeName,
//    autoAck: false,
//    consumer: consumer
//    );

////Consumer üzerinden kuyruğu tüketiyoruz.
//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};
#endregion

#region Publish/Subscribe (Pub/Sub) Tasarımı
////Exchange adını verdik
//string exchangeName = "example-pub-sub-exchange";

////Exchange oluşturduk
//channel.ExchangeDeclare(
//    exchange: exchangeName,
//    type: ExchangeType.Fanout);

////Kuyruğu oluşturuyoruz ve adını alıyoruz.
//string queueName = channel.QueueDeclare().QueueName;

////Queue exchange bind ediyoruz
//channel.QueueBind(
//    queue:queueName,
//    exchange: exchangeName,
//    routingKey: string.Empty
//    );

////Consumer oluşturuyoruz
//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(
//    queue: queueName,
//    autoAck: false,
//    consumer: consumer);

//////Consumer üzerinden kuyruğu tüketiyoruz.
//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};

////NOT : Publish/Subscribe (Pub/Sub) Tasarımında genelde BasicQos ile ölçeklendirme yapılır 5 ders gelişmiş kuyruk mimarisinde 
////Kullanımını görebilirsin.
#endregion


#region Work Queue(İş Kuyruğu Tasarımı)

//// Kuyruk adı oluşturuldu
//string queueName = "example-work-queue";

////Kuyruk oluşturuyoruz
//channel.QueueDeclare(
//    queue: queueName,
//    durable: false,
//    exclusive: false,
//    autoDelete: false);

////Consumer oluşturuyoruz
//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(
//    queue: queueName,
//    autoAck: true,
//    consumer: consumer);

////Her bir consumer 1 mesaj alabilir ve sınırsız mesaj alabilir diyoruz.
//channel.BasicQos(
//    prefetchCount: 1,
//    prefetchSize: 0,
//    global: false);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};

#endregion

#region Request/Response Tasarımı

//Kuyruk adını oluşturuyoruz
string requestQueueName = "example-request-response-queue";

//Kuyruğu oluşturuyoruz
channel.QueueDeclare(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: requestQueueName,
    autoAck: true,
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

    byte[] responseMessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı : {message}");
    IBasicProperties properties =  channel.CreateBasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;

    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: e.BasicProperties.ReplyTo,
        basicProperties: properties,
        body: responseMessage);
};

#endregion

Console.Read();