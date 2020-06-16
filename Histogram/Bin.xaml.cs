using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Histogram
{
	/// <summary>
	/// Is an abstraction of diagram's bin item
	/// </summary>
	public partial class Bin : UserControl
	{
		public const int BinsWidth = 20;

		/// <summary>
		/// Current bin's color
		/// </summary>
		public Brush Color
		{
			get => MainRect.Fill;

			set
			{
				MainRect.Fill = value;
			}
		}


		public Bin(double binHeight)
		{
			InitializeComponent();

			MainRect.Height = binHeight;
		}

		/// <summary>
		/// Shifts object right for <paramref name="shiftValue"/>
		/// </summary>
		/// <param name="shiftValue">value to be shifted</param>
		public void Shift(int shiftValue)
		{
			Margin = new Thickness(shiftValue, 0, 0, 0);
		}

		//I can Light value either draw dotted line to this value or smth
		private void MainRect_MouseEnter(object sender, MouseEventArgs e)
		{

		}

		private void MainRect_MouseLeave(object sender, MouseEventArgs e)
		{

		}
	}
}
