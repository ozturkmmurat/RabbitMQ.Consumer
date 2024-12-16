//Bagalnti Olusturma
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


//6 DERS DERS

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktifleştirme ve Kanal Açma,
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1 Adım : Publisher da ki exchange ile birebir aynı isim ve type'a sahip bir exchange tanımlanmalıdır !
//2 Adım : Publisher tarafından routing key'de bulunan değerdeki kuyruğa gönderilen mesajları kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemiz gerekmektedir. 
// Bunun için çncelikle bir kuyruk oluşturulmalıdır.

//3 Adım : RabbitMq da bir queue(kuyruk) ile bir exchange arasında bağlantı kurmak için kullanılan bir işlemdir
//Bu bağlantı sayesinde, belirli bir routing key ile gönderilen mesajlar, ilgili kuyruğa yönlendirilir.

// 1 Adım

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//2 Adım
string queueName = channel.QueueDeclare().QueueName;

//3 Adım
channel.QueueBind(queue: queueName, exchange: "direct-exchange-example", routingKey: "direct-exchange-example");

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    //Gelen mesajları ayırt edip alıyoruz
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();