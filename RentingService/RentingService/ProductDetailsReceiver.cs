using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentingService.Model;
using System.Text;

namespace RentingService;

public class ProductDetailsReceiver : BackgroundService
{
    private readonly Data.RentingServiceContext _context; // ?????
    private IServiceProvider _sp;
    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    public ProductDetailsReceiver(IServiceProvider sp)
    {
        _sp = sp;
        _factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "product_details", durable: true, exclusive: false, autoDelete: false, arguments: null);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // when the service is stopping
        // dispose these references
        // to prevent leaks
        if (stoppingToken.IsCancellationRequested)
        {
            _channel.Dispose();
            _connection.Dispose();
            return Task.CompletedTask;
        }
        var consumer = new EventingBasicConsumer(_channel);
        // handle the Received event on the consumer
        // this is triggered whenever a new message
        // is added to the queue by the producer
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("[x] Received {0}", message);

            Task.Run(async () =>
            {
                // split the incoming message
                // into chunks which are inserted
                // into respective columns of the Heroes table
                var chunks = message.Split("|");

                var productdetails = new ProductDetails();
                if (chunks.Length == 2)
                {
                    productdetails.ProductId = new Guid(chunks[0]);
                    productdetails.Usage = int.Parse(chunks[1]);
                }
                Console.WriteLine(productdetails.ToString());

                using (var scope = _sp.CreateScope())
                {
                    _context.Add(productdetails);
                    await _context.SaveChangesAsync();
                }
            });
        };
        _channel.BasicConsume(queue: "product_details", autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
}
