using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace CrunchyrollDownloader
{
    public class Program
    {
        public void FirstStep()
        {

            string ActualFolder = @"C:\ProgramData\Crunchy-DL";
            WebClient Client = new WebClient();
            var x = new ICSharpCode.SharpZipLib.Zip.FastZip();

            if (Directory.Exists(@"C:\ProgramData\Crunchy-DL"))
            {
                Downloading();
            }
            else
            {
                MessageBox.Show("Youtube-DL & FFmpeg not detected, downloading ...", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
                Directory.CreateDirectory(@"C:\ProgramData\Crunchy-DL");
                Client.DownloadFile("https://github.com/rg3/youtube-dl/releases/download/2017.10.12/youtube-dl.exe", @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
                Client.DownloadFile("http://download.tucr.tk/ffmpeg.zip", @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");

                x.ExtractZip(ActualFolder + @"\ffmpeg.zip", ActualFolder, "");
                MessageBox.Show("youtube-dl and FFmpeg are now installed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Downloading();



            }
        }

        public string url { get; set; }
        public string cookie { get; set; }
        public string langue { get; set; }
        public string format { get; set; }
        public string savePath { get; set; }

        private void Downloading()
        {
            MessageBox.Show("Everything is good, a pop up will tell you when the download is finished.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe";
            process.StartInfo.Arguments = "--write-sub --sub-lang " + langue + " --sub-format " + format + " --no-part -o " + '"' + savePath + '"' + " --cookies " + '"' + cookie + '"' + " " + url;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.
            MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);


        }


    }
}
