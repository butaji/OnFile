using System;

namespace OnFile.Domain
{
    public class CustomerReadModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
        
        public string Email { get; set; }

        public int Version { get; set; }
    }
}