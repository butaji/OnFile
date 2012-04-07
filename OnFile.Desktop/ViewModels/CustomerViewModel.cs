using System;
using OnFile.Domain;
using ServiceStack.Text;

namespace OnFile.Desktop.ViewModels
{
    public class CustomerViewModel : ViewModel
    {
        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { SetField(ref _id, value, "Id"); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetField(ref _name, value, "Name"); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetField(ref _phone, value, "Phone"); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetField(ref _address, value, "Address"); }
        }

        private string _email;
        private int _version;

        public string Email
        {
            get { return _email; }
            set { SetField(ref _email, value, "Email"); }
        }

        public int Version
        {
            get
            {
                return _version;
            }
            set { SetField(ref _version, value, "Version"); }
        }

        public static CustomerViewModel Create(CustomerReadModel readModel)
        {
            var model = readModel.ToJson().FromJson<CustomerViewModel>();
            return model;
        }
    }
}