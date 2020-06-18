using DiagramsModel;
using Model;
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
		private const int BunchesSpace = 20;
		private const int BottomMargin = 20;

		public Thickness DiagramMargin => new Thickness(BunchesSpace, 0, 0, BottomMargin);
		public const int GapsAmount = 10;
		public Point Center { get; private set; } = new Point(0, 0);

		double YMaxScale => 300;
		double XMaxScale => 300;

		public HistoDiagram(params Scopes[] scopesCollection)
		{
			InitializeComponent();

			OnInitialized(scopesCollection);
		}

		private void OnInitialized(params Scopes[] scopesCollection)
		{
			InitializeAxies(scopesCollection.Max(s => s.Max(i => i.Max)),
							scopesCollection.Min(s => s.Min(i => i.Min)));

			foreach (var scopes in scopesCollection)
			{
				var b = new BinsBunch(scopes);
				b.Shift(BunchesSpace, BottomMargin);
				DiagramGrid.Children.Add(b);
			}
		}

		private void InitializeAxies(decimal max, decimal min)
		{
			//TODO: add case if all items are negative (max==0) - lift Center. Draw Y-axies to negative direction if (min<0)
			//TODO: add pointers
			var yAxiesStep = (double)(max - min) / GapsAmount;
			InitializeXAxis();
			InitializeYAxis();
		}

		private void InitializeYAxis()
		{
			var yAxies = new PathFigure()
			{
				StartPoint = Center,
				IsFilled = false,
				IsClosed = false,
			};
			yAxies.Segments.Add(new LineSegment(new Point(Center.X, Center.Y - YMaxScale), true));
			Geometries.Figures.Add(yAxies);
		}

		private void InitializeXAxis()
		{
			var xAxies = new PathFigure()
			{
				StartPoint = Center,
				IsFilled = false,
				IsClosed = false,
			};
			xAxies.Segments.Add(new LineSegment(new Point(Center.X + XMaxScale, Center.Y), true));
			Geometries.Figures.Add(xAxies);
		}

		private void DrawArrow(PathFigure figure, double xCentre, double yCentre)
		{

		}

		///Find Max value, and max Y-value will be a bit bigger
	}
}
