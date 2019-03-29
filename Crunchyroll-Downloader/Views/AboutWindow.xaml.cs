using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using CrunchyrollDownloader;

namespace Crunchyroll_Downloader
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

		private void button_Click(object sender, RoutedEventArgs e)
		{
			UpdateYtdl();
		}

		private void button_Click2(object sender, RoutedEventArgs e)
		{
			UpdateYtdl();
		}

		private async void UpdateYtdl()
		{
			var process = new Process
			{
				StartInfo =
				{
					FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe",
					Arguments = "-U",
					WindowStyle = ProcessWindowStyle.Normal
				}
			};
			// Configure the process using the StartInfo properties.
			process.Start();
			await Task.Run(() => process.WaitForExit()); // Waits here for the process to exit. Without any thread blocks.
		}
	}
}
