namespace PizzaPlace.Domain.PubSubModule.Event;

public interface IEventHandler<TEvent>
{
    void Handle(TEvent @event);
}
