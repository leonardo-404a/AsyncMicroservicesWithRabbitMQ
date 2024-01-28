namespace SharedLib;

public interface IEventHandler<TEvent>
{
    void Handle(TEvent @event);
}
