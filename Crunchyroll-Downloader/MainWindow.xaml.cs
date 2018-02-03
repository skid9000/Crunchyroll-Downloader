using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

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
            using (var client = new WebClient())
            {
                var zip = new FastZip();

                if (Directory.Exists(@"C:\ProgramData\Crunchy-DL"))
                {
                    if (File.Exists(@"C:\ProgramData\Crunchy-DL\ffmpeg.exe"))
                    {
                        if (File.Exists(@"C:\ProgramData\Crunchy-DL\youtube-dl.exe"))
                        {
                            if (Directory.Exists(@"C:\ProgramData\Crunchy-DL\login"))
                            {
                                InitializeComponent();
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
                            else
                            {
                                if (File.Exists(@"C:\ProgramData\Crunchy-DL\login.zip"))
                                    zip.ExtractZip(actualFolder + @"\login.zip", actualFolder, "");
                                else
                                {
                                    client.DownloadFile("http://download.tucr.tk/login.zip", @"C:\ProgramData\Crunchy-DL\login.zip");
                                    zip.ExtractZip(actualFolder + @"\login.zip", actualFolder, "");
                                }
                            }
                        }
                        else
                        {
                            client.DownloadFile("https://github.com/rg3/youtube-dl/releases/download/2018.01.27/youtube-dl.exe", @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
                            UpdateYTDL();
                        }
                    }
                    else
                    {
                        if (File.Exists(@"C:\ProgramData\Crunchy-DL\ffmpeg.zip"))
                            zip.ExtractZip(actualFolder + @"\ffmpeg.zip", actualFolder, "");
                        else
                        {
                            client.DownloadFile("http://download.tucr.tk/ffmpeg.zip", @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");
                            zip.ExtractZip(actualFolder + @"\ffmpeg.zip", actualFolder, "");
                        }
                    }

                }
                else
                    InstallAll();
            }
        }

        private void UpdateYTDL()
        {
            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe";
            process.StartInfo.Arguments = "-U";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit(); // Waits here for the process to exit.
        }

        private void InstallAll()
        {
            var viewerThread = new Thread(() =>
            {
                var download_window = new download();
                download_window.Show();
                download_window.Activate();
                download_window.Closed += (s, e) =>
                Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                Dispatcher.Run();
            });
            viewerThread.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
            viewerThread.Start();
            string ActualFolder = @"C:\ProgramData\Crunchy-DL";
            using (var client = new WebClient())
            {
                var zip = new FastZip();
                MessageBox.Show("Youtube-DL & FFmpeg not detected, downloading ...", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
                Directory.CreateDirectory(@"C:\ProgramData\Crunchy-DL");
                client.DownloadFile("https://github.com/rg3/youtube-dl/releases/download/2018.01.27/youtube-dl.exe", @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
                client.DownloadFile("http://download.tucr.tk/ffmpeg.zip", @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");
                client.DownloadFile("http://download.tucr.tk/login.zip", @"C:\ProgramData\Crunchy-DL\login.zip");

                zip.ExtractZip(ActualFolder + @"\ffmpeg.zip", ActualFolder, "");
                zip.ExtractZip(ActualFolder + @"\login.zip", ActualFolder, "");
                UpdateYTDL();
                MessageBox.Show("youtube-dl and FFmpeg are now installed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                viewerThread.Abort();
                InitializeComponent();

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
        }

        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            var dialog = new SaveFileDialog
            {
                // Set filter for file extension and default file extension
                DefaultExt = ".mp4",
                Filter = "Such mp4, such wow | *.mp4"
            };

            // Display OpenFileDialog by calling ShowDialog method
            var result = dialog.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result ?? false)
            {
                // Open document
                string filename = dialog.FileName;
                save_TextBox.Text = filename;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var machin = new Program();

            if (comboBox.Text == "Français (France)")
            {
                machin.Langue = "frFR";
            }
            else if (comboBox.Text == "English (US)")
            {
                machin.Langue = "enUS";
            }
            else if (comboBox.Text == "Español")
            {
                machin.Langue = "esES";
            }
            else if (comboBox.Text == "Español (España)")
            {
                machin.Langue = "esLA";
            }
            else if (comboBox.Text == "Português (Brasil)")
            {
                machin.Langue = "ptBR";
            }
            else if (comboBox.Text == "العربية")
            {
                machin.Langue = "arME";
            }
            else if (comboBox.Text == "Italiano")
            {
                machin.Langue = "itIT";
            }
            else if (comboBox.Text == "Deutsch")
            {
                machin.Langue = "deDE";
            }

            machin.Format = comboBox_Copy.Text;
            machin.Url = urlBox.Text;
            machin.SavePath = save_TextBox.Text;
            machin.STState = "0";

            if (string.IsNullOrEmpty(machin.Url))
            {
                MessageBox.Show("Please, put a URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(machin.SavePath))
            {
                MessageBox.Show("Please, put a save path.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (checkBox.IsChecked ?? false)
            {

                if (string.IsNullOrEmpty(machin.Langue))
                {
                    MessageBox.Show("Please, choose a language.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (string.IsNullOrEmpty(machin.Format))
                {
                    MessageBox.Show("Please, choose a sub format.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                machin.STState = "1";
            }
            if (File.Exists(@"C:\ProgramData\Crunchy-DL\cookies.txt"))
            {
                UpdateYTDL();
                machin.Downloading();
            }
            else
                MessageBox.Show("Please login.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e) =>
            MessageBox.Show("All informations on github. https://github.com/skid9000/Crunchyroll-Downloader", "About", MessageBoxButton.OK);

        private void CheckBoxChanged()
        {
            comboBox.IsEnabled = checkBox.IsChecked.Value;
            comboBox_Copy.IsEnabled = checkBox.IsChecked.Value;
        }
        private void checkBox_Checked(object sender, RoutedEventArgs e) => CheckBoxChanged();
        private void checkBox_Unchecked(object sender, RoutedEventArgs e) => CheckBoxChanged();

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new login();
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