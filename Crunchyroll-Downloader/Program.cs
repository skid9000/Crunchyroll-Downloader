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


        public string STState { get; set; }
        public string url { get; set; }
        public string langue { get; set; }
        public string format { get; set; }
        public string savePath { get; set; }

        public void Downloading()
        {
            //MessageBox.Show("Everything is good, a pop up will tell you when the download is finished.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe";

            if (STState == "1")
            {

                process.StartInfo.Arguments = "--write-sub --sub-lang " + langue + " --sub-format " + format + " --no-part -o " + '"' + savePath + '"' + " --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt" + " " + url;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.Start();
                process.WaitForExit();// Waits here for the process to exit.
                MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                process.StartInfo.Arguments = "--no-part -o " + '"' + savePath + '"' + " --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt"+ " " + url;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.Start();
                process.WaitForExit();// Waits here for the process to exit.
                MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);

            }


        }


    }
}
