namespace PizzaPlace.Domain.PubSubModule;

public interface IBusPublisher
{
    void Publish<TEvent>(TEvent @event);
}
