using System;
using System.Collections.Generic;
using System.Web.Http;
using OnFile.Domain;
using OnFile.Infra;

namespace OnFile.Web.Controllers
{
    public class CustomersController : ApiController
    {
        // GET /api/<controller>
        public IEnumerable<CustomerReadModel> Get()
        {
            return ServiceLocator.Instance.Data.GetCustomers();
        }

        // GET /api/<controller>/5
        public CustomerReadModel Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST /api/<controller>
        public void Post(string value)
        {
            throw new NotImplementedException();
        }

        // PUT /api/<controller>/5
        public void Put(int id, string value)
        {
            throw new NotImplementedException();
        }

        // DELETE /api/<controller>/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}