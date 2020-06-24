using System.Windows;
using System.Windows.Controls;

namespace DiagramsDataOutput
{
	/// <summary>
	/// Логика взаимодействия для DiagramStatInfo.xaml
	/// </summary>
	public partial class DiagramStatInfo : UserControl
	{
		/// <summary>
		/// Manage single column output
		/// </summary>
		public enum ColumnType
		{
			/// <summary>
			/// Bold FontWeight
			/// </summary>
			Header,

			/// <summary>
			/// Normal FontWeight
			/// </summary>
			Data,
		}

		private Style TbStyle { get; }
		private int itemsAmount = 0;
		private const int UnitedColumnHeaderFontSize = 22;
		private const int UnitedColumnDataFontSize = 18;

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
		public void Add(string unitedColumn, ColumnType columnType = ColumnType.Header)
		{
			InitParams(columnType, out var fontWeight, out var fontSize, out var underline);

			var columnTb = new TextBlock()
			{
				Text = unitedColumn,
				Style = TbStyle,
				FontWeight = fontWeight,
				FontSize = fontSize,
				TextAlignment = TextAlignment.Left,
				TextDecorations = underline,
			};

			MainGrid.RowDefinitions.Add(new RowDefinition());

			AddItemToGrid(columnTb, itemsAmount, 0);
			Grid.SetColumnSpan(columnTb, 2);

			itemsAmount++;
		}

		private void InitParams(ColumnType columnType, out FontWeight fontWeight, out int fontSize, out TextDecorationCollection underline)
		{
			if (columnType == ColumnType.Header)
			{
				fontWeight = FontWeights.Bold;
				fontSize = UnitedColumnHeaderFontSize;
				underline = null;
			}
			else
			{
				fontWeight = FontWeights.Normal;
				fontSize = UnitedColumnDataFontSize;
				underline = TextDecorations.Underline;
			}
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
