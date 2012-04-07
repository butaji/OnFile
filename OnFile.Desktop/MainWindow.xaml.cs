using System.Windows.Controls;
using System.Windows.Input;
using OnFile.Desktop.ViewModels;

namespace OnFile.Desktop
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = new WorkspaceViewModel();
            InitializeComponent();
            Grid.Focus();

            CommandManager.RegisterClassInputBinding(
                      typeof(DataGrid),
                new InputBinding(DataGrid.BeginEditCommand, new KeyGesture(Key.F2)));

            CommandManager.RegisterClassInputBinding(
                      typeof(DataGrid),
                new InputBinding(DataGrid.CancelEditCommand, new KeyGesture(Key.Escape)));

            CommandManager.RegisterClassInputBinding(
                      typeof(DataGrid),
                new InputBinding(DataGrid.DeleteCommand, new KeyGesture(Key.Delete)));

            CommandManager.RegisterClassInputBinding(
                      typeof(DataGrid),
                new InputBinding(DataGrid.CommitEditCommand, new KeyGesture(Key.Enter)));
        }
    }
}
