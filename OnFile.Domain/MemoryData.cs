using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OnFile.Domain
{
    public class MemoryData : IData<CustomerReadModel>
    {
        private DateTimeOffset? _lastChange;

        private readonly List<CustomerReadModel> Customers = new List<CustomerReadModel>();

        public IEnumerable<CustomerReadModel> GetCustomers()
        {
            return Customers;
        }

        public DateTimeOffset? GetLastChangesdate()
        {
            return _lastChange;
        }

        public void AddCustomer(Guid id, string name, string address,
            string email, string phone, int version)
        {
            Customers.Add(new CustomerReadModel
                {
                    Id = id,
                    Name = name,
                    Address = address,
                    Email = email,
                    Phone = phone,
                    Version = version
                });

            _lastChange = DateTime.Now;
        }

        public void ChangeInfo(Guid id, string name, object value, int version)
        {
            var customer = Get(id);

            var prop = TypeDescriptor.GetProperties(customer).Find(name, true);
            prop.SetValue(customer, value);
            
            customer.Version = version;

            _lastChange = DateTime.Now;
        }

        private CustomerReadModel Get(Guid id)
        {
            return Customers.First(x => x.Id == id);
        }

        public void Remove(Guid id)
        {
            var customer = Get(id);
            Customers.Remove(customer);
        }
    }
}