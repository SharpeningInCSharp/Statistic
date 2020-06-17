using DiagramsModel;
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
		double YMaxScale => 0.95 * ActualHeight;
		public const int GapsAmount = 10;

		List<Bin> Bins = new List<Bin>();

		public BinsBunch(Scopes<StatEnumItem, ValueItem> scopes)
		{
			InitializeComponent();

			Initialize(scopes);
		}

		//TODO: add case if all items are negative
		private void Initialize(Scopes<StatEnumItem, ValueItem> scopes)
		{
			var generalShift = Bin.BinsWidth;
			foreach (var scope in scopes)
			{
				var b = new Bin(scope.Ratio * YMaxScale);
				b.Shift(generalShift);
				generalShift += Bin.BinsWidth;
				Bins.Add(b);
				MainGrid.Children.Add(b);
			}
		}

		///According to Max and ratio create Bin(Height=MaxHeight*Ratio)
	}
}
