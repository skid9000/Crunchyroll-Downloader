using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CrunchyrollDownloader
{
	public class PropertyChangedObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		protected void Set<TVal>(TVal value, out TVal output, [CallerMemberName] string propertyName = null)
		{
			output = value;
			OnPropertyChanged(propertyName);
		}
	}
}