using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using CrunchyrollDownloader.Progress;
using CrunchyrollDownloader.ViewModels;

namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly MainWindowViewModel _otherVm;
        public LoginWindow(MainWindowViewModel otherVm = null)
        {
            InitializeComponent();
            _otherVm = otherVm;
        }

        private async void button_login_Click(object sender, RoutedEventArgs e)
        {
            var username = textBox_username.Text;
            var password = textBox_password.Password;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Please, put your username.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please, put your password.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var process = new Process
            {
                StartInfo =
                {
                    FileName = @"C:\ProgramData\Crunchy-DL\login.exe",
                    Arguments = $@"""{username}"" ""{password}""",
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            // Configure the process using the StartInfo properties.
            process.Start();
            var data = new DownloadingViewModel
            {
                IsIndeterminate = true,
                Progress = new TaskManager(new[] {new ProgressTask("Logging in") {Display = "{3}..."}})
            };
            var window = new DownloadWindow(data) { Owner = this };
            window.Show();
            await Task.Run(() => process.WaitForExit());// Waits here for the process to exit.
            window.Close();
            if (!File.Exists(@"C:\ProgramData\Crunchy-DL\cookies.txt"))
            {
                MessageBox.Show("Something went wrong when logging into your account. Please check your credentials.",
                    "Oops", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _otherVm?.UpdateCookies();
            Close();
        }
    }
}