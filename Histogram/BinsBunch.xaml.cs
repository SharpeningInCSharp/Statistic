using DiagramsDataOutput;
using DiagramsModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
		private Bin itemToBeSelected;

		public DateTime InitialDate { get; }
		public DateTime? FinalDate { get; }

		public event StatTypeSelectionHandler StatTypeSelected;
		public delegate void StatTypeSelectionHandler(IEnumType enumType);
		public event Action StatTypeUnselected;

		public delegate double CalcultateHeightHandler(decimal currentValue);

		public BinsBunch(Scopes scopes, Brush[] brushes, CalcultateHeightHandler calcultateHeightHandler)
		{
			if (scopes is null)
			{
				throw new ArgumentNullException(nameof(scopes));
			}
			Scopes = scopes;

			InitialDate = scopes.InitialDate;
			FinalDate = scopes.FinalDate;

			if (calcultateHeightHandler is null)
			{
				throw new ArgumentNullException(nameof(calcultateHeightHandler));
			}

			this.brushes = brushes ?? throw new ArgumentNullException(nameof(brushes));

			InitializeComponent();

			Initialize(calcultateHeightHandler);
		}

		internal Scopes Scopes { get; }
		private readonly Brush[] brushes;

		private void Initialize(CalcultateHeightHandler calcultateHeightHandler)
		{
			for (int i = 0; i < Scopes.Count; i++)
			{
				var bin = new Bin(Scopes[i], i, calcultateHeightHandler.Invoke(Scopes[i].Sum))
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

		public void Select(IEnumType enumType)
		{
			itemToBeSelected = Bins.FirstOrDefault(x => x.Scope.EnumMember.Equals(enumType) && x.Scope.Sum != 0);

			itemToBeSelected?.Select();
		}

		public void Unselect()
		{
			//foreach(var bin in Bins)
			//{
			//bin.Unselect();
			itemToBeSelected?.Unselect();
			//}
		}

		public void Shift(double left, double bottom)
		{
			Margin = new Thickness(left, 0, 0, bottom);
		}

		private void Bin_MouseOut(Bin sender)
		{
			Panel.SetZIndex(sender, 1);

			StatTypeUnselected?.Invoke();
		}

		private void Bin_MouseOn(Bin sender)
		{
			Panel.SetZIndex(sender, 100);

			StatTypeSelected?.Invoke(sender.Scope.EnumMember);
		}
	}
}
