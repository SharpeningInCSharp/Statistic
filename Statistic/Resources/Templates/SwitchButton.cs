using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Statistic.Resources.Templates
{
	public class SwitchButton : Control
	{
		public class OnOffButtonClickHandlerEventArgs
		{
			public bool State { get; }

			public OnOffButtonClickHandlerEventArgs(bool state)
			{
				State = state;
			}

		}

		public delegate void OnOffButtonClickHandler(object sender, OnOffButtonClickHandlerEventArgs eventArgs);
		public event OnOffButtonClickHandler Click;


		public bool IsActivated { get; private set; } = false;

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

		private void ChangeState()
		{
			IsActivated = !IsActivated;

			if (IsActivated)
			{
				ToActivePosition();
			}
			else
			{
				ToInactivePosition();
			}

			Click?.Invoke(this, new OnOffButtonClickHandlerEventArgs(IsActivated));
		}
	}
}
