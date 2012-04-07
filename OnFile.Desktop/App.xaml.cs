using System.Windows;
using OnFile.Desktop.Properties;
using OnFile.Domain;
using OnFile.Domain.Events;
using OnFile.Storage;

namespace OnFile.Desktop
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bus = new FakeBus();

            var storage = new DirectoryEventStore(bus, Settings.Default.WorkDir);
            var rep = new Repository<Customer>(storage);
           
            var commands = new CustomerCommandHandlers(rep);
            bus.RegisterHandler<CreateCustomerCommand>(commands.Handle);
            bus.RegisterHandler<ChangeCustomerInfoCommand>(commands.Handle);
            bus.RegisterHandler<RemoveCustomerCommand>(commands.Handle);

            var data = new MemoryData();

            var events = new CustomerEventsHandler(data);
            bus.RegisterHandler<CustomerCreated>(events.Handle);
            bus.RegisterHandler<CustomerInfoChanged>(events.Handle);
            bus.RegisterHandler<CustomerDeleted>(events.Handle);

            ServiceLocator.Bus = bus;

            ServiceLocator.Data = data;

            storage.LoadEvents();
        }
    }
}
