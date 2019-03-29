using System.Windows;

namespace CrunchyrollDownloader
{
<<<<<<< HEAD
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
	}
=======
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Installer _installer = new Installer();
        protected override async void OnStartup(StartupEventArgs e)
        {
            if (!_installer.CheckIfInstalled())
            {
                await _installer.InstallAll();
            }
            new MainWindow().Show();
        }
    }
>>>>>>> 644ec605da8b6d0107da2185f650523b05f35e2a
}