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

namespace WpfApplication1
{
    public class Program
    {
        public void FirstStep()
        {

            string ActualFolder = @"%USERPROFILE%\AppData\Roaming\Crunchy-DL";
            WebClient Client = new WebClient();
            var x = new ICSharpCode.SharpZipLib.Zip.FastZip();

            if (Directory.Exists(@"%USERPROFILE%\AppData\Roaming\Crunchy-DL"))
            {
                Downloading();
            }
            else
            {
                MessageBox.Show("Youtube-DL & Ffmpeg not detected, downloading ...", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
                Directory.CreateDirectory(@"%USERPROFILE%\AppData\Roaming\Crunchy-DL");
                Client.DownloadFile("https://github.com/rg3/youtube-dl/releases/download/2017.10.12/youtube-dl.exe", @"%USERPROFILE%\AppData\Roaming\Crunchy-DL\youtube-dl.exe");
                Client.DownloadFile("http://download.tucr.tk/ffmpeg.zip", @"%USERPROFILE%\AppData\Roaming\Crunchy-DL\ffmpeg.zip");

                x.ExtractZip(ActualFolder + @"\ffmpeg.zip", ActualFolder, "");



            }
        }

        public string url { get; set; }
        public string cookie { get; set; }
        public string langue { get; set; }
        public string format { get; set; }
        public string savePath { get; set; }

        private void Downloading()
        {
            MessageBox.Show("Everything is good, a pop up will tell you when the downloadis finished.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"%USERPROFILE%\AppData\Roaming\Crunchy-DL\youtube-dl.exe";
            process.StartInfo.Arguments = "--write-sub --sub-lang " + langue + " --sub-format " + format + " --no-part -o " + '"' + savePath + '"' + " --cookies " + '"' + cookie + '"' + " " + url;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.
            MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);


        }


    }
}
