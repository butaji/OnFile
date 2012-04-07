using System;

namespace OnFile.Domain
{
    public class CreateCustomerCommand : Command
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Address;
        public readonly string Phone;
        public readonly string Email;

        public CreateCustomerCommand(Guid id, string name, string address, 
                                     string phone, string email)
        {
            Id = id;
            Name = name;
            Address = address;
            Phone = phone;
            Email = email;
        }
    }
}