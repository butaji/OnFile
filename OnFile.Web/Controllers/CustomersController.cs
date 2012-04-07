using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using OnFile.Domain;
using OnFile.Infra;
using OnFile.Web.Models;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using HttpRequestWrapper = ServiceStack.WebHost.Endpoints.Extensions.HttpRequestWrapper;

namespace OnFile.Web.Controllers
{
    public class CustomersController : ServiceBase<CustomersResponse>
    {
        private object Get(CustomersResponse request)
        {
            return ServiceLocator.Instance.Data.GetCustomers();
        }

        private object Put(CustomersResponse request)
        {
            var customers = ServiceLocator.Instance.Data.GetCustomers();

            var bus = ServiceLocator.Instance.Bus;
            foreach (var @new in request.Customers)
            {
                var old = customers.First(x => x.Id == @new.Id);

                var changes = GetChanges(old, @new);

                foreach (var change in changes)
                {
                    bus.Send(change);
                }
            }

            return request;
        }

        private IEnumerable<ChangeCustomerInfoCommand> GetChanges(
            CustomerReadModel old, CustomerReadModel @new)
        {
            var props = TypeDescriptor.GetProperties(typeof(CustomerReadModel));
            foreach (PropertyDescriptor prop in props)
            {
                var oldValue = prop.GetValue(old);
                var newValue = prop.GetValue(@new);

                if (!object.Equals(oldValue,newValue))
                    yield return new ChangeCustomerInfoCommand(
                        @new.Id, prop.Name, newValue, old.Version);
            }
        }

        private object Post(CustomersResponse request)
        {
            foreach (var customer in request.Customers)
            {
                ServiceLocator.Instance.Bus.Send(new CreateCustomerCommand(
                    Guid.NewGuid(), customer.Name, customer.Address,
                                   customer.Phone, customer.Email));
            }

            return request;
        }

        private object Delete(CustomersResponse request)
        {
            foreach (var customer in request.Customers)
            {
                ServiceLocator.Instance.Bus.Send(new RemoveCustomerCommand(
                    customer.Id, customer.Version));
            }

            return request;
        }


        protected override object Run(CustomersResponse request)
        {
            var httpReq = base.RequestContext.Get<IHttpRequest>();
            var aspNetRequest = ((HttpRequestWrapper)httpReq).Request;

            return RouteTo(aspNetRequest.RequestType, request);
        }

        private object RouteTo(string requestType, CustomersResponse request)
        {
            switch (requestType)
            {
                case "GET":
                    return Get(Check(request));

                case "POST":
                    return Post(Check(request));

                case "PUT":
                    return Put(Check(request));

                case "DELETE":
                    return Delete(Check(request));

                default:
                    throw new ArgumentException();
            }
        }


        private CustomersResponse Check(CustomersResponse request)
        {
            var httpReq = base.RequestContext.Get<IHttpRequest>();
            var aspNetRequest = ((HttpRequestWrapper)httpReq).Request;
            var json = HttpUtility.UrlDecode(aspNetRequest["Customers"]);

            request.Customers = json.FromJson<List<CustomerReadModel>>();

            return request;
        }
    }
}