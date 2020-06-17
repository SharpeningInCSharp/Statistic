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
		/// <summary>
		/// Width of item
		/// </summary>
		public const int BinsWidth = 20;

		private const int RotationNegativeAngle = 180;
		private const int RotationPositiveAngle = 0;

		/// <summary>
		/// Indicates is value of Bin negative
		/// </summary>
		public bool IsNegative
		{
			get => rotateTransform.Angle == RotationNegativeAngle;

			set
			{
				if (value)
				{
					rotateTransform.Angle = RotationNegativeAngle;
				}
				else
				{
					rotateTransform.Angle = RotationPositiveAngle;
				}
			}
		}

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

		//TODO: I can Light value either draw dotted line to this value or smth
		private void MainRect_MouseEnter(object sender, MouseEventArgs e)
		{

		}

		private void MainRect_MouseLeave(object sender, MouseEventArgs e)
		{

		}
	}
}
