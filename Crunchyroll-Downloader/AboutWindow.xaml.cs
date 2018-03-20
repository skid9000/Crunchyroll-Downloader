using System.Diagnostics;
using System.Windows;

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

    }
}
