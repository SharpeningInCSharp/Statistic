﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramsDataOutput
{
	/// <summary>
	/// Логика взаимодействия для PieLegendItem.xaml
	/// </summary>
	public partial class PieLegendItem : UserControl
	{
		public Brush InitialBrush { get; }

		private double InitialFontSize { get; }
		private double InitialBorderThickness { get; }
		private double InitialSideSize { get; }

		private const double SelectedFontSize = 20;
		private const int SelectedBorderThickness = 1;

		public delegate void LegendHandler(int num);
		public event LegendHandler MouseOn;
		public event LegendHandler MouseOut;

		public int Ind { get; }

		public PieLegendItem(int i, Brush color, string title)
		{
			InitializeComponent();

			InitialFontSize = ItemName.FontSize;
			InitialBorderThickness = ItemColor.BorderThickness.Left;
			InitialSideSize = ItemColor.Height;

			ItemColor.Background = InitialBrush = color;
			ItemName.Text = title;
			Ind = i;
		}

		private void Item_MouseEnter(object sender, MouseEventArgs e)
		{
			ToSelectedView();
			MouseOn?.Invoke(Ind);
		}

		private void ToSelectedView()
		{
			ItemColor.BorderThickness = new Thickness(SelectedBorderThickness);
			ItemColor.Height = ItemColor.MaxHeight;
			ItemColor.Width = ItemColor.MaxWidth;
			ItemName.FontSize = SelectedFontSize;
		}

		private void Item_MouseLeave(object sender, MouseEventArgs e)
		{
			ToUnselectedView();
			MouseOut?.Invoke(Ind);
		}

		private void ToUnselectedView()
		{
			ItemColor.BorderThickness = new Thickness(InitialBorderThickness);
			ItemColor.Height = InitialSideSize;
			ItemColor.Width = InitialSideSize;
			ItemName.FontSize = InitialFontSize;
		}
	}
}
