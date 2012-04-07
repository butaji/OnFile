namespace OnFile.Domain
{
    public interface Handles<T>
    {
        void Handle(T message);
    }
}