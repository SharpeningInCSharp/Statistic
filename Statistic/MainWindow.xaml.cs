using Model;
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
using Statistic.Resources.Templates;

namespace Statistic
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const int DisplayHeight = 1056;
		private const int DisplayWidth = 1936;
		Scopes[] scopesCollection = null;

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
			if (scopesCollection != null && scopesCollection.Length == 1)
			{
				InitDiagram(eventArgs.State);
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

			InitDiagram(DiagramSwitchButton.ButtonState);
		}

		private void InitDiagram(SwitchButton.SwitchButtonState state)
		{
			Dispatcher.Invoke(() =>
			{
				DiagramGrid.Children.Clear();

				if (state == SwitchButton.SwitchButtonState.Activated)
				{
					var pie = new PieDiagram(scopesCollection[0], brushes);
					DiagramGrid.Children.Add(pie);
				}
				else
				{
					var histo = new HistoDiagram(brushes, scopesCollection);
					DiagramGrid.Children.Add(histo);
				}

				LoadingAnimation.Visibility = Visibility.Hidden;
			}
			);
		}

		private void InitializeScopes(IEnumerable<ValuesBunch> items, int itemsAmount)
		{
			scopesCollection = new Scopes[itemsAmount];
			for (int i = 0; i < itemsAmount; i++)
			{
				scopesCollection[i] = new Scopes(items.ElementAt(i));
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
			if (WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
				Height = DisplayHeight;
				Width = DisplayWidth;
			}

			DragMove();
		}

		private void FullScreenWindowButton_Click(object sender, RoutedEventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				WindowState = WindowState.Normal;
				Height = DisplayHeight;
				Width = DisplayWidth;
			}

			Top = -6;
			Left = -6;
		}
		#endregion
	}
}
