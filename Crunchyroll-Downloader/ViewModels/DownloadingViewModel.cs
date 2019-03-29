using CrunchyrollDownloader.Progress;

namespace CrunchyrollDownloader.ViewModels
{
	public class DownloadingViewModel : PropertyChangedObject
	{
		public TaskManager Progress { get; set; }
		private bool _isIndeterminate;

		public bool IsIndeterminate
		{
			get => _isIndeterminate;
			set => Set(value, out _isIndeterminate);
		}
	}
}