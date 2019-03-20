using CrunchyrollDownloader;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Crunchyroll_Downloader
{
	/// <summary>
	/// Logique d'interaction pour AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();
			Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			UpdateYTDL();
		}
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ReDownload();
        }


        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			var mainWindow = new CrunchyrollDownloader.MainWindow();
			mainWindow.Show();
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

        private void ReDownload()
        {
            var viewerThread = new Thread(() =>
            {
                var download_window = new DownloadWindow();
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
                Directory.Delete(@"C:\ProgramData\Crunchy-DL", true);
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
            }
        }

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

    }
}
