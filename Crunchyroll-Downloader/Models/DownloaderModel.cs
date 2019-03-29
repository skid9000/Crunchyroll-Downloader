namespace CrunchyrollDownloader.Models
{
    public class DownloaderModel
    {
        public bool AreSubtitlesEnabled { get; set; }
        public string Url { get; set; }
        public string Language { get; set; } = "enUS";
        public string Format { get; set; } = "srt";
        public string SavePath { get; set; }
        public Quality Quality { get; set; } = Quality.Best;
        public string MkvStatus { get; set; }
        public string DlStatus { get; set; }
    }
}
