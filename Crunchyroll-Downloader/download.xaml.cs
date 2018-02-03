using System.Windows;

namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for download.xaml
    /// </summary>
    public partial class download : Window
    {
        public download()
        {
            InitializeComponent();
            Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
    }
}