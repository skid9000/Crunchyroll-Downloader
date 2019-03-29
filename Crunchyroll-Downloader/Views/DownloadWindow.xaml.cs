using System.ComponentModel;
using System.Windows;
using CrunchyrollDownloader.ViewModels;

namespace CrunchyrollDownloader
{
    /// <summary>
    /// Interaction logic for download.xaml
    /// </summary>
    /// 

    public partial class DownloadWindow : Window
    {
        private DownloadingViewModel _vm;
        public bool RestrictClose { get; set; } = true;
        public DownloadWindow(DownloadingViewModel vm = null)
        {
            DataContext = _vm = vm ?? new DownloadingViewModel();
            Closing += Window_Closing;
            InitializeComponent();
        }

        void Window_Closing(object sender, CancelEventArgs e) => e.Cancel = RestrictClose;

        public new void Close()
        {
            RestrictClose = false;
            base.Close();
        }
    }
}