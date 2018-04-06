using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System;
using System.ComponentModel;

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
                                if (File.Exists(@"C:\ProgramData\Crunchy-DL\login\v1.2.3.txt"))
                                {
                                    InitializeComponent();
                                    CheckCookie();
                                }
                                else
                                {
                                    File.Delete(@"C:\ProgramData\Crunchy-DL\login.zip");
                                    Directory.Delete(@"C:\ProgramData\Crunchy-DL\login", true);
                                    client.DownloadFile("http://download.tucr.tk/login.zip", @"C:\ProgramData\Crunchy-DL\login.zip");
                                    zip.ExtractZip(actualFolder + @"\login.zip", actualFolder, "");
                                    InitializeComponent();
                                    CheckCookie();
                                }
                              
                            }
                            else
                            {
                                if (File.Exists(@"C:\ProgramData\Crunchy-DL\login.zip"))
                                {
                                    zip.ExtractZip(actualFolder + @"\login.zip", actualFolder, "");
                                    InitializeComponent();
                                    CheckCookie();
                                }
                                else
                                {
                                    client.DownloadFile("http://download.tucr.tk/login.zip", @"C:\ProgramData\Crunchy-DL\login.zip");
                                    zip.ExtractZip(actualFolder + @"\login.zip", actualFolder, "");
                                    InitializeComponent();
                                    CheckCookie();
                                }
                            }
                        }
                        else
                        {
                            client.DownloadFile("https://yt-dl.org/downloads/latest/youtube-dl.exe", @"C:\ProgramData\Crunchy-DL\youtube-dl.exe");
                            UpdateYTDL();
                            InitializeComponent();
                            CheckCookie();
                        }
                    }
                    else
                    {
                        if (File.Exists(@"C:\ProgramData\Crunchy-DL\ffmpeg.zip"))
                        {
                            zip.ExtractZip(actualFolder + @"\ffmpeg.zip", actualFolder, "");
                            InitializeComponent();
                            CheckCookie();
                        }
                        else
                        {
                            client.DownloadFile("http://download.tucr.tk/ffmpeg.zip", @"C:\ProgramData\Crunchy-DL\ffmpeg.zip");
                            zip.ExtractZip(actualFolder + @"\ffmpeg.zip", actualFolder, "");
                            InitializeComponent();
                            CheckCookie();
                        }
                    }
                }
                else
                    InstallAll();
            }
        }

        private void CheckCookie()
        {
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

        private void InstallAll()
        {
            var dl_status = new dl_status();
            dl_status.progress = "1";
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
            var actualFolder = @"C:\ProgramData\Crunchy-DL";
                var zip = new FastZip();
                MessageBox.Show("Youtube-DL & FFmpeg not detected, downloading ...", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
                Directory.CreateDirectory(@"C:\ProgramData\Crunchy-DL");
                dl_status.label_dl = "Downloading Youtube-DL";
                string dl_url = "https://yt-dl.org/downloads/latest/youtube-dl.exe";
                string dl_path = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe";
                startDownload(dl_url, dl_path);
                dl_status.label_dl = "Downloading FFmpeg";
                dl_url = "http://download.tucr.tk/ffmpeg.zip";
                dl_path = @"C:\ProgramData\Crunchy-DL\ffmpeg.zip";
                startDownload(dl_url, dl_path);
                dl_status.label_dl = "Downloading CrunchyrollAuth";
                dl_url = "http://download.tucr.tk/login.zip";
                dl_path = @"C:\ProgramData\Crunchy-DL\login.zip";
                startDownload(dl_url, dl_path);

                dl_status.label_dl = "Extracting ...";
                zip.ExtractZip(actualFolder + @"\ffmpeg.zip", actualFolder, "");
                zip.ExtractZip(actualFolder + @"\login.zip", actualFolder, "");
                UpdateYTDL();
                MessageBox.Show("youtube-dl and FFmpeg are now installed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                viewerThread.Abort();
                InitializeComponent();
                CheckCookie();
            
        }

        private void startDownload(string dl_url, string dl_path)
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri(dl_url), dl_path);
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var dl_status = new dl_status();
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            int lol = int.Parse(Math.Truncate(percentage).ToString());
            string lol2 = lol.ToString();
            dl_status.progress = lol2;
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            return;
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
                save_TextBox.Text = dialog.FileName; // Open document
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var machin = new Program();

            if (comboBox.Text == "Français (France)")
                machin.Langue = "frFR";
            else if (comboBox.Text == "English (US)")
                machin.Langue = "enUS";
            else if (comboBox.Text == "Español")
                machin.Langue = "esES";
            else if (comboBox.Text == "Español (España)")
                machin.Langue = "esLA";
            else if (comboBox.Text == "Português (Brasil)")
                machin.Langue = "ptBR";
            else if (comboBox.Text == "العربية")
                machin.Langue = "arME";
            else if (comboBox.Text == "Italiano")
                machin.Langue = "itIT";
            else if (comboBox.Text == "Deutsch")
                machin.Langue = "deDE";
            else if (comboBox.Text == "Русский")
                machin.Langue = "ruRU";

            machin.Format = comboBox_Copy.Text;
            machin.Url = urlBox.Text;
            machin.SavePath = save_TextBox.Text;
            machin.STState = "0";
            machin.Quality = QualitycomboBox.Text;

            if (string.IsNullOrEmpty(machin.Url))
            {
                MessageBox.Show("Please, put a URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!Uri.TryCreate(machin.Url, UriKind.Absolute, out Uri uri) || null == uri)
            {
                //Invalid URL
                MessageBox.Show("Please, put a valid URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(machin.SavePath))
            {
                MessageBox.Show("Please, put a save path.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(machin.Quality))
            {
                MessageBox.Show("Please, choose a quality.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
                machin.Downloading();
            }
            else
                MessageBox.Show("Please login.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new Crunchyroll_Downloader.AboutWindow();
            aboutWindow.Show();
            Close();
        }

        private void CheckBoxChanged()
        {
            comboBox.IsEnabled = checkBox.IsChecked.Value;
            comboBox_Copy.IsEnabled = checkBox.IsChecked.Value;
        }
        private void checkBox_Checked(object sender, RoutedEventArgs e) => CheckBoxChanged();
        private void checkBox_Unchecked(object sender, RoutedEventArgs e) => CheckBoxChanged();

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
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