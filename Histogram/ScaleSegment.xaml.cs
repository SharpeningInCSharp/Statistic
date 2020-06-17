using System.Windows;
using System.Windows.Controls;

namespace Histogram
{
	/// <summary>
	/// Логика взаимодействия для ScaleSegment.xaml
	/// </summary>
	public partial class ScaleSegment : UserControl
	{
		public ScaleSegment()
		{
			InitializeComponent();
		}

		public void Shift(double shiftValue)
		{
			scaleSegmentPath.Margin = new Thickness(0, 0, 0, shiftValue);
		}
	}
}
