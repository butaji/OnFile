using System;

namespace OnFile.Domain.Events
{
    public class CustomerCreated : Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public CustomerCreated(Guid id, string name, string address, string email, string phone,
            int version)
        {
            Id = id;
            Name = name;
            Address = address;
            Email = email;
            Phone = phone;
            Version = version;
        }
    }
}