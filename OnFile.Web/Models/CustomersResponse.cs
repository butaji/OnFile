using System.Collections.Generic;
using OnFile.Domain;
using OnFile.Infra;
using ServiceStack.ServiceInterface.ServiceModel;

namespace OnFile.Web.Models
{
    public class CustomersResponse : IHasResponseStatus, IHasAction
    {
        public CustomersResponse()
        {
            this.ResponseStatus = new ResponseStatus();
            this.Customers = new List<CustomerReadModel>();
        }

        public List<CustomerReadModel> Customers { get; set; }

        public ResponseStatus ResponseStatus { get; set; }

        public string Action { get; set; }
    }
}