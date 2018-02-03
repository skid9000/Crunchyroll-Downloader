using System.Diagnostics;
using System.Windows;

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

            if (STState == "1")
            {
                process.StartInfo.Arguments = "--write-sub --sub-lang " + Langue + " --sub-format " + Format + " --no-part -o " + '"' + SavePath + '"' + " --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt" + " " + Url;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.Start();
                process.WaitForExit();// Waits here for the process to exit.
                MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                process.StartInfo.Arguments = "--no-part -o " + '"' + SavePath + '"' + " --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt"+ " " + Url;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.Start();
                process.WaitForExit();// Waits here for the process to exit.
                MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}