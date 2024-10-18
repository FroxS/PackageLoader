using PackageLoader.ViewModel;
using System.Windows;

namespace PackageLoader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow();
            window.DataContext = new MainWindowModel();
            Current.MainWindow = window;
            Current.MainWindow.ShowDialog();

        }

    }
}
