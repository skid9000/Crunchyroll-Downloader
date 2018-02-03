using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using ICSharpCode.SharpZipLib.Zip;

namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            string ActualFolder = @"C:\ProgramData\Crunchy-DL";
            WebClient Client = new WebClient();
            var x = new ICSharpCode.SharpZipLib.Zip.FastZip();

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
                            {
                                x.ExtractZip(ActualFolder + @"\login.zip", ActualFolder, "");
                            }
                            else
                            {
                                Client.DownloadFile("http://download.tucr.tk/login.zip", @"C:\ProgramData\Crunchy-DL\login.zip");
                                x.ExtractZip(ActualFolder + @"\login.zip", ActualFolder, "");
                            }
                        }
                    }
                    else
                    {
                        Client.DownloadFile("https://github.com/rg3/youtube-dl/releases/download/2018.01.27/youtube-dl.exe", @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
                        YTDL_update();
                    }
                }
                else
                {
                    if (File.Exists(@"C:\ProgramData\Crunchy-DL\ffmpeg.zip"))
                    {
                        x.ExtractZip(ActualFolder + @"\ffmpeg.zip", ActualFolder, "");
                    }
                    else
                    {
                        Client.DownloadFile("http://download.tucr.tk/ffmpeg.zip", @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");
                        x.ExtractZip(ActualFolder + @"\ffmpeg.zip", ActualFolder, "");
                    }
                }

            }
            else
                Install_All();
        }

        public void YTDL_update()
        {
            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe";
            process.StartInfo.Arguments = "-U";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit(); // Waits here for the process to exit.
        }

        public void Install_All()
        {
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
            string ActualFolder = @"C:\ProgramData\Crunchy-DL";
            WebClient Client = new WebClient();
            var x = new ICSharpCode.SharpZipLib.Zip.FastZip();
            MessageBox.Show("Youtube-DL & FFmpeg not detected, downloading ...", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
            Directory.CreateDirectory(@"C:\ProgramData\Crunchy-DL");
            Client.DownloadFile("https://github.com/rg3/youtube-dl/releases/download/2018.01.27/youtube-dl.exe", @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
            Client.DownloadFile("http://download.tucr.tk/ffmpeg.zip", @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");
            Client.DownloadFile("http://download.tucr.tk/login.zip", @"C:\ProgramData\Crunchy-DL\login.zip");

            x.ExtractZip(ActualFolder + @"\ffmpeg.zip", ActualFolder, "");
            x.ExtractZip(ActualFolder + @"\login.zip", ActualFolder, "");
            YTDL_update();
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

        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            var dlg = new SaveFileDialog
            {
                // Set filter for file extension and default file extension
                DefaultExt = ".mp4",
                Filter = "Such mp4, such wow | *.mp4"
            };

            // Display OpenFileDialog by calling ShowDialog method
            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result ?? false)
            {
                // Open document
                string filename = dlg.FileName;
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

            if (String.IsNullOrEmpty(machin.Url))
            {
                MessageBox.Show("Please, put a URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (String.IsNullOrEmpty(machin.SavePath))
            {
                MessageBox.Show("Please, put a save path.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (checkBox.IsChecked ?? false)
            {

                if (String.IsNullOrEmpty(machin.Langue))
                {
                    MessageBox.Show("Please, choose a language.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (String.IsNullOrEmpty(machin.Format))
                {
                    MessageBox.Show("Please, choose a sub format.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                machin.STState = "1";
            }
            if (File.Exists(@"C:\ProgramData\Crunchy-DL\cookies.txt"))
            {
                YTDL_update();
                Machin.Downloading();
            }
            else
            {
                MessageBox.Show("Please login.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("All informations on github. https://github.com/skid9000/Crunchyroll-Downloader", "About", MessageBoxButton.OK);
        }

        void CheckBoxChanged()
        {
            comboBox.IsEnabled = checkBox.IsChecked.Value;
            comboBox_Copy.IsEnabled = checkBox.IsChecked.Value;
        }
        private void checkBox_Checked(object sender, RoutedEventArgs e) => CheckBoxChanged();
        private void checkBox_Unchecked(object sender, RoutedEventArgs e) => CheckBoxChanged();

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            login login_window = new login();
            login_window.Show();
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