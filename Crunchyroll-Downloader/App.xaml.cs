using System.Windows;
using CrunchyrollDownloader.Views;

namespace CrunchyrollDownloader
{
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
}