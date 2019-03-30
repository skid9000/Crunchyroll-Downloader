using System;
using System.Collections.Generic;
using System.Linq;

namespace CrunchyrollDownloader
{
	public enum Quality
	{       
		Best,
		P1080,
		P720,
		P480,
		P360
	}

	public class QualityItem
	{
		public static QualityItem[] AllQualities => Enum.GetValues(typeof(Quality)).Cast<Quality>().Select(q => new QualityItem(q)).ToArray();
		public QualityItem(Quality innerQuality)
		{
			InnerQuality = innerQuality;
		}
		public string DisplayName => InnerQuality.ToDisplay();
		public Quality InnerQuality { get; set; }

		public static bool operator ==(QualityItem i, Quality q)
		{
			return i?.InnerQuality == q;
		}

		public static bool operator !=(QualityItem i, Quality q)
		{
			return !(i == q);
		}

		public override string ToString() => InnerQuality.ToNormalized();
	}
	public static class QualityExtensions
	{
		public static IReadOnlyList<string> Displays { get; } = new[]
		{
			"Best (recommended)",
			"1080p",
			"720p",
			"480p",
			"360p"
		};

		public static IReadOnlyList<string> Normalizations { get; } = new[]
		{
			"best",
			"1080",
			"720",
			"480",
			"360"
		};
		public static string ToDisplay(this Quality quality) => Displays[(int) quality];
		public static string ToNormalized(this Quality quality) => Normalizations[(int) quality];
	}
}