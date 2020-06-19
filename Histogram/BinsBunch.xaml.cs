using DiagramsModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Histogram
{
	/// <summary>
	/// Логика взаимодействия для BinsBunch.xaml
	/// </summary>
	public partial class BinsBunch : UserControl
	{
		List<Bin> Bins = new List<Bin>();

		public delegate double CalcultateHeightHandler(decimal currentValue);

		public BinsBunch(Scopes scopes, Brush[] brushes, CalcultateHeightHandler calcultateHeightHandler)
		{
			if (scopes is null)
			{
				throw new ArgumentNullException(nameof(scopes));
			}

			if (calcultateHeightHandler is null)
			{
				throw new ArgumentNullException(nameof(calcultateHeightHandler));
			}

			InitializeComponent();

			this.brushes = brushes ?? throw new ArgumentNullException(nameof(brushes));
			Initialize(scopes, calcultateHeightHandler);
		}


		private readonly Brush[] brushes;

		private void Initialize(Scopes scopes, CalcultateHeightHandler calcultateHeightHandler)
		{
			for (int i = 0; i < scopes.Count; i++)
			{
				var bin = new Bin(i, calcultateHeightHandler.Invoke(scopes[i].Sum))
				{
					Color = brushes[i],
				};
				bin.MouseOn += Bin_MouseOn;
				bin.MouseOut += Bin_MouseOut;

				bin.Shift();
				Bins.Add(bin);
				MainGrid.Children.Add(bin);
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
