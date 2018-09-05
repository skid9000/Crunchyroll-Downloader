using System.Threading;
using System.Windows;


namespace CrunchyrollDownloader
{
	/// <summary>
	/// Interaction logic for download.xaml
	/// </summary>
	/// 
	
	public partial class DownloadWindow : Window
	{

        public string MyString { get; set; }

        public DownloadWindow()
			{
			    Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
                //var machin = new Program();
                
                //download_bar.IsIndeterminate = true;
                InitializeComponent();

                dl_label.Content = this.MyString;

                //download_bar.Value = 1;
                //dl_status.progress = "1";


                //download_bar.Value = double.Parse(dl_status.progress);

        }

        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
	}
}