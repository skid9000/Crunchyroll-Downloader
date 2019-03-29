using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using CrunchyrollDownloader.ViewModels;
using Crunchyroll_Downloader;
using MessageBox = System.Windows.MessageBox;

namespace CrunchyrollDownloader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowViewModel _vm;
		private Installer _installer = new Installer();
		public MainWindow()
		{
			DataContext = _vm = new MainWindowViewModel();
			InitializeComponent();
		}
		private void UpdateYtdl()
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
			process.WaitForExit(); // Waits here for the process to exit.
		}

		private void button_Save_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog
			var dialog = new FolderBrowserDialog();
			var result = dialog.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.Cancel || dialog.SelectedPath is null) return;
			_vm.SavePath = dialog.SelectedPath;
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(_vm.Url))
			{
				MessageBox.Show("Please, put a URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (!Uri.TryCreate(_vm.Url, UriKind.Absolute, out var uri) || null == uri)
			{
				//Invalid URL
				MessageBox.Show("Please, put a valid URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (string.IsNullOrEmpty(_vm.SavePath))
			{
				MessageBox.Show("Please, put a save path.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			if (checkBox.IsChecked ?? false)
			{

				if (string.IsNullOrEmpty(_vm.Language))
				{
					MessageBox.Show("Please, choose a language.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
				if (string.IsNullOrEmpty(_vm.Format))
				{
					MessageBox.Show("Please, choose a sub format.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
				_vm.AreSubtitlesEnabled = true;
			}
			if (_vm.HasLogin)
			{
				await _vm.Download();
			}
			else
				MessageBox.Show("Please login.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		private void aboutButton_Click(object sender, RoutedEventArgs e)
		{
			var aboutWindow = new AboutWindow { Owner = this };
			aboutWindow.ShowDialog();
			Focus();
		}

		private void button_login_Click(object sender, RoutedEventArgs e)
		{
			var loginWindow = new LoginWindow(_vm) { Owner = this };
			loginWindow.ShowDialog();
			Focus();
		}

		private void button_logout_Click(object sender, RoutedEventArgs e)
		{
			File.Delete(@"C:\ProgramData\Crunchy-DL\login.json");
			_vm.UpdateLogin();
		}
	}
}