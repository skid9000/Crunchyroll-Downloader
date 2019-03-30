using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CrunchyrollDownloader.Progress
{
	public class TaskManager : PropertyChangedObject
	{
		public void GoNext() => CurrentTask.Complete();
		public List<ProgressTask> Tasks { get; } = new List<ProgressTask>();

		public double TotalProgress
		{
			get
			{
				var totalProgress = Tasks.Sum(t => t.Weight);
				var completedProgress = Tasks.Sum(t => t.Weight * t.Progress);
				return completedProgress / totalProgress;
			}
		}

		public string CurrentDisplay => CurrentTask is null ? "Complete!" : CurrentTask.GetFormattedDisplay(Tasks.IndexOf(CurrentTask) + 1, Tasks.Count);
		public ProgressTask CurrentTask => Tasks.FirstOrDefault(p => !p.IsComplete);
		public TaskManager(IEnumerable<ProgressTask> tasks)
		{
			Tasks.AddRange(tasks);
			foreach (var task in Tasks)
			{
				task.PropertyChanged += UpdateProgress;
			}
		}

		private void UpdateProgress(object sender, PropertyChangedEventArgs e)
		{
			var task = (ProgressTask) sender;
			OnPropertyChanged(nameof(TotalProgress));
			OnPropertyChanged(nameof(CurrentDisplay));
			OnPropertyChanged(nameof(CurrentTask));
		}
	}

	public class ProgressTask : PropertyChangedObject
	{
		public ProgressTask(string name = "Something", int weight = 100)
		{
			Name = name;
			Weight = weight;
		}
		public string GetFormattedDisplay(int current, int remaining)
		{
			return string.Format(Display, current, remaining, Progress.ToString("P"), Name);
		}
		public string Name { get; set; }
		public string Display { get; set; } = "{3}... {2} [{0}/{1}]";

		public double Weight { get; } = 100;

		private double _progress;
		/// <summary>
		/// Progress, from 0 to 1
		/// </summary>
		public double Progress
		{
			get => _progress;
			set => Set(Math.Min(value, 100), out _progress);
		}

		public bool IsComplete => Progress >= 1 - double.Epsilon * 10;
		public void Complete()
		{
			Progress = 1;
		}
	}
}