using System.IO;
using System.Windows;
using CrunchyrollDownloader.ViewModels;
using Newtonsoft.Json;

namespace CrunchyrollDownloader.Views
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

			IdentsInfos i = new IdentsInfos();
			i.username = username;
			i.password = password;

			File.WriteAllText(@"C:\ProgramData\Crunchy-DL\login.json", JsonConvert.SerializeObject(i));

			_otherVm.UpdateLogin();
			Close();
			
		}
	}

	public class IdentsInfos
	{
		public string username { get; set; }
		public string password { get; set; }
	}

}