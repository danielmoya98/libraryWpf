using System.Windows;
using library.Services;

namespace library
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ThemeService.Apply(false); // inicia en Light (false) o Dark (true)
        }
    }
}
