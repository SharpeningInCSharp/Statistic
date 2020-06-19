using DiagramsModel;
using System;
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
		public Brush SelectionBrush { get; set; } = Brushes.LightGray;

		public delegate void BinMouseEventHandler(Bin sender);
		public event BinMouseEventHandler MouseOn;
		public event BinMouseEventHandler MouseOut;

		private const int RotationNegativeAngle = 180;
		private const int RotationPositiveAngle = 0;

		private const byte defaultRectThickness = 1;
		private const byte activatedRectThickness = 2;

		/// <summary>
		/// Width of item
		/// </summary>
		public int BinsWidth => 20 + 2 * defaultRectThickness;

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

		private Brush color;
		/// <summary>
		/// Current bin's color
		/// </summary>
		public Brush Color
		{
			get => color;

			set
			{
				color = value;
				MainRect.Fill = color;
			}
		}

		internal Scope Scope { get; }

		public int Ind { get; private set; }

		internal Bin(Scope scope, int num, double binHeight)
		{
			Scope = scope ?? throw new ArgumentNullException(nameof(scope));

			InitializeComponent();

			MainRect.Height = binHeight;
			Ind = num;
		}

		/// <summary>
		/// Shifts object right for <paramref name="shiftValue"/>
		/// </summary>
		/// <param name="shiftValue">value to be shifted</param>
		public void Shift()
		{
			Margin = new Thickness(BinsWidth * (Ind + 1), 0, 0, 0);
		}

		public void Select()
		{
			MainRect.StrokeThickness = activatedRectThickness;
			MainRect.Fill = SelectionBrush;
		}

		public void Unselect()
		{
			MainRect.StrokeThickness = defaultRectThickness;
			MainRect.Fill = color;
		}

		//TODO: I can Light value either draw dotted line to this value or smth
		private void MainRect_MouseEnter(object sender, MouseEventArgs e)
		{
			Select();
			MouseOn?.Invoke(this);
		}

		private void MainRect_MouseLeave(object sender, MouseEventArgs e)
		{
			Unselect();
			MouseOut?.Invoke(this);
		}
	}
}
