using System;
using System.ComponentModel;
using CrunchyrollDownloader.Progress;

namespace CrunchyrollDownloader.ViewModels
{
	public class ProgressViewModel : PropertyChangedObject
	{
		public TaskManager Progress
		{
			get => _progress;
			set
			{
				if (_progress != null)
					_progress.PropertyChanged -= ProgressOnPropertyChanged;

				_progress = value;
				_progress.PropertyChanged += ProgressOnPropertyChanged;
			}
		}

		private void ProgressOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(nameof(IsIndeterminate));
		}

		private bool _isIndeterminate;
		private TaskManager _progress;

		public bool IsIndeterminate
		{
			get => _isIndeterminate || Progress.CurrentTask is null;
			set => Set(value, out _isIndeterminate);
		}

		public Func<bool> CanClose { get; set; } = () => false;
		public Action OnClose { get; set; } = () => { };
	}
}