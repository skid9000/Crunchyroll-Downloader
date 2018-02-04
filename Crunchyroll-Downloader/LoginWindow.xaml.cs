using System.Diagnostics;
using System.Windows;

namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
        }

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            var username = textBox_username.Text;
            var password = textBox_password.Password;

            var process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\login\login.exe";
            process.StartInfo.Arguments = $"{username} {password}";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.

            var mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }
        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }
    }
}