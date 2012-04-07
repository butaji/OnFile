using System.Windows;
using OnFile.Infra;

namespace OnFile.Desktop
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Bootstrapper.Run();

        }
    }
}
