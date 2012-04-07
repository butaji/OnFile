using System;

namespace OnFile.Domain.Events
{
    public class CustomerInfoChanged : Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public CustomerInfoChanged(Guid id, string name, object value, int version)
        {
            Id = id;
            Name = name;
            Value = value;
            Version = version;
        }
    }
}