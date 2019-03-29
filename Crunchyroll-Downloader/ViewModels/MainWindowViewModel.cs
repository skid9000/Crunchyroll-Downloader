using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CrunchyrollDownloader.Models;
using CrunchyrollDownloader.Progress;
using static System.Globalization.CultureInfo;
using static CrunchyrollDownloader.Quality;
namespace CrunchyrollDownloader.ViewModels
{
	public class MainWindowViewModel : ViewModelBase<DownloaderModel>
	{
		public MainWindowViewModel() : this(null)
		{

		}

		public MainWindowViewModel(DownloaderModel model = null) : base(model)
		{
		}

		public string DlStatus
		{
			get => Model.DlStatus;
			set => SetModelProperty(value);
		}

		public string[] AvailableFormats { get; } =
		{
			"srt",
			"ass" // lol
		};

		public string Format
		{
			get => Model.Format;
			set => SetModelProperty(value);
		}

		public string Language
		{
			get => Model.Language;
			set => SetModelProperty(value);
		}

		public string MkvStatus
		{
			get => Model.MkvStatus;
			set => SetModelProperty(value);
		}

		public QualityItem Quality
		{
			get => new QualityItem(Model.Quality);
			set => SetModelProperty(value.Quality);
		}

		public string SavePath
		{
			get => Model.SavePath;
			set => SetModelProperty(value);
		}

		public bool AreSubtitlesEnabled
		{
			get => Model.AreSubtitlesEnabled;
			set => SetModelProperty(value);
		}

		public string Url
		{
			get => Model.Url;
			set => SetModelProperty(value);
		}
		private int _selectedQualityItemIndex = 1;

		public int SelectedQualityItemIndex
		{
			get => _selectedQualityItemIndex;
			set
			{
				Set(value, out _selectedQualityItemIndex);
				Quality = QualityItem.AllQualities[value];
			}
		}

		private QualityItem _selectedQualityItem;

		public QualityItem SelectedQualityItem
		{
			get => QualityItem.AllQualities[_selectedQualityItemIndex];
			set => SelectedQualityItemIndex = Array.IndexOf(QualityItem.AllQualities, value);
		}

		public bool HasCookies => File.Exists(@"C:\ProgramData\Crunchy-DL\cookies.txt");
		public bool HasNoCookies => !HasCookies;

		public List<CultureInfo> AvailableLanguages { get; } = new List<CultureInfo>
		{
			GetCultureInfo("fr-FR"),
			GetCultureInfo("en-US"),
			GetCultureInfo("es-ES"),
			GetCultureInfo("es-LA"),
			GetCultureInfo("pt-BR"),
			GetCultureInfo("it-IT"),
			GetCultureInfo("de-DE"),
			GetCultureInfo("ru-RU")
		};
		private CultureInfo _selectedCultureInfo;

		public CultureInfo SelectedLanguage
		{
			get
			{
				if (_selectedCultureInfo is null)
				{
					if (AvailableLanguages.Contains(InstalledUICulture)) _selectedCultureInfo = InstalledUICulture;
					else _selectedCultureInfo = AvailableLanguages[1];
				}
				return _selectedCultureInfo;
			}
			set
			{
				Set(value, out _selectedCultureInfo);
				Language = new string(_selectedCultureInfo.Name.Where(c => c != '-').ToArray());
			}
		}

		public void UpdateCookies()
		{
			OnPropertyChanged(nameof(HasCookies));
			OnPropertyChanged(nameof(HasNoCookies));
		}
		public async Task Download()
		{

			var process = new Process
			{
				StartInfo =
				{
					FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe",
					WindowStyle = ProcessWindowStyle.Hidden,
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true
				}
			};

			SavePath += @"\%(title)s.%(ext)s";
			string BasicArguments = $" --cookies C:\\ProgramData\\Crunchy-DL\\cookies.txt --download-archive C:\\ProgramData\\Crunchy-DL\\youtube-dl_archive.txt --user-agent \"Mozilla / 5.0 (Windows NT 10.0; Win64; x64; rv: 65.0) Gecko / 20100101 Firefox / 65.0\" --no-part -o \"{SavePath}\" ";
			string SubsArguments = $" --write-sub --sub-lang {Language} --sub-format {Format} ";

			if (AreSubtitlesEnabled)
			{
				if (MkvStatus == "1")
				{
					if (Quality == Best)
						process.StartInfo.Arguments = $"-f best --recode-video mkv --embed-subs --postprocessor-args \"-disposition:s:0 default\" {SubsArguments} {BasicArguments} {Url}";
					else
						process.StartInfo.Arguments = $"-f \"best[height={Quality}]\" --recode-video mkv --embed-subs --postprocessor-args \"-disposition:s:0 default\" {SubsArguments} {BasicArguments} {Url}";
				}
				else
				{
					if (Quality == Best)
						process.StartInfo.Arguments = $"-f best {SubsArguments} {BasicArguments} {Url}";
					else
						process.StartInfo.Arguments = $"-f \"best[height={Quality}]\" {SubsArguments} {BasicArguments} {Url}";
				}
			}
			else
			{
				if (Quality == Best)
					process.StartInfo.Arguments = $"-f best {BasicArguments} {Url}";
				else
					process.StartInfo.Arguments = $"-f \"best[height={Quality}]\" {BasicArguments} {Url}";
			}

			var data = new DownloadingViewModel
			{
				IsIndeterminate = true,
				Progress = new TaskManager(new[]
				{
					new ProgressTask("Downloading")
				})
			};
			var downloadWindow = new DownloadWindow(data);
			downloadWindow.Show();
			downloadWindow.Activate();
			process.ErrorDataReceived += (_, __) => { };
			process.OutputDataReceived += (sender, args) =>
			{
				if (!args.Data.StartsWith("[download]")) return;
				var match = Regex.Match(args.Data, "([0-9.]+)%");
				if (match.Success && double.TryParse(match.Groups[0].Value, out var percentage))
				{
					data.Progress.CurrentTask.Progress = percentage / 100;
				}
			};
			process.Start();
			await Task.Delay(10);
			// process.BeginOutputReadLine();
			await Task.Run(() => process.WaitForExit()); // do not block other threads.
			downloadWindow.Close();
			MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}