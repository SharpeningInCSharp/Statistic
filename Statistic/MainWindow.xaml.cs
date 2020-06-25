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

namespace Statistic
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			ExcelExportManager.ParsingComplete += ExcelExportManager_ParsingComplete;
			//Initialize();
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

			Dispatcher.Invoke(() => LoadingAnimation.Visibility = Visibility.Hidden);
		}

		//TODO: create special util for color generation
		//TODO: extract common from diagrams
		//TODO: should I extend IScopeSelectionItem interface or create some other with DateTime

		private Brush[] brushes = new Brush[] { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Brown, Brushes.Chartreuse, Brushes.Purple };

		//private void Initialize()
		//{
		//	var scopes1 = new Scopes(GetEnums1, GetValues1, DateTime.Today, null);
		//	var scopes2 = new Scopes(GetEnums2, GetValues2, DateTime.Today, null);

		//	var hdiagram = new HistoDiagram(brushes, scopes1, scopes2);
		//	var pDiagram = new PieDiagram(scopes1, brushes);
		//	DiagramGrid.Children.Add(hdiagram);
		//}

		//private IEnumerable<ValueItem> GetValues2(IEnumType enumItem, DateTime initial, DateTime? final)
		//{
		//	var data = new List<ValueItem>()
		//	{
		//		new ValueItem(26,new StatEnumItem("Jopa")),
		//		new ValueItem(56,new StatEnumItem("Jopa")),
		//		new ValueItem(76,new StatEnumItem("Pupa")),
		//		new ValueItem(66,new StatEnumItem("Pupa")),
		//		new ValueItem(96,new StatEnumItem("Lupa")),
		//		new ValueItem(226,new StatEnumItem("Zalupa")),
		//		new ValueItem(24,new StatEnumItem("Zalupa")),
		//	};

		//	return data.Where(x => x.EnumType.Equals(enumItem));
		//}

		//private IEnumerable<StatEnumItem> GetEnums2()
		//{
		//	return new List<StatEnumItem>()
		//	{
		//		new StatEnumItem("Jopa"),
		//		new StatEnumItem("Pupa"),
		//		new StatEnumItem("Lupa"),
		//		new StatEnumItem("Zalupa"),
		//	};
		//}

		//private IEnumerable<ValueItem> GetValues1(IEnumType enumItem, DateTime initial, DateTime? final)
		//{
		//	var data = new List<ValueItem>()
		//	{
		//		new ValueItem(16,new StatEnumItem("Jopa")),
		//		new ValueItem(26,new StatEnumItem("Jopa")),
		//		new ValueItem(76,new StatEnumItem("Pupa")),
		//		new ValueItem(36,new StatEnumItem("Pupa")),
		//		new ValueItem(96,new StatEnumItem("Lupa")),
		//	};

		//	return data.Where(x => x.EnumType.Equals(enumItem));
		//}

		//private IEnumerable<StatEnumItem> GetEnums1()
		//{
		//	return new List<StatEnumItem>()
		//	{
		//		new StatEnumItem("Jopa"),
		//		new StatEnumItem("Pupa"),
		//		new StatEnumItem("Lupa"),
		//	};
		//}

		//TODO: can I do such thing - that I can drag files into

		private void DiagramSwitchButton_Click(object sender, Resources.Templates.SwitchButton.OnOffButtonClickHandlerEventArgs eventArgs)
		{

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
				ExcelExportManager.ParseFileAsync(fileDialog.FileName);
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
