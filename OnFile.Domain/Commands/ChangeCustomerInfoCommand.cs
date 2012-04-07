using System;

namespace OnFile.Domain
{
    public class ChangeCustomerInfoCommand : Command
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly object Value;
        public readonly int Version;

        public ChangeCustomerInfoCommand(Guid id, string name, object value, int version)
        {
            Id = id;
            Name = name;
            Value = value;
            Version = version;
        }
    }
}