using OnFile.Domain;

namespace OnFile.Desktop
{
    public static class ServiceLocator
    {
        public static ICommandSender Bus { get; internal set; }

        public static IData<CustomerReadModel> Data { get; internal set; }
    }
}