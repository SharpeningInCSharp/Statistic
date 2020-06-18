﻿using DiagramsModel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Histogram
{
	/// <summary>
	/// Логика взаимодействия для BinsBunch.xaml
	/// </summary>
	public partial class BinsBunch : UserControl
	{
		/// <summary>
		/// Height of Y-axis
		/// </summary>
		double YMaxScale => 0.95 * 300;

		//TODO: should get Y/X-Scales thought ctor
		//TODO: bind to Height
		List<Bin> Bins = new List<Bin>();

		public BinsBunch(Scopes scopes)
		{
			InitializeComponent();

			Initialize(scopes);
		}


		private Brush[] brushes = new Brush[] { Brushes.Red, Brushes.Green, Brushes.Blue, Brushes.Brown, Brushes.Chartreuse, Brushes.Purple };

		
		private void Initialize(Scopes scopes)
		{
			for (int i = 0; i < scopes.Count; i++)
			{
				var b = new Bin(i, scopes[i].Ratio * YMaxScale)
				{
					Color = brushes[i],
				};
				b.MouseOn += Bin_MouseOn;
				b.MouseOut += Bin_MouseOut;

				b.Shift();
				Bins.Add(b);
				MainGrid.Children.Add(b);
			}
		}

		public void Shift(double left, double bottom)
		{
			Margin = new Thickness(left, 0, 0, bottom);
		}

		private void Bin_MouseOut(Bin sender)
		{
			Panel.SetZIndex(sender, 1);
		}

		private void Bin_MouseOn(Bin sender)
		{
			Panel.SetZIndex(sender, 100);
		}
	}
}
