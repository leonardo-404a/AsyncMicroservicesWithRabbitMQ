namespace SharedLib;

public interface IBusSubscriber
{
    void Subscribe<TEvent>(Func<IEventHandler<TEvent>> handlerFactory) where TEvent : class;
}
