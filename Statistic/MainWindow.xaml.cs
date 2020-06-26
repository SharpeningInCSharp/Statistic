using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DiagramsModel;
using System.Windows.Input;
using System.Windows.Media;
using Histogram;
using PieDiagramControls;
using Statistic.AdditionalWindows;
using Microsoft.Win32;
using Utils;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace Statistic
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Scopes[] scopes = null;

		public MainWindow()
		{
			InitializeComponent();
		}

		//TODO: create special util for color generation
		//TODO: extract common from diagrams

		private Brush[] brushes = new Brush[] { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Brown, Brushes.Chartreuse, Brushes.Purple };

		//TODO: can I do such thing - that I can drag files into

		private void DiagramSwitchButton_Click(object sender, Resources.Templates.SwitchButton.OnOffButtonClickHandlerEventArgs eventArgs)
		{
			if (scopes != null && scopes.Length == 1)
			{
				if (eventArgs.State)
				{

				}
			}
		}

		private void UploadFileButton_Click(object sender, RoutedEventArgs e)
		{
			var fileDialog = new OpenFileDialog()
			{
				InitialDirectory = System.IO.Path.GetFullPath("ExcelSamples"),
			};
			fileDialog.ShowDialog();

			if (fileDialog.FileName.EndsWith(".xlsx"))
			{
				LoadingAnimation.Visibility = Visibility.Visible;

				var excelParser = new ExcelExportManager(fileDialog.FileName);
				excelParser.ParsingComplete += ExcelExportManager_ParsingComplete;
				excelParser.ParseFileAsync();
			}
		}

		private void ExcelExportManager_ParsingComplete(ExcelExportManager exportManager, IEnumerable<ValuesBunch> items)
		{
			var readItems = items;

			var itemsAmount = items.Count();

			Dispatcher.Invoke(() =>
			{
				if (itemsAmount == 0)
					MessageBox.Show("File is empty and bla-bla-bla... so, read info!");
				else
					DiagramsSwitchStackPanel.Visibility = items.Count() > 1 ? Visibility.Hidden : Visibility.Visible;
			}
			);
			InitializeScopes(items, itemsAmount);

			Dispatcher.Invoke(() => LoadingAnimation.Visibility = Visibility.Hidden);
		}

		private void InitializeScopes(IEnumerable<ValuesBunch> items, int itemsAmount)
		{
			scopes = new Scopes[itemsAmount];
			for (int i = 0; i < itemsAmount; i++)
			{
				scopes[i] = new Scopes(items.ElementAt(i));
			}
		}

		private void AppInfo_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var infoWin = new AppInfoWindow();
			infoWin.ShowDialog();
		}

		#region WindowManagmentControls
		private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			MainGrid.Focus();
		}

		private void CollapseWindowButton_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}
		#endregion
	}
}
