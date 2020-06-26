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
	/// Логика взаимодействия для HistoDiagram.xaml
	/// </summary>
	public partial class HistoDiagram : UserControl
	{
		//TODO: should bind shift to Center
		private const int BunchesSpace = 20;
		private const int BottomMargin = 20;
		private const double PureScalePerPercentage = 0.95;

		public Thickness DiagramMargin => new Thickness(BunchesSpace, 0, 0, BottomMargin);

		public const int GapsAmount = 10;
		private readonly decimal maxValue;
		private readonly decimal minValue;
		private readonly double valueAxiesStep;
		private readonly double yAxiesStep;

		public Point Center { get; private set; } = new Point(0, 0);

		double wholeXScale = 450;
		double wholeYScale = 450;

		double YMaxScale => PureScalePerPercentage * wholeYScale - ArrowLength;

		double XMaxScale => PureScalePerPercentage * wholeXScale - ArrowLength;

		public Brush[] Brushes { get; }

		private readonly Scopes[] scopesCollection;
		private readonly List<BinsBunch> binsBunches = new List<BinsBunch>();
		private readonly List<PieLegendItem> enumTypes = new List<PieLegendItem>();

		//SOLVE: add interraction with DiagramDataOutput
		//SOLVE: add caprion under BinsBunch
		public HistoDiagram(Brush[] brushes, params Scopes[] scopesCollection)
		{
			if (scopesCollection is null)
			{
				throw new ArgumentNullException(nameof(scopesCollection));
			}
			this.scopesCollection = scopesCollection;

			Brushes = brushes ?? throw new ArgumentNullException(nameof(brushes));

			maxValue = scopesCollection.Max(s => s.Max(i => i.Sum));
			minValue = scopesCollection.Min(s => s.Min(i => i.Sum));
			valueAxiesStep = (double)(maxValue - minValue) / (GapsAmount - 1);
			yAxiesStep = YMaxScale / GapsAmount;

			InitializeComponent();

			OnInitialized();

			OutAllData();
		}

		private void OnInitialized()
		{
			InitializeAxies();

			int genAmount = 0;
			for (int i = 0; i < scopesCollection.Length; i++)
			{
				var binsBunch = new BinsBunch(scopesCollection[i], Brushes, CalculateItemHeight, AddNewLegendItem);
				binsBunch.StatTypeSelected += BinsBunch_StatTypeSelected;
				binsBunch.StatTypeUnselected += BinsBunch_StatTypeUnselected;

				binsBunch.Shift((BunchesSpace + da) * (genAmount + 1), BottomMargin + da);
				genAmount += binsBunch.Count;

				binsBunches.Add(binsBunch);

				Grid.SetColumn(binsBunch, 1);
				DiagramGrid.Children.Add(binsBunch);
			}
		}

		private void AddNewLegendItem(IEnumType enumType, Brush brush)
		{
			if (enumTypes.Count(x => x.EnumType.Equals(enumType)) == 0)
			{
				var legendItem = new PieLegendItem(enumType, brush);
				legendItem.MouseOn += LegendItem_MouseOn;
				legendItem.MouseOut += LegendItem_MouseOut;
				LegendItemsStackPanel.Children.Add(legendItem);
				enumTypes.Add(legendItem);
			}
		}

		private void LegendItem_MouseOut(IEnumType enumType)
		{
			BinsBunch_StatTypeUnselected();
		}

		private void LegendItem_MouseOn(IEnumType enumType)
		{
			BinsBunch_StatTypeSelected(enumType);
		}

		private void BinsBunch_StatTypeUnselected()
		{
			UnselectAllItems();
			OutAllData();
		}

		private void OutAllData()
		{
			DiagramStatInfo.Clear();

			DiagramStatInfo.Header = "General info";

			foreach (var scopes in scopesCollection)
			{
				DiagramStatInfo.Add(scopes.DatesToString());

				foreach (var type in scopes.EnumValues)
				{
					DiagramStatInfo.Add($"{scopes[type].EnumMember} - {scopes[type].Sum:f2}", scopes[type].Ratio.ToString("P2"));
				}

				DiagramStatInfo.Add($"Sum: {scopes.TotalSum:f2}", DiagramStatInfo.ColumnType.Data);

				var avgPercent = scopes.TotalSum / scopesCollection.Sum(x => x.TotalSum);
				DiagramStatInfo.Add($"Average: {scopes.Average(x => x.Sum):f2}", $"{avgPercent:P2}");
			}
		}

		private void UnselectAllItems()
		{
			foreach (var item in binsBunches)
			{
				item.Unselect();
			}
		}

		//TODO: add LoadNew(Scopes) method
		//SOLVE: extract some interfaces and other common stuff
		//SOLVE: remake a bit: probably I shouldn't use Model parts at Controls as much
		private void BinsBunch_StatTypeSelected(IEnumType enumType)
		{
			SelectAllItems(enumType);
			OutTypeData(enumType);
		}

		private void OutTypeData(IEnumType enumType)
		{
			DiagramStatInfo.Clear();
			DiagramStatInfo.Header = enumType.Item;

			var items = binsBunches.Select(x => x.Scopes[enumType]).Where(x => x != null);

			foreach (var item in items)
			{
				DiagramStatInfo.Add(item.Sum.ToString("f2"), item.Ratio.ToString("P2"));
			}
			var sum = items.Sum(x => x.Sum);
			DiagramStatInfo.Add($"Total: {sum:f2}", DiagramStatInfo.ColumnType.Data);

			var avgPercent = sum / binsBunches.Sum(x => x.Scopes.TotalSum);
			DiagramStatInfo.Add($"Average: {items.Average(x => x.Sum):f2}", $"{avgPercent:P2}");
		}

		private void SelectAllItems(IEnumType enumType)
		{
			var items = binsBunches.Where(x => x.Scopes.EnumValues.Contains(enumType));     //items with that type

			foreach (var item in items)
			{
				item.Select(enumType);
			}
		}

		private double CalculateItemHeight(decimal value)
		{
			return yAxiesStep * ((double)(value - minValue) / valueAxiesStep + 1);
		}

		private void InitializeAxies()
		{
			//TODO: add case if all items are negative (max==0) - lift Center. Draw Y-axies to negative direction if (min<0)
			InitializeXAxis();
			InitializeYAxis();
		}

		private const double ArrowAngle = 30 * Math.PI / 180;
		private const int ArrowLength = 20;

		/// <summary>
		/// Side opposite of arrow angle of half-arrow triangle
		/// </summary>
		private readonly double da = ArrowLength * Math.Sin(ArrowAngle / 2);

		/// <summary>
		/// Side fit to arrow angle of half-arrow triangle
		/// </summary>
		private readonly double db = ArrowLength * Math.Cos(ArrowAngle / 2);

		private void InitializeYAxis()
		{
			var yAxies = new PathFigure()
			{
				StartPoint = Center,
				IsFilled = false,
				IsClosed = false,
			};

			var axisLine = new LineSegment(new Point(Center.X, Center.Y - wholeYScale), true);

			var arrowSideLine1 = new LineSegment(new Point(axisLine.Point.X - da, axisLine.Point.Y + db),
															true);

			var arrowBottomLine = new LineSegment(new Point(arrowSideLine1.Point.X + 2 * da, arrowSideLine1.Point.Y),
															false);

			yAxies.Segments.Add(axisLine);
			yAxies.Segments.Add(arrowSideLine1);
			yAxies.Segments.Add(arrowBottomLine);
			yAxies.Segments.Add(axisLine);

			Geometries.Figures.Add(yAxies);

			InitializeScales();
		}

		private void InitializeXAxis()
		{
			var xAxies = new PathFigure()
			{
				StartPoint = Center,
				IsFilled = false,
				IsClosed = false,
			};

			var axisLine = new LineSegment(new Point(Center.X + wholeXScale, Center.Y), true);

			var arrowSideLine1 = new LineSegment(new Point(axisLine.Point.X - db, axisLine.Point.Y + da),
															true);

			var arrowBottomLine = new LineSegment(new Point(arrowSideLine1.Point.X, arrowSideLine1.Point.Y - 2 * da),
															false);

			xAxies.Segments.Add(axisLine);
			xAxies.Segments.Add(arrowSideLine1);
			xAxies.Segments.Add(arrowBottomLine);
			xAxies.Segments.Add(axisLine);

			Geometries.Figures.Add(xAxies);
		}

		private const int ScaleStrokeLength = 8;

		private void InitializeScales()
		{
			for (int i = 1; i <= GapsAmount; i++)
			{
				var axisScale = new PathFigure()
				{
					StartPoint = new Point(Center.X - ScaleStrokeLength / 2, -(Center.Y + yAxiesStep * i)),
					IsClosed = false,
					IsFilled = false,
				};

				axisScale.Segments.Add(new LineSegment(new Point(axisScale.StartPoint.X + ScaleStrokeLength, axisScale.StartPoint.Y),
														true));

				Geometries.Figures.Add(axisScale);
			}
		}
	}
}
