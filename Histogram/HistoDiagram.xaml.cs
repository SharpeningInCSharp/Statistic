using DiagramsModel;
using System;
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

		//SOLVE: add interraction with DiagramDataOutput
		//SOLVE: add caprion under BinsBunch
		public HistoDiagram(Brush[] brushes, params Scopes[] scopesCollection)
		{
			if (scopesCollection is null)
			{
				throw new ArgumentNullException(nameof(scopesCollection));
			}
			Brushes = brushes ?? throw new ArgumentNullException(nameof(brushes));

			maxValue = scopesCollection.Max(s => s.Max(i => i.Sum));
			minValue = scopesCollection.Min(s => s.Min(i => i.Sum));
			valueAxiesStep = (double)(maxValue - minValue) / (GapsAmount - 1);
			yAxiesStep = YMaxScale / GapsAmount;

			InitializeComponent();

			OnInitialized(scopesCollection);
		}

		private void OnInitialized(params Scopes[] scopesCollection)
		{
			InitializeAxies();

			//TODO: set numbers accordig to EnumValues
			foreach (var scopes in scopesCollection)
			{
				var binsBunch = new BinsBunch(scopes, Brushes, CalculateItemHeight);
				binsBunch.Shift(BunchesSpace + da, BottomMargin + da);
				Grid.SetRow(binsBunch, 1);
				DiagramGrid.Children.Add(binsBunch);
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

		///Find Max value, and max Y-value will be a bit bigger
	}
}
