using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Statistic.AdditionalWindows
{
	/// <summary>
	/// Логика взаимодействия для AppInfoWindow.xaml
	/// </summary>
	public partial class AppInfoWindow : Window
	{
		private static readonly string samplePath1 = @"ExcelSamples\Sample 1.xlsx";
		private static readonly string samplePath2 = @"ExcelSamples\Sample 2.xlsx";
		private static readonly string samplePath3 = @"ExcelSamples\Sample 3.xlsx";
		private static readonly string samplePath4 = @"ExcelSamples\Sample 4.xlsx";

		private readonly List<string> samples;

		public AppInfoWindow()
		{
			InitializeComponent();

			samples = new List<string>() { samplePath1, samplePath2, samplePath3, samplePath4 };
		}

		private void OpenSample_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var rnd = new Random();
			for (int i = 0; i < 6; i++)
			{
				var sampleInd = rnd.Next(0, samples.Count);

				if (File.Exists(samples[sampleInd]))
				{
					var fullPath = Path.GetFullPath(samples[sampleInd]);
					var p = new Process
					{
						StartInfo = new ProcessStartInfo(fullPath)
						{
							UseShellExecute = true,
						},
					};
					p.Start();
					
					//Process.Start("cmd.exe ",$@"/c {fullPath}");
					return;
				}
			}

			

			MessageBox.Show("Sorry, there're no samples available :(");
		}
	}
}
