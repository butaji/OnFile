using System;
using OnFile.Domain.Events;

namespace OnFile.Domain
{
    public class Customer : AggregateRoot
    {
        private Guid _id;
        private bool _deleted;

        public Customer()
        {
        }

        private void Apply(CustomerCreated e)
        {
            _id = e.Id;
        }

        public Customer(Guid id, string name, string address, 
            string email, string phone, int version)
        {
            ApplyChange(new CustomerCreated(id, name, address, email, phone, version));
        }

        public override Guid Id
        {
            get { return _id; }
        }

        public void Change(string name, object value)
        {
            if (_deleted) throw new MethodAccessException();
            ApplyChange(new CustomerInfoChanged(_id, name, value, ++Version));
        }

        public void Delete()
        {
            _deleted = true;
            ApplyChange(new CustomerDeleted(_id));
        }
    }
}