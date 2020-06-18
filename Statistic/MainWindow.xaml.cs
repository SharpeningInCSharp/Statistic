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
			var scopes = new Scopes(GetEnums, GetValues, DateTime.Today, null);
			var hdiagram = new HistoDiagram(scopes);
			var pDiagram = new PieDiagram(scopes,brushes);
			MainGrid.Children.Add(hdiagram);
		}

		private IEnumerable<ValueItem> GetValues(IEnumType enumItem, DateTime initial, DateTime? final)
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

		private IEnumerable<StatEnumItem> GetEnums()
		{
			return new List<StatEnumItem>()
			{
				new StatEnumItem("Jopa"),
				new StatEnumItem("Pupa"),
				new StatEnumItem("Lupa"),
			};
		}
	}
}
