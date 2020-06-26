using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Statistic.Resources.Templates
{
	public class SwitchButton : Control
	{
		public class SwitchButtonState : IEquatable<SwitchButtonState>
		{
			/// <summary>
			/// Right ball position
			/// </summary>
			public static SwitchButtonState Activated = new SwitchButtonState(true);

			/// <summary>
			/// Left ball position
			/// </summary>
			public static SwitchButtonState Deactivated = new SwitchButtonState(false);

			private bool isActive = false;

			private SwitchButtonState(bool isActive)
			{
				this.isActive = isActive;
			}

			public void Reverse()
			{
				isActive = !isActive;
			}

			public override bool Equals(object obj)
			{
				if (obj is SwitchButtonState buttonState)
					return Equals(buttonState);

				return false;
			}

			public bool Equals([AllowNull] SwitchButtonState other)
			{
				if (other is null)
					return false;

				return isActive == other.isActive;
			}

			public static implicit operator bool(SwitchButtonState buttonState)
			{
				return buttonState.isActive;
			}

			public static bool operator ==(SwitchButtonState left, SwitchButtonState right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(SwitchButtonState left, SwitchButtonState right)
			{
				return !(left == right);
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(isActive);
			}
		}

		public class OnOffButtonClickHandlerEventArgs
		{
			public SwitchButtonState State { get; }

			public OnOffButtonClickHandlerEventArgs(SwitchButtonState state)
			{
				State = state;
			}

		}

		public delegate void OnOffButtonClickHandler(object sender, OnOffButtonClickHandlerEventArgs eventArgs);
		public event OnOffButtonClickHandler Click;

		private SwitchButtonState buttonState = SwitchButtonState.Deactivated;

		public SwitchButtonState ButtonState
		{
			get => buttonState;
			set
			{
				buttonState = value;
				ApplyState();
			}
		}

		public Brush ActiveBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 31, 209, 161));   //#FF1FD1A1  


		public Brush InactiveBrush { get; set; } = new SolidColorBrush(Color.FromArgb(255, 36, 194, 224));  //#FF24C2E0

		private const double EllipseScale = 0.95;
		private Border outerBorder;
		private Ellipse ellipse;

		static SwitchButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchButton), new FrameworkPropertyMetadata(typeof(SwitchButton)));
		}

		public SwitchButton()
		{
			ApplyTemplate();
		}

		public override void OnApplyTemplate()
		{
			outerBorder = GetTemplateChild("outerBorder") as Border;
			outerBorder.CornerRadius = new CornerRadius(outerBorder.Height / 2);
			outerBorder.MouseDown += OuterBorder_MouseDown;

			ellipse = GetTemplateChild("innerEllipse") as Ellipse;
			ellipse.Height = EllipseScale * outerBorder.Height;
			ellipse.Width = EllipseScale * outerBorder.Height;
			ellipse.MouseDown += Ellipse_MouseDown;

			ToInactivePosition();
		}

		private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
		{
			ChangeState();
		}

		private void ToInactivePosition()
		{
			Background = InactiveBrush;
			ellipse.Margin = new Thickness(0, 0, outerBorder.Width / 2, 0);
		}

		private void ToActivePosition()
		{
			Background = ActiveBrush;
			ellipse.Margin = new Thickness(outerBorder.Width / 2, 0, 0, 0);
		}

		private void OuterBorder_MouseDown(object sender, MouseButtonEventArgs e)
		{
			ChangeState();
		}

		private void ApplyState()
		{
			if (ButtonState)
			{
				ToActivePosition();
			}
			else
			{
				ToInactivePosition();
			}
		}

		private void ChangeState()
		{
			ButtonState.Reverse();

			ApplyState();

			Click?.Invoke(this, new OnOffButtonClickHandlerEventArgs(ButtonState));
		}
	}
}
