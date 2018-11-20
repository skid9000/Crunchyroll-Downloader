using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;

namespace CrunchyrollDownloader {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  
  public partial class MainWindow : Window {
    private string crunchyData = @"C:\ProgramData\Crunchy-DL\";
    public MainWindow() {
      using (var client = new WebClient()) {
        var zip = new FastZip();
        if (Directory.Exists(crunchyData)) {
          if (
            File.Exists(crunchyData + "ffmpeg.exe")
            && File.Exists(crunchyData + "youtube-dl.exe")
            && File.Exists(crunchyData + "login.exe")
          ) {
            InitializeComponent();
            CheckCookie();
          } else {
            MessageBox.Show(
              "Dependencies corrupted or missing, click OK to re-download them.",
              "Missing Dependencies",
              MessageBoxButton.OK,
              MessageBoxImage.Information
            );
            Directory.Delete(crunchyData, true);
            InstallAll();
          }
        } else {
          InstallAll();
        }
      }
    }

    private void CheckCookie() {
      if (File.Exists(crunchyData + "cookies.txt")) {
        button_login.IsEnabled = false;
        button_logout.IsEnabled = true;
      } else {
        button_login.IsEnabled = true;
        button_logout.IsEnabled = false;
      }
    }

    private void UpdateYTDL() {
      var process = new Process();
      // Configure the process using the StartInfo properties.
      process.StartInfo.FileName = crunchyData + "youtube-dl.exe";
      process.StartInfo.Arguments = "-U";
      process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
      process.Start();
      process.WaitForExit(); // Waits here for the process to exit.
    }

    private void removeZips() {
      var ffmpegZip = crunchyData + "ffmpeg.zip";
      var ffplayZip = crunchyData + "ffplay.zip";
      var ffprobeZip = crunchyData + "ffprobe.zip";
      if (File.Exists(ffmpegZip)) {
        File.Delete(ffmpegZip);
      }
      if (File.Exists(ffplayZip)) {
        File.Delete(ffplayZip);
      }
      if (File.Exists(ffprobeZip)) {
        File.Delete(ffprobeZip);
      }
    }
    
    private void InstallAll() {
      var viewerThread = new Thread(() => {
        var download_window = new DownloadWindow();
        download_window.Show();
        download_window.Activate();
        download_window.Closed += (s, e) =>
        Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
        Dispatcher.Run();
      });
      viewerThread.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception

      using (var client = new WebClient()) {
        var zip = new FastZip();
        //MessageBox.Show("Dependencies not detected, downloading ...", "Important Note", MessageBoxButton.OK, MessageBoxImage.Information);
        Directory.CreateDirectory(@"C:\ProgramData\Crunchy-DL");
        //dl_label="[1/3] Downloading dependencies : Youtube-DL ...";
        viewerThread.Start();
        client.DownloadFile(
          "https://yt-dl.org/downloads/latest/youtube-dl.exe",
          crunchyData + "youtube-dl.exe"
        );
        //viewerThread.Abort();
        //dl_label = "[2/3] Downloading dependencies : FFmpeg ...";
        //viewerThread.Start();
        var ghraws = "https://raw.githubusercontent.com/skid9000/Crunchyroll-Downloader/develop/FFmpeg/";
        client.DownloadFile(ghraws + "ffmpeg.zip", crunchyData + "ffmpeg.zip");
        client.DownloadFile(ghraws + "ffplay.zip", crunchyData + "ffplay.zip");
        client.DownloadFile(ghraws + "ffprobe.zip", crunchyData + "ffprobe.zip");
        //viewerThread.Abort();
        //dl_label = "[3/3] Downloading dependencies : Crunchyroll-Auth ...";
        //viewerThread.Start();
        client.DownloadFile(
          "https://github.com/skid9000/CrunchyrollAuth/releases/download/1.0/login.exe",
          crunchyData + "login.exe"
        );
        //viewerThread.Abort();

        //dl_label = "Extracting ...";
        //viewerThread.Start();
        zip.ExtractZip(crunchyData + "ffmpeg.zip", crunchyData, "");
        zip.ExtractZip(crunchyData + "ffplay.zip", crunchyData, "");
        zip.ExtractZip(crunchyData + "ffprobe.zip", crunchyData, "");
        viewerThread.Abort();
        MessageBox.Show(
          "youtube-dl and ffmpeg are now installed.",
          "Success",
          MessageBoxButton.OK,
          MessageBoxImage.Information
        );
        removeZips();
        InitializeComponent();
        CheckCookie();
      }
    }

    private void button_Save_Click(object sender, RoutedEventArgs e) {
      // Create OpenFileDialog
      var dialog = new FolderBrowserDialog();
      var result = dialog.ShowDialog();
      save_TextBox.Text = dialog.SelectedPath;
    }

    private void show_error(string s) {
      MessageBox.Show(s, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }
    private void button_Click(object sender, RoutedEventArgs e) {
      var machin = new Program();

      if (comboBox.Text == "Français (France)") {
        machin.Langue = "frFR";
      } else if (comboBox.Text == "English (US)") {
        machin.Langue = "enUS";
      } else if (comboBox.Text == "Español") {
        machin.Langue = "esES";
      } else if (comboBox.Text == "Español (España)") {
        machin.Langue = "esLA";
      } else if (comboBox.Text == "Português (Brasil)") {
        machin.Langue = "ptBR";
      } else if (comboBox.Text == "العربية") {
        machin.Langue = "arME";
      } else if (comboBox.Text == "Italiano") {
        machin.Langue = "itIT";
      } else if (comboBox.Text == "Deutsch") {
        machin.Langue = "deDE";
      } else if (comboBox.Text == "Русский") {
        machin.Langue = "ruRU";
      }

      if (QualitycomboBox.Text == "Best (recommended)") {
        machin.Quality = "best";
      } else {
        machin.Quality = QualitycomboBox.Text.TrimEnd('p');
      }

      if (MkvcheckBox.IsChecked.Value == true) {
        machin.MkvStatus = "1";
      } else {
        machin.MkvStatus = "0";
      }

      machin.Format = comboBox_Copy.Text;
      machin.Url = urlBox.Text;
      machin.SavePath = save_TextBox.Text;
      machin.STState = "0";

      if (string.IsNullOrEmpty(machin.Url)) {
        show_error("Cannot download without a URL.");
        return;
      }

      if (!Uri.TryCreate(machin.Url, UriKind.Absolute, out Uri uri) || null == uri) {
        //Invalid URL
        show_error("The URL is invalid");
        return;
      }

      if (string.IsNullOrEmpty(machin.SavePath)) {
        show_error("Please choose a save path.");
        return;
      }

      if (string.IsNullOrEmpty(machin.Quality)) {
        show_error("Please choose a quality.");
        return;
      }

      if (checkBox.IsChecked ?? false) {
        if (string.IsNullOrEmpty(machin.Langue)) {
          show_error("Please choose a language.");
          return;
        }
        if (string.IsNullOrEmpty(machin.Format)) {
          show_error("Please choose a sub format.");
          return;
        }
        machin.STState = "1";
      }
      if (File.Exists(crunchyData + "cookies.txt")) {
        machin.Downloading();
      } else {
        show_error("Please login.");
      }
    }

    private void aboutButton_Click(object sender, RoutedEventArgs e) {
      var aboutWindow = new Crunchyroll_Downloader.AboutWindow();
      aboutWindow.Show();
      Close();
    }

    private void CheckBoxChanged() {
      comboBox.IsEnabled = checkBox.IsChecked.Value;
      comboBox_Copy.IsEnabled = checkBox.IsChecked.Value;
      MkvcheckBox.IsEnabled = checkBox.IsChecked.Value;

    }
    private void checkBox_Checked(object sender, RoutedEventArgs e) => CheckBoxChanged();
    private void checkBox_Unchecked(object sender, RoutedEventArgs e) => CheckBoxChanged();

    private void button_login_Click(object sender, RoutedEventArgs e) {
      var loginWindow = new LoginWindow();
      loginWindow.Show();
      Close();
    }

    private void button_logout_Click(object sender, RoutedEventArgs e) {
      File.Delete(crunchyData + "cookies.txt");
      button_login.IsEnabled = true;
      button_logout.IsEnabled = false;
    }
  }
}
