namespace CrunchyrollDownloader.Models
{
	public class DownloaderModel
	{
		public bool AreSubtitlesEnabled { get; set; }
		public string Url { get; set; }
		public string Language { get; set; } = "enUS";
		public string Format { get; set; } = "ass";
		public string SavePath { get; set; }
		public Quality Quality { get; set; } = Quality.Best;
		public bool IsMkv { get; set; }
		public string DlStatus { get; set; }
	}
}
