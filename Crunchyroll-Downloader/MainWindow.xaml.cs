using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

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
			RemoveZips(actualFolder);
			using (var client = new WebClient())
			{
				var zip = new FastZip();

				if (Directory.Exists(@"C:\ProgramData\Crunchy-DL"))
				{
					if (File.Exists(@"C:\ProgramData\Crunchy-DL\ffmpeg.exe"))
					{
						if (File.Exists(@"C:\ProgramData\Crunchy-DL\youtube-dl.exe"))
						{
							if (File.Exists(@"C:\ProgramData\Crunchy-DL\login.exe"))
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
		private void RemoveZips(string actualFolder)
		{
			var ffmpeg = actualFolder + @"\ffmpeg.zip";
			var ffplay = actualFolder + @"\ffplay.zip";
			var ffprobe = actualFolder + @"\ffprobe.zip";
			if (File.Exists(ffmpeg))
				File.Delete(ffmpeg);
			if (File.Exists(ffplay))
				File.Delete(ffplay);
			if (File.Exists(ffprobe))
				File.Delete(ffprobe);
		}
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
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var zip = new FastZip();
				Directory.CreateDirectory(@"C:\ProgramData\Crunchy-DL");
				viewerThread.Start();
				client.DownloadFile("https://yt-dl.org/downloads/latest/youtube-dl.exe", @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
				client.DownloadFile("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffmpeg.zip", @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");
				client.DownloadFile("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffplay.zip", @"C:\ProgramData\Crunchy-DL\ffplay.zip");
				client.DownloadFile("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffprobe.zip", @"C:\ProgramData\Crunchy-DL\ffprobe.zip");
				client.DownloadFile("https://raw.githubusercontent.com/skid9000/CrunchyrollAuth/master/login.exe", @"C:\ProgramData\Crunchy-DL\login.exe");
				zip.ExtractZip(actualFolder + @"\ffmpeg.zip", actualFolder, "");
				zip.ExtractZip(actualFolder + @"\ffplay.zip", actualFolder, "");
				zip.ExtractZip(actualFolder + @"\ffprobe.zip", actualFolder, "");
				viewerThread.Abort();
				RemoveZips(actualFolder);
				MessageBox.Show("Dependencies are now installed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				InitializeComponent();
				CheckCookie();
			}
		}

		private void button_Save_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new FolderBrowserDialog();
			var result = dialog.ShowDialog();
			save_TextBox.Text = dialog.SelectedPath;
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			var instance = new Program();

			if (comboBox.Text == "Français (France)")
				instance.Langue = "frFR";
			else if (comboBox.Text == "English (US)")
				instance.Langue = "enUS";
			else if (comboBox.Text == "Español (ES)")
				instance.Langue = "esES";
			else if (comboBox.Text == "Español (LA)")
				instance.Langue = "esLA";
			else if (comboBox.Text == "Português (Brasil)")
				instance.Langue = "ptBR";
			else if (comboBox.Text == "العربية")
				instance.Langue = "arME";
			else if (comboBox.Text == "Italiano")
				instance.Langue = "itIT";
			else if (comboBox.Text == "Deutsch")
				instance.Langue = "deDE";
			else if (comboBox.Text == "Русский")
				instance.Langue = "ruRU";

			if (QualitycomboBox.Text == "Best (recommended)")
				instance.Quality = "best";
			else if (QualitycomboBox.Text == "1080p")
				instance.Quality = "1080";
			else if (QualitycomboBox.Text == "720p")
				instance.Quality = "720";
			else if (QualitycomboBox.Text == "480p")
				instance.Quality = "480";
			else if (QualitycomboBox.Text == "360p")
				instance.Quality = "360";

			if (MkvcheckBox.IsChecked.Value == true)
				instance.MkvStatus = "1";
			else
				instance.MkvStatus = "0";

			instance.Format = comboBox_Copy.Text;
			instance.Url = urlBox.Text;
			instance.SavePath = save_TextBox.Text;
			instance.STState = "0";

			if (string.IsNullOrEmpty(instance.Url))
			{
				MessageBox.Show("Please, put a URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (!Uri.TryCreate(instance.Url, UriKind.Absolute, out Uri uri) || null == uri)
			{
				//Invalid URL
				MessageBox.Show("Please, put a valid URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (string.IsNullOrEmpty(instance.SavePath))
			{
				MessageBox.Show("Please, put a save path.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (string.IsNullOrEmpty(instance.Quality))
			{
				MessageBox.Show("Please, choose a quality.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}

			if (checkBox.IsChecked ?? false)
			{

				if (string.IsNullOrEmpty(instance.Langue))
				{
					MessageBox.Show("Please, choose a language.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
				if (string.IsNullOrEmpty(instance.Format))
				{
					MessageBox.Show("Please, choose a sub format.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
				instance.STState = "1";
			}
			if (File.Exists(@"C:\ProgramData\Crunchy-DL\cookies.txt"))
			{
				instance.Downloading();
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