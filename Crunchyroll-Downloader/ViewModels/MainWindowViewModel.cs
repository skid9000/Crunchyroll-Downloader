using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using CrunchyrollDownloader.Models;
using CrunchyrollDownloader.Progress;
using CrunchyrollDownloader.Views;
using Newtonsoft.Json;
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

        public bool IsMkv
        {
            get => Model.IsMkv;
            set => SetModelProperty(value);
        }

        public QualityItem Quality
        {
            get => new QualityItem(Model.Quality);
            set => SetModelProperty(value.InnerQuality);
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
        private int _selectedQualityItemIndex = 0;

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

        public bool HasLogin => File.Exists(@"C:\ProgramData\Crunchy-DL\login.json");
        public bool HasNoLogin => !HasLogin;

        public List<CultureInfo> AvailableLanguages { get; } = new List<CultureInfo>
        {
            GetCultureInfo("en-US"),
            GetCultureInfo("fr-FR"),
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
                    _selectedCultureInfo = AvailableLanguages[0];
                }
                return _selectedCultureInfo;
            }
            set
            {
                Set(value, out _selectedCultureInfo);
                Language = new string(_selectedCultureInfo.Name.Where(c => c != '-').ToArray());
            }
        }

        public void UpdateLogin()
        {
            OnPropertyChanged(nameof(HasLogin));
            OnPropertyChanged(nameof(HasNoLogin));
        }
        public async Task Download()
        {
            var loginJson = File.ReadAllText(@"C:\ProgramData\Crunchy-DL\login.json");
            dynamic loginIdents = JsonConvert.DeserializeObject(loginJson);
            string username = loginIdents.username;
            string password = loginIdents.password;

            var currentSavePath = SavePath;
            currentSavePath += @"\%(title)s.%(ext)s";

            var login = $"--username {username} --password {password}";
            var ua = $"--user-agent \"Mozilla / 5.0 (Windows NT 10.0; Win64; x64; rv: 65.0) Gecko / 20100101 Firefox / 65.0\"";
            var basicArguments = $"{login} --console-title --newline --no-warnings --no-part -o \"{currentSavePath}\" {ua}";
            var subsArguments = $"--write-sub --sub-lang {Language} --sub-format {Format}";
            var qualityArguments = Quality == Best ? "-f best" : $"-f \"best[height={Quality}]\"";

            var process = new Process
            {
                StartInfo =
                {
                    FileName = @"C:\ProgramData\Crunchy-DL\youtube-dl.exe",
                    WindowStyle = ProcessWindowStyle.Minimized,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                }
            };

            if (AreSubtitlesEnabled)
            {
                process.StartInfo.Arguments = IsMkv ? $"{qualityArguments} --recode-video mkv --embed-subs --postprocessor-args \"-disposition:s:0 default\" {subsArguments} {basicArguments}" 
                                                    : $"{qualityArguments} {subsArguments} {basicArguments}";
            }
            else
            {
                process.StartInfo.Arguments = $"{qualityArguments} {basicArguments}";
            }
            process.StartInfo.Arguments += " " + Url;
            var data = new ProgressViewModel
            {
                IsIndeterminate = true,
                //Progress = new TaskManager(new[]
                //{
                //    new ProgressTask("Downloading")
                //})
                Progress = new TaskManager("Downloading...")
            };
            var downloadWindow = new ProgressWindow(data);
            _ = Task.Run(() => Application.Current.Dispatcher.Invoke(() => downloadWindow.ShowDialog()));
            // TODO parse progress
            //process.ErrorDataReceived += (_, __) => { };
            //process.OutputDataReceived += (sender, args) =>
            //{
            //    if (!args.Data.StartsWith("[download]")) return;
            //    var match = Regex.Match(args.Data, "([0-9.]+)%");
            //    if (match.Success && double.TryParse(match.Groups[0].Value, out var percentage))
            //    {
            //        data.Progress.CurrentTask.Progress = percentage / 100;
            //    }
            //};
            process.Start();
            await Task.Delay(10);
            // process.BeginOutputReadLine();
            await Task.Run(() => process.WaitForExit()); // do not block other threads.
            downloadWindow.Close();
            MessageBox.Show("Download finished !", "Success !", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}