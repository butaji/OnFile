using System;
using System.Collections.Generic;

namespace OnFile.Domain
{
    public interface IData<T>
    {
        IEnumerable<T> GetCustomers();
        DateTimeOffset? GetLastChangesdate();
    }
}