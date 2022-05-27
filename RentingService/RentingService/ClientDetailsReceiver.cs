using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentingService.Model;
using System.Text;

namespace RentingService;

    public class ClientDetailsReceiver : BackgroundService
    {
    private readonly Data.RentingServiceContext _context; // ?????
    private IServiceProvider _sp;
    private ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;
    public ClientDetailsReceiver(IServiceProvider sp)
    {
        _sp = sp;
        _factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: "client_details", durable: true, exclusive: false, autoDelete: false, arguments: null);
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

                var clientdetails = new ClientDetails();
                if (chunks.Length == 4)
                {
                    clientdetails.ClientId = new Guid(chunks[0]);
                    clientdetails.ClientName = chunks[1];
                    clientdetails.ClientSurname = chunks[2];
                    clientdetails.ClientDeposit = float.Parse(chunks[3]);
                }
                Console.WriteLine(clientdetails.ToString());

                using (var scope = _sp.CreateScope())
                {
                        _context.Add(clientdetails);
                        await _context.SaveChangesAsync();
                }
            });
        };
            _channel.BasicConsume(queue: "client_details", autoAck: true, consumer: consumer);
        return Task.CompletedTask;
        }
    }
