using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Cookie_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Crunchyroll Cookie Text File (*.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                cookie_TextBox.Text = filename;
            }


        }


        private void button_Save_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".mp4";
            dlg.Filter = "Such mp4, such wow | *.mp4";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                save_TextBox.Text = filename;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Program Machin = new Program();


            Machin.langue = comboBox.Text;
            Machin.format = comboBox_Copy.Text;
            Machin.cookie = cookie_TextBox.Text;
            Machin.url = urlBox.Text;
            Machin.savePath = save_TextBox.Text;

            if (String.IsNullOrEmpty(Machin.url))
            {
                MessageBox.Show("Please, put a URL.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (String.IsNullOrEmpty(Machin.savePath))
            {
                MessageBox.Show("Please, put a save path.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (String.IsNullOrEmpty(Machin.cookie))
            {
                MessageBox.Show("Please, put a cookie file.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (String.IsNullOrEmpty(Machin.langue))
            {
                MessageBox.Show("Please, choose a language.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (String.IsNullOrEmpty(Machin.format))
            {
                MessageBox.Show("Please, choose a sub format.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Machin.FirstStep();



        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("All informations on github. https://github.com/skid9000/Crunchyroll-Downloader", "About", MessageBoxButton.OK);
        }
    }
}
