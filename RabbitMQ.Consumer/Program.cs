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
channel.BasicConsume(queue: "example-queue",autoAck:false, consumer);

//FairDispatch BasicQos konfigürasyonu
//Birinci parametre consumer tarafındn alınabilecek en büyük mesaj boyutunu byte cinsinden belirler. 0 sınırsız demektir.
//İkinci parametre bir consumer tarafından aynı anda işleme alınabilecek mesaj sayını belirler.
//Üçüncü parametre bu konfigürasyonun tüm consumerlar için mi yoksa sadece çağrı yapılan consumer için mi geçerli olacağını belirler.
channel.BasicQos(prefetchSize:0, prefetchCount: 1, global: false);

consumer.Received +=(sender, e) =>
{
    //Kuyruga gelen mesajin islendigi yerdir.
    //e.Body : Kuyruktaki mesajin verisini bütünsel olarak getirecketir.
    //Eger kuyruktaki mesajin icindeki byte veriyi elde etmek isityorsan span veya toArray metodu kullanılmalıdır.
    //e.Body.Span veya e.Body.ToArray() : Kuyruktakim esajın byte verisini getirecektir.
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));


    //Mesajı başarılı şekilde işlediğimizi bildiriyoruz.
    //DeliveryTag id gibi düşünebilirsin hangi mesajı başarılı şekilde işlediğimizi belirtmek için
    // 2 Parametre eğer false ise sadece şuan ki mesajı başarılı şekilde işlediğnii bildiririz
    // Eğer true olur ise şuan ki mesaj ve bu mesajdan önceki mesajların da başarılı şekilde işlendiğini bildiririz.
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple:false);
};

Console.Read();