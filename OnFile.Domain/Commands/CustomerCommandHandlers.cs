using System;

namespace OnFile.Domain
{
    public class CustomerCommandHandlers :
        IHandles<CreateCustomerCommand>,
        IHandles<ChangeCustomerInfoCommand>,
        IHandles<RemoveCustomerCommand>
    {
        private readonly IRepository<Customer> _rep;

        public CustomerCommandHandlers(IRepository<Customer> rep)
        {
            _rep = rep;
        }

        public void Handle(CreateCustomerCommand command)
        {
            const int version = -1;

            var customer = new Customer(
                command.Id, command.Name, command.Address,
                command.Email, command.Phone, version);

            _rep.Save(customer, version);
        }

        public void Handle(ChangeCustomerInfoCommand command)
        {
            var customer = _rep.GetById(command.Id);

            if (customer.Id != command.Id)
                throw new ArgumentException();

            customer.Change(command.Name, command.Value);
            _rep.Save(customer, command.Version + 1);
        }

        public void Handle(RemoveCustomerCommand command)
        {
            var customer = _rep.GetById(command.Id);
            customer.Delete();
            _rep.Save(customer, command.Version);
        }
    }
}