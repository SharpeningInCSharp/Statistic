using DiagramsModel;
using Model;
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
	/// Логика взаимодействия для HistoDiagram.xaml
	/// </summary>
	public partial class HistoDiagram : UserControl
	{
		private const int BunchesSpace = 30;


		public HistoDiagram(params Scopes<StatEnumItem, ValueItem>[] scopesCollection)
		{
			InitializeComponent();

			OnInitialized(scopesCollection);
		}

		private void OnInitialized(params Scopes<StatEnumItem, ValueItem>[] scopesCollection)
		{
			//TODO: logic to shiftBunch
			foreach (var scopes in scopesCollection)
			{
				var b = new BinsBunch(scopes);
				DiagramGrid.Children.Add(b);
			}
		}

		///Find Max value, and max Y-value will be a bit bigger
		///can be created using Scopes or Scopes[]
	}
}
