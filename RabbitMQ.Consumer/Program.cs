//Bagalnti Olusturma
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://mbnjciig:pyWEy4hPFEOr0FY7lEe1SmJos7Emj7MB@toad.rmq.cloudamqp.com/mbnjciig");

//Baglanti Aktifleştirme ve Kanal Açma,
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//Queue Oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false); //Consumer'da da kuyurk publisher'daki ile birebir aynı yapılandırma tanımlanmalıdır.


//Queue'dan Mesaj Oluşturma
EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
//AutoAck = true ise mesajlar otomatik olarak silinir.
channel.BasicConsume(queue: "example-queue",false, consumer);
consumer.Received +=(sender, e) =>
{
    //Kuyruga gelen mesajin islendigi yerdir.
    //e.Body : Kuyruktaki mesajin verisini bütünsel olarak getirecketir.
    //Eger kuyruktaki mesajin icindeki byte veriyi elde etmek isityorsan span veya toArray metodu kullanılmalıdır.
    //e.Body.Span veya e.Body.ToArray() : Kuyruktakim esajın byte verisini getirecektir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

Console.Read();