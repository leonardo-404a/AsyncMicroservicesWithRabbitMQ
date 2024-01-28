namespace SharedLib;
public interface IBusPublisher
{
    void Publish<TEvent>(TEvent @event);
}
