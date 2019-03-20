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

	public DownloadWindow()	{
			    Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
	        	InitializeComponent();
    }

	void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => e.Cancel = true;
	}
}