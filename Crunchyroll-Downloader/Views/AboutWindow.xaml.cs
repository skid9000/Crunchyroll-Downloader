using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using CrunchyrollDownloader.Progress;
using CrunchyrollDownloader.ViewModels;

namespace CrunchyrollDownloader.Views
{
	/// <summary>
	/// About the program
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			await UpdateYtdl();
		}

		private async void button_Click2Async(object sender, RoutedEventArgs e)
		{
			await RedownloadDependencies();
		}

		private async Task UpdateYtdl()
		{
			var process = new Process
			{
				StartInfo =
				{
					FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe",
					Arguments = "-U",
					WindowStyle = ProcessWindowStyle.Hidden
				}
			};
			// Configure the process using the StartInfo properties.
			process.Start();
			var manager = new TaskManager("Updating YTDL...");
			var window = Application.Current.Dispatcher.Invoke(() => new ProgressWindow(new ProgressViewModel
			{
				IsIndeterminate = true,
				Progress = manager
			}));
			_ = Task.Run(() => Application.Current.Dispatcher.Invoke(() => window.ShowDialog()));
			await Task.Run(() => process.WaitForExit()); // Waits here for the process to exit. Without any thread blocks.
			window.Close();
		}

		private readonly Installer _installer = new Installer();
		private async Task RedownloadDependencies()
		{
			_installer.DeleteInstallation();
			await _installer.InstallAll();
		}
	}
}
