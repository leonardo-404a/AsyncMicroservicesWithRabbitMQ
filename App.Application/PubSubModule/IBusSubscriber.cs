using PizzaPlace.Domain.PubSubModule.Event;

namespace PizzaPlace.Domain.PubSubModule;

public interface IBusSubscriber
{
    void Subscribe<TEvent>(Func<IEventHandler<TEvent>> handlerFactory) where TEvent : class;
}
