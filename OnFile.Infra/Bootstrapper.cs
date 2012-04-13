using OnFile.Domain;
using OnFile.Domain.Events;
using OnFile.Domain.ReadModel;
using OnFile.Infra.Properties;
using OnFile.Storage;

namespace OnFile.Infra
{
    public class Bootstrapper
    {
        public static void Run<T>(T application)
        {
            var bus = new FakeBus();

            var storage = new DirectoryEventStore(bus, Settings.Default.WorkDir);
            var rep = new Repository<Customer>(storage);

            var commands = new CustomerCommandHandlers(rep);
            bus.RegisterHandler<CreateCustomerCommand>(commands.Handle);
            bus.RegisterHandler<ChangeCustomerInfoCommand>(commands.Handle);
            bus.RegisterHandler<RemoveCustomerCommand>(commands.Handle);

            var data = new MemoryData(typeof(T).FullName);

            var events = new CustomerEventsHandler(data);
            bus.RegisterHandler<CustomerCreated>(events.Handle);
            bus.RegisterHandler<CustomerInfoChanged>(events.Handle);
            bus.RegisterHandler<CustomerDeleted>(events.Handle);

            ServiceLocator.Instance.Bus = bus;

            ServiceLocator.Instance.Data = data;

            storage.LoadEvents();

        }
    }
}