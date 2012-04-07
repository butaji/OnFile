using OnFile.Domain;

namespace OnFile.Infra
{
    public class ServiceLocator
    {
        private static readonly ServiceLocator _instance = new ServiceLocator();

        public ICommandSender Bus { get; internal set; }

        public IData<CustomerReadModel> Data { get; internal set; }

        public static ServiceLocator Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}