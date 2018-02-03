using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            string username = textBox_username.Text;
            string password = textBox_password.Password;

            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\ProgramData\Crunchy-DL\login\login.exe";

                process.StartInfo.Arguments = username + " " + password;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                process.Start();
                process.WaitForExit();// Waits here for the process to exit.

            MainWindow main_window = new MainWindow();
            main_window.Show();

            this.Close();
            }
    }
}
