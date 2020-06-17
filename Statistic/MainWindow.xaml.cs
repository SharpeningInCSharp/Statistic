using Model;
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

		private void Initialize()
		{
			var scopes = new Scopes<StatEnumItem, ValueItem>(GetEnums,)
		}

		private IEnumerable<ValueItem> GetValues(StatEnumItem enumItem, DateTime initial, DateTime? final)
		{
			var data = new List<ValueItem>()
			{
				new ValueItem()
			};
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
