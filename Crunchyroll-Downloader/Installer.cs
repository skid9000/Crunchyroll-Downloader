using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CrunchyrollDownloader.Progress;
using CrunchyrollDownloader.ViewModels;
using ICSharpCode.SharpZipLib.Zip;

namespace CrunchyrollDownloader
{
    public class Installer
    {
        public const string InstallFolder = @"C:\ProgramData\Crunchy-DL";
        public async Task InstallAll()
        {
            var data = new DownloadingViewModel();
            data.Progress = new TaskManager(new []
            {
                new ProgressTask("Downloading dependencies: Youtube-DL"),
                new ProgressTask("Downloading dependencies: FFmpeg-base"),
                new ProgressTask("Downloading dependencies: FFmpeg-play"),
                new ProgressTask("Downloading dependencies: FFmpeg-probe"),
                new ProgressTask("Downloading dependencies: Crunchyroll-Auth"), 
                new ProgressTask("Extracting")
            });
            var viewerThread = new Thread(() =>
            {
                var downloadWindow = new DownloadWindow(data);
                downloadWindow.Show();
                downloadWindow.Activate();
                downloadWindow.Closed += (s, e) =>
                Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                Dispatcher.Run();
            });
            viewerThread.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception oh no
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += (sender, args) =>
                {
                    var progress = args.BytesReceived / (double) args.TotalBytesToReceive;
                    if (progress >= 0.99) return;
                    data.Progress.CurrentTask.Progress = progress;
                };
                var zip = new FastZip();
                Directory.CreateDirectory(@"C:\ProgramData\Crunchy-DL");
                viewerThread.Start();
                await client.DownloadFileTaskAsync(new Uri("https://yt-dl.org/downloads/latest/youtube-dl.exe"), @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
                data.Progress.GoNext();

                await client.DownloadFileTaskAsync(new Uri("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffmpeg.zip"), @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");
                data.Progress.GoNext();

                await client.DownloadFileTaskAsync(new Uri("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffplay.zip"), @"C:\ProgramData\Crunchy-DL\ffplay.zip");
                data.Progress.GoNext();

                await client.DownloadFileTaskAsync(new Uri("https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/ffprobe.zip"), @"C:\ProgramData\Crunchy-DL\ffprobe.zip");
                data.Progress.GoNext();

                await client.DownloadFileTaskAsync(new Uri("https://github.com/skid9000/CrunchyrollAuth/releases/download/1.0/login.exe"), @"C:\ProgramData\Crunchy-DL\login.exe");
                data.Progress.GoNext();

                zip.ExtractZip(InstallFolder + @"\ffmpeg.zip", InstallFolder, "");
                zip.ExtractZip(InstallFolder + @"\ffplay.zip", InstallFolder, "");
                zip.ExtractZip(InstallFolder + @"\ffprobe.zip", InstallFolder, "");
                viewerThread.Abort();
                MessageBox.Show("youtube-dl and FFmpeg are now installed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ShowErrorMessage() => MessageBox.Show("Dependencies seems corrupted or missing, click OK to re-download them.", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
        public bool CheckIfInstalled()
        {
            if (Directory.Exists(@"C:\ProgramData\Crunchy-DL") &&
                File.Exists(@"C:\ProgramData\Crunchy-DL\ffmpeg.exe") &&
                File.Exists(@"C:\ProgramData\Crunchy-DL\youtube-dl.exe") &&
                File.Exists(@"C:\ProgramData\Crunchy-DL\login.exe"))
            {
                return true;
            }
            ShowErrorMessage();
            if (Directory.Exists(InstallFolder))
                Directory.Delete(InstallFolder, true);
            return false;
        }
    }
}