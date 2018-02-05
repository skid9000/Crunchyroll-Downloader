using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace CrunchyrollDownloader
{
    public class Program
    {
        public string STState { get; set; }
        public string Url { get; set; }
        public string Langue { get; set; }
        public string Format { get; set; }
        public string SavePath { get; set; }

        /// <summary>
        /// Downloadings a file.
        /// </summary>
        public void Downloading()
        {
            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            if (STState == "1")
            {
                process.StartInfo.Arguments = $"--write-sub --sub-lang {Langue} --sub-format {Format} --no-part -o \"{SavePath}\" --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt {Url}";
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
                viewerThread.Start();

                process.Start();
                process.WaitForExit(); // Waits here for the process to exit.
                viewerThread.Abort();
                MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                process.StartInfo.Arguments = $"--no-part -o \"{SavePath}\" --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt {Url}";

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
                viewerThread.Start();

                process.Start();
                process.WaitForExit();// Waits here for the process to exit.
                viewerThread.Abort();
                MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}