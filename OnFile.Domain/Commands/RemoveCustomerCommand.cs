using System;

namespace OnFile.Domain
{
    public class RemoveCustomerCommand : Command
    {
        public readonly Guid Id;
        public readonly int Version;

        public RemoveCustomerCommand(Guid id, int version)
        {
            Id = id;
            Version = version;
        }
    }
}