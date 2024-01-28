using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace SharedLib;

public class EventBus : IBusPublisher, IBusSubscriber, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public EventBus(EventBusConfig config)
    {
        var factory = new ConnectionFactory() { HostName = config.HostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();

        GC.SuppressFinalize(this);
    }

    public void Publish<TEvent>(TEvent @event)
    {
        ExecuteWithRetry(() =>
        {
            var exchangeName = typeof(TEvent).Name;

            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: exchangeName, routingKey: string.Empty, basicProperties: null, body: body);
        });
    }

    public void Subscribe<TEvent>(Func<IEventHandler<TEvent>> handlerFactory) where TEvent : class
    {
        var exchangeName = typeof(TEvent).Name;
        _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) => ExecuteWithRetry(() =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var @event = JsonSerializer.Deserialize<TEvent>(message);

            var handler = handlerFactory();
            handler.Handle(@event!);
        });

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    private static void ExecuteWithRetry(Action action, int maxRetries = 3)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(maxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .Execute(action);
    }
}
