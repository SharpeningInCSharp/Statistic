using System.Windows;
using System.Windows.Controls;

namespace DiagramsDataOutput
{
	/// <summary>
	/// Логика взаимодействия для DiagramStatInfo.xaml
	/// </summary>
	public partial class DiagramStatInfo : UserControl
	{
		private Style TbStyle { get; }
		private int itemsAmount = 0;
		private const int UnitedColumnFontSize = 22;

		public string Header
		{
			get => headerTexBlock.Text;
			set
			{
				headerTexBlock.Visibility = Visibility.Visible;
				headerTexBlock.Text = value;
			}
		}

		public DiagramStatInfo()
		{
			InitializeComponent();
			TbStyle = FindResource("TextBlockStyle") as Style;
			MainGrid.RowDefinitions.Add(new RowDefinition());
		}

		public void Add(string unitedColumn)
		{
			var columnTb = new TextBlock()
			{
				Text = unitedColumn,
				Style = TbStyle,
				FontWeight = FontWeights.Bold,
				FontSize = UnitedColumnFontSize,
				TextAlignment = TextAlignment.Left,
			};

			MainGrid.RowDefinitions.Add(new RowDefinition());

			AddItemToGrid(columnTb, itemsAmount, 0);
			Grid.SetColumnSpan(columnTb, 2);

			itemsAmount++;
		}

		public void Add(string column1, string column2)
		{
			var column1Tb = new TextBlock()
			{
				Text = column1,
				Style = TbStyle,
				TextAlignment = TextAlignment.Left,
			};

			var column2Tb = new TextBlock()
			{
				Text = column2,
				Style = TbStyle,
				TextAlignment = TextAlignment.Right,
			};

			MainGrid.RowDefinitions.Add(new RowDefinition());

			AddItemToGrid(column1Tb, itemsAmount, 0);
			AddItemToGrid(column2Tb, itemsAmount, 1);

			itemsAmount++;
		}

		private void AddItemToGrid(UIElement uIElement, int row, int column)
		{
			Grid.SetColumn(uIElement, column);
			Grid.SetRow(uIElement, row);

			MainGrid.Children.Add(uIElement);
		}

		public void Clear()
		{
			headerTexBlock.Visibility = Visibility.Collapsed;
			itemsAmount = 0;

			MainGrid.RowDefinitions.Clear();
			MainGrid.RowDefinitions.Add(new RowDefinition());
			Header = "";
			MainGrid.Children.Clear();
		}
	}
}
