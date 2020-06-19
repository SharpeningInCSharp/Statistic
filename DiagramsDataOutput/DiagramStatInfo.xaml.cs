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

		/// <summary>
		/// Table header
		/// </summary>
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

		/// <summary>
		/// Adds new line to table. One value in united columns
		/// </summary>
		/// <param name="unitedColumn">Value to be added</param>
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

		/// <summary>
		/// Adds new line to table. Each value in each column
		/// </summary>
		/// <param name="column1">First column value</param>
		/// <param name="column2">Second column value</param>
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

		/// <summary>
		/// Clears output info
		/// </summary>
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
