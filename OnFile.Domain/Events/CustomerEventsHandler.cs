using OnFile.Domain.ReadModel;

namespace OnFile.Domain.Events
{
    public class CustomerEventsHandler : 
        IHandles<CustomerCreated>,
        IHandles<CustomerInfoChanged>,
        IHandles<CustomerDeleted> 
    {
        private readonly MemoryData _storage;

        public CustomerEventsHandler(MemoryData storage)
        {
            _storage = storage;
        }

        public void Handle(CustomerCreated eventData)
        {
            _storage.AddCustomer(eventData.Id, eventData.Name, 
                eventData.Address, eventData.Email, 
                eventData.Phone, eventData.Version);
        }

        public void Handle(CustomerInfoChanged obj)
        {
            _storage.ChangeInfo(obj.Id, obj.Name, obj.Value, obj.Version);
        }

        public void Handle(CustomerDeleted obj)
        {
            _storage.Remove(obj.Id);
        }
    }
}