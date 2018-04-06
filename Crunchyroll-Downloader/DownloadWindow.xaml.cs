using System.Threading;
using System.Windows;


namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for download.xaml
    /// </summary>
    /// 
    public class dl_status
    {
        public string progress { get; set; }
        public string label_dl { get; set; }
    }
    public partial class DownloadWindow : Window
    {
        
            public DownloadWindow()
            {
            Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
            var dl_status = new dl_status();
                InitializeComponent();
                download_bar.IsIndeterminate = false;
                download_bar.Value = 1;
                dl_status.progress = "1";
                while (true) {
                    dl_label.Content = dl_status.label_dl;
                    download_bar.Value = double.Parse(dl_status.progress);
                }
            }

            void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
    }
}