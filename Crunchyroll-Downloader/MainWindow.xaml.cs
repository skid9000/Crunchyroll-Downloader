using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System;
using System.ComponentModel;

namespace CrunchyrollDownloader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        public MainWindow()
		{
			var actualFolder = @"C:\ProgramData\Crunchy-DL";
			using (var client = new WebClient())
			{
				var zip = new FastZip();

				if (Directory.Exists(@"C:\ProgramData\Crunchy-DL"))
				{
					if (File.Exists(@"C:\ProgramData\Crunchy-DL\ffmpeg.exe"))
					{
						if (File.Exists(@"C:\ProgramData\Crunchy-DL\youtube-dl.exe"))
						{
							if (Directory.Exists(@"C:\ProgramData\Crunchy-DL\login"))
							{
								InitializeComponent();
								CheckCookie();
							}
							else
							{
								MessageBox.Show("Dependencies seems corrupted or missing, click OK to re-download them.", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
								Directory.Delete(actualFolder, true);
								InstallAll();
							}
						}
						else
						{
							MessageBox.Show("Dependencies seems corrupted or missing, click OK to re-download them.", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
							Directory.Delete(actualFolder, true);
							InstallAll();
						}
					}
					else
					{
						if (File.Exists(@"C:\ProgramData\Crunchy-DL\ffmpeg.zip"))
						{
							MessageBox.Show("Dependencies seems corrupted or missing, click OK to re-download them.", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
							Directory.Delete(actualFolder, true);
							InstallAll();
						}
						else
						{
							MessageBox.Show("Dependencies seems corrupted or missing, click OK to re-download them.", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
							Directory.Delete(actualFolder, true);
							InstallAll();
						}
					}
				}
				else
					InstallAll();
			}
		}

		private void CheckCookie()
		{
			if (File.Exists(@"C:\ProgramData\Crunchy-DL\cookies.txt"))
			{
				button_login.IsEnabled = false;
				button_logout.IsEnabled = true;
			}
			else
			{
				button_login.IsEnabled = true;
				button_logout.IsEnabled = false;
			}
		}

		private void UpdateYTDL()
		{
			var process = new Process();
			// Configure the process using the StartInfo properties.
			process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe";
			process.StartInfo.Arguments = "-U";
			process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			process.Start();
			process.WaitForExit(); // Waits here for the process to exit.
		}

        public string dl_label { get; private set; }

        private void InstallAll()
		{
            var viewerThread = new Thread(() =>
            {
                var download_window = new DownloadWindow();
                download_window.MyString = dl_label;
                download_window.Show();
                download_window.Activate();
                download_window.Closed += (s, e) =>
                Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                Dispatcher.Run();
            });
            viewerThread.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
            
            var actualFolder = @"C:\ProgramData\Crunchy-DL";
            using (var client = new WebClient())
            {
                var zip = new FastZip();
                MessageBox.Show("Dependencies not detected, downloading ...", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
                Directory.CreateDirectory(@"C:\ProgramData\Crunchy-DL");
                dl_label="[1/3] Downloading dependencies : Youtube-DL ...";
                viewerThread.Start();
                client.DownloadFile("https://yt-dl.org/downloads/latest/youtube-dl.exe", @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
                viewerThread.Abort();
                dl_label = "[2/3] Downloading dependencies : FFmpeg ...";
                viewerThread.Start();
                client.DownloadFile("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffmpeg.zip", @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");
				client.DownloadFile("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffplay.zip", @"C:\ProgramData\Crunchy-DL\ffplay.zip");
				client.DownloadFile("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffprobe.zip", @"C:\ProgramData\Crunchy-DL\ffprobe.zip");
                viewerThread.Abort();
                dl_label = "[3/3] Downloading dependencies : Crunchyroll-Auth ...";
                viewerThread.Start();
                client.DownloadFile("https://github.com/skid9000/CrunchyrollAuth/releases/download/1.0/login.zip", @"C:\ProgramData\Crunchy-DL\login.zip");
                viewerThread.Abort();

                dl_label = "Extracting ...";
                viewerThread.Start();
                zip.ExtractZip(actualFolder + @"\ffmpeg.zip", actualFolder, "");
				zip.ExtractZip(actualFolder + @"\ffplay.zip", actualFolder, "");
				zip.ExtractZip(actualFolder + @"\ffprobe.zip", actualFolder, "");
                zip.ExtractZip(actualFolder + @"\login.zip", actualFolder, "");
                viewerThread.Abort();
                MessageBox.Show("youtube-dl and FFmpeg are now installed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                InitializeComponent();
                CheckCookie();
            }
        }

		private void button_Save_Click(object sender, RoutedEventArgs e)
		{
            // Create OpenFileDialog
                var dialog = new FolderBrowserDialog();
                var result = dialog.ShowDialog();
                if (result ?? false)
                    save_TextBox.Text = dialog.SelectedPath;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			var machin = new Program();

			if (comboBox.Text == "Français (France)")
				machin.Langue = "frFR";
			else if (comboBox.Text == "English (US)")
				machin.Langue = "enUS";
			else if (comboBox.Text == "Español")
				machin.Langue = "esES";
			else if (comboBox.Text == "Español (España)")
				machin.Langue = "esLA";
			else if (comboBox.Text == "Português (Brasil)")
				machin.Langue = "ptBR";
			else if (comboBox.Text == "العربية")
				machin.Langue = "arME";
			else if (comboBox.Text == "Italiano")
				machin.Langue = "itIT";
			else if (comboBox.Text == "Deutsch")
				machin.Langue = "deDE";
			else if (comboBox.Text == "Русский")
				machin.Langue = "ruRU";

            if (QualitycomboBox.Text == "Best (recommended)")
                machin.Quality = "best";
            else if (QualitycomboBox.Text == "1080p")
                machin.Quality = "1080";
            else if (QualitycomboBox.Text == "720p")
                machin.Quality = "720";
            else if (QualitycomboBox.Text == "480p")
                machin.Quality = "480";
            else if (QualitycomboBox.Text == "360p")
                machin.Quality = "360";

            if (MkvcheckBox.IsChecked.Value == true)
                machin.MkvStatus = "1";
            else
                machin.MkvStatus = "0";

            machin.Format = comboBox_Copy.Text;
			machin.Url = urlBox.Text;
			machin.SavePath = save_TextBox.Text;
			machin.STState = "0";

			if (string.IsNullOrEmpty(machin.Url))
			{
				MessageBox.Show("Please, put a URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (!Uri.TryCreate(machin.Url, UriKind.Absolute, out Uri uri) || null == uri)
			{
				//Invalid URL
				MessageBox.Show("Please, put a valid URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (string.IsNullOrEmpty(machin.SavePath))
			{
				MessageBox.Show("Please, put a save path.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (string.IsNullOrEmpty(machin.Quality))
			{
				MessageBox.Show("Please, choose a quality.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (checkBox.IsChecked ?? false)
			{

				if (string.IsNullOrEmpty(machin.Langue))
				{
					MessageBox.Show("Please, choose a language.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
				if (string.IsNullOrEmpty(machin.Format))
				{
					MessageBox.Show("Please, choose a sub format.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
				machin.STState = "1";
			}
			if (File.Exists(@"C:\ProgramData\Crunchy-DL\cookies.txt"))
			{
				machin.Downloading();
			}
			else
				MessageBox.Show("Please login.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}

		private void aboutButton_Click(object sender, RoutedEventArgs e)
		{
			var aboutWindow = new Crunchyroll_Downloader.AboutWindow();
			aboutWindow.Show();
			Close();
		}

		private void CheckBoxChanged()
		{
			comboBox.IsEnabled = checkBox.IsChecked.Value;
			comboBox_Copy.IsEnabled = checkBox.IsChecked.Value;
            MkvcheckBox.IsEnabled = checkBox.IsChecked.Value;

        }
		private void checkBox_Checked(object sender, RoutedEventArgs e) => CheckBoxChanged();
		private void checkBox_Unchecked(object sender, RoutedEventArgs e) => CheckBoxChanged();

		private void button_login_Click(object sender, RoutedEventArgs e)
		{
			var loginWindow = new LoginWindow();
			loginWindow.Show();
			Close();
		}

		private void button_logout_Click(object sender, RoutedEventArgs e)
		{
			File.Delete(@"C:\ProgramData\Crunchy-DL\cookies.txt");
			button_login.IsEnabled = true;
			button_logout.IsEnabled = false;
		}
    }
}