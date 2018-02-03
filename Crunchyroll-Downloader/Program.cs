using System.Diagnostics;
using System.IO;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Threading;
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


        public void Downloading()
        {



            //MessageBox.Show("Everything is good, a pop up will tell you when the download is finished.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.EnableRaisingEvents = true;


            if (STState == "1")
            {
                process.StartInfo.Arguments = "--write-sub --sub-lang " + langue + " --sub-format " + format + " --no-part -o " + '"' + savePath + '"' + " --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt" + " " + url;
                Thread viewerThread = new Thread(delegate ()
                {
                    download download_window = new download();
                    download_window.Show();
                    download_window.Activate();
                    download_window.Closed += (s, e) =>
                    Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                    System.Windows.Threading.Dispatcher.Run();
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
                process.StartInfo.Arguments = "--no-part -o " + '"' + savePath + '"' + " --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt" + " " + url;

                Thread viewerThread = new Thread(delegate ()
                {
                    download download_window = new download();
                    download_window.Show();
                    download_window.Activate();
                    download_window.Closed += (s, e) =>
                    Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                    System.Windows.Threading.Dispatcher.Run();
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