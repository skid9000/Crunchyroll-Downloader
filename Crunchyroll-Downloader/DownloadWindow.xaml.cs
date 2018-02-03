using System.Windows;

namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for download.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        public DownloadWindow()
        {
            InitializeComponent();
            Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
    }
}