using System;

namespace OnFile.Domain.Events
{
    public class CustomerDeleted : Event
    {
        public Guid Id { get; set; }

        public CustomerDeleted(Guid id)
        {
            Id = id;
        }
    }
}