namespace OnFile.Domain
{
    public interface IHandles<T>
    {
        void Handle(T message);
    }
}