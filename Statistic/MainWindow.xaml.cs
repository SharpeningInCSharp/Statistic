﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DiagramsModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Histogram;
using PieDiagramControls;

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

			Initialize();
		}
		private Brush[] brushes = new Brush[] { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Brown, Brushes.Chartreuse, Brushes.Purple };
		private void Initialize()
		{
			//TODO: extract common from diagrams
			//TODO: should I extend IScopeSelectionItem interface or create some other with DateTime
			var scopes1 = new Scopes(GetEnums1, GetValues1, DateTime.Today, null);
			var scopes2 = new Scopes(GetEnums2, GetValues2, DateTime.Today, null);

			var hdiagram = new HistoDiagram(brushes, scopes1, scopes2);
			var pDiagram = new PieDiagram(scopes1, brushes);
			MainGrid.Children.Add(hdiagram);
		}

		private IEnumerable<ValueItem> GetValues2(IEnumType enumItem, DateTime initial, DateTime? final)
		{
			var data = new List<ValueItem>()
			{
				new ValueItem(26,new StatEnumItem("Jopa")),
				new ValueItem(56,new StatEnumItem("Jopa")),
				new ValueItem(76,new StatEnumItem("Pupa")),
				new ValueItem(66,new StatEnumItem("Pupa")),
				new ValueItem(96,new StatEnumItem("Lupa")),
				new ValueItem(226,new StatEnumItem("Zalupa")),
				new ValueItem(24,new StatEnumItem("Zalupa")),
			};

			return data.Where(x => x.EnumType.Equals(enumItem));
		}

		private IEnumerable<StatEnumItem> GetEnums2()
		{
			return new List<StatEnumItem>()
			{
				new StatEnumItem("Jopa"),
				new StatEnumItem("Pupa"),
				new StatEnumItem("Lupa"),
				new StatEnumItem("Zalupa"),
			};
		}

		private IEnumerable<ValueItem> GetValues1(IEnumType enumItem, DateTime initial, DateTime? final)
		{
			var data = new List<ValueItem>()
			{
				new ValueItem(16,new StatEnumItem("Jopa")),
				new ValueItem(26,new StatEnumItem("Jopa")),
				new ValueItem(76,new StatEnumItem("Pupa")),
				new ValueItem(36,new StatEnumItem("Pupa")),
				new ValueItem(96,new StatEnumItem("Lupa")),
			};

			return data.Where(x => x.EnumType.Equals(enumItem));
		}

		private IEnumerable<StatEnumItem> GetEnums1()
		{
			return new List<StatEnumItem>()
			{
				new StatEnumItem("Jopa"),
				new StatEnumItem("Pupa"),
				new StatEnumItem("Lupa"),
			};
		}

		//TODO: how to can I do such think that I can drag files into
		private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{

		}

		private void InfoButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void UploadFileButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void CollapseWindowButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void DiagramSwitchButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
