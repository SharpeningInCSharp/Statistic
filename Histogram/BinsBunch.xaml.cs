using DiagramsModel;
using System;
using System.Collections.Generic;
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
		///YMaxScale = Height of Y-axis

		List<Bin> Bins = new List<Bin>();

		public BinsBunch()
		{
			InitializeComponent();

			Initialize();
		}

		//TODO: add case if all items are negative
		private void Initialize()
		{
			///foreach var item in scopes
			///var b = new Bin(YMaxScale*item.Ratio);
			///bins.Add(bin)
		}

		///According to Max and ratio create Bin(Height=MaxHeight*Ratio)
	}
}
