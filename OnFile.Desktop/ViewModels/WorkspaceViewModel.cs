using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using OnFile.Desktop.Helpers;
using OnFile.Domain;

namespace OnFile.Desktop.ViewModels
{
    public class WorkspaceViewModel : ViewModel
    {
        private readonly List<Command> _commands;
        private DispatcherTimer _timer;

        public WorkspaceViewModel()
        {
            _commands = new List<Command>();

            _customers = new ObservableCollection<CustomerViewModel>();
            _customers.CollectionChanged += CustomersChanged;

            RefreshData();

            AddCommand = new ViewCommand(Add);
            SaveCommand = new ViewCommand(Commit);
            DiscardCommand = new ViewCommand(Discard);

            LoadChangesCommand = new ViewCommand(CanLoadChanges, LoadChanges);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Tick += (s, e) => LoadChangesCommand.CanExecuteMethod = CanLoadChanges;
            _timer.Start();
        }

        private void LoadChanges()
        {
            RefreshData();
        }

        private bool CanLoadChanges()
        {
            var dateTimeOffset = ServiceLocator.Data.GetLastChangesdate();
            return dateTimeOffset > Changed;
        }

        private void RefreshData()
        {
            _customers.Clear();

            var customers = ServiceLocator.Data.GetCustomers().Select(CustomerViewModel.Create);
            foreach (var customer in customers)
            {
                customer.PropertyChanged += CustomerPropertyChanged;
                _customers.Add(customer);
            }

            _commands.Clear();

            Changed = ServiceLocator.Data.GetLastChangesdate();
        }

        private void Discard()
        {
            RefreshData();
        }

        private void Commit()
        {
            Changed = null;

            ThreadPool.QueueUserWorkItem(t =>
            {
                foreach (var command in _commands)
                {
                    ServiceLocator.Bus.Send(command);
                }

                _commands.Clear();

                Application.Current.Dispatcher.BeginInvoke(new Action(RefreshData));
            });
        }

        private void Add()
        {
            var item = new CustomerViewModel { Id = Guid.NewGuid() };
            item.PropertyChanged += CustomerPropertyChanged;
            _customers.Add(item);
        }

        private readonly ObservableCollection<CustomerViewModel> _customers;

        public ObservableCollection<CustomerViewModel> Customers
        {
            get { return _customers; }
        }


        private DateTimeOffset? _changed;

        public DateTimeOffset? Changed
        {
            get { return _changed; }
            set { SetField(ref _changed, value, "Changed"); }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DiscardCommand { get; private set; }
        public ViewCommand LoadChangesCommand { get; private set; }

        void CustomerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var customer = (CustomerViewModel)sender;
            _commands.Add(new ChangeCustomerInfoCommand(
                customer.Id, e.PropertyName,
                TypeHelper.PropertyValue<object>(customer, e.PropertyName),
                customer.Version));
        }

        void CustomersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        var customer = (CustomerViewModel)newItem;
                        _commands.Add(new CreateCustomerCommand(customer.Id,
                            customer.Name, customer.Address,
                            customer.Phone, customer.Email));
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        var customer = (CustomerViewModel)oldItem;
                        _commands.Add(new RemoveCustomerCommand(customer.Id, customer.Version));
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}