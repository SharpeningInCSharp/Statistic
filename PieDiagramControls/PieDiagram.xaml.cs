using DiagramsDataOutput;
using DiagramsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PieDiagramControls
{
	/// <summary>
	/// Логика взаимодействия для PieDiagram.xaml
	/// </summary>
	public partial class PieDiagram : UserControl
	{
		//SOLVE: color generation
		public DateTime Initial { get; }
		public DateTime? Final { get; }

		private readonly List<PiePiece> piePieces = new List<PiePiece>();
		private const int FullAngle = 360;

		public Scopes Scopes { get; private set; }
		public Brush[] UsersBrushes { get; }

		public PieDiagram(Scopes scopes, Brush[] brushes)
		{
			if (scopes is null)
				throw new ArgumentNullException($"{nameof(scopes)} was null!");

			InitializeComponent();
			UsersBrushes = brushes;
			Scopes = scopes;

			if (Scopes.IsEmpty)
			{
				//MessageBox.Show("There's no data for this period");
				return;
			}

			InitializeLegend();
			InitializePiePieces();
			ShowGeneralInfo();
		}

		public void LoadNew(Scopes scopes)
		{
			ClearPie();

			if (scopes is null)
				throw new ArgumentNullException($"{nameof(scopes)} was null!");

			Scopes = scopes;

			if (Scopes.IsEmpty)
			{
				MessageBox.Show("There's no data for this period");
				return;
			}

			InitializeLegend();
			InitializePiePieces();
			ShowGeneralInfo();
		}

		private void ClearPie()
		{
			piePieces.Clear();
			PiecesGrid.Children.Clear();
			DiagramInfo.Clear();
		}

		private void InitializePiePieces()
		{
			var generalVol = Scopes.TotalSum;
			var genAngle = 0.0;

			for (int i = 0; i < Scopes.Count(); i++)
			{
				if (Scopes[i].Sum != 0)
				{
					var angle = Convert.ToDouble((Scopes[i].Sum * FullAngle) / generalVol);
					var piePiece = new PiePiece(Scopes[i].EnumMember, angle, UsersBrushes[i]);
					piePiece.MouseIn += PiePiece_MouseIn;
					piePiece.MouseOut += PiePiece_MouseOut;
					piePiece.Rotate(genAngle);
					genAngle += angle;
					piePieces.Add(piePiece);
					PiecesGrid.Children.Add(piePiece);
				}
			}
		}

		private const int ElementToForegroundIndex = 10;
		private const int ElementToBachgroundIndex = 0;

		private void PiePiece_MouseOut(PiePiece sender)
		{
			Panel.SetZIndex(sender, ElementToBachgroundIndex);
			ShowGeneralInfo();
		}

		private void ShowGeneralInfo()
		{
			piePieceHeaderTextBlock.Text = "General info";

			//TODO: edit DiagramInfo interraction - set new Header FontSize and remove piePieceHeaderTextBlock also udopt to new output view (like as HistoDiagram)
			DiagramInfo.Clear();
			DiagramInfo.Header = Scopes.TotalSum.ToString("C2");
			Scopes.OutputData((col1, col2) => DiagramInfo.Add(col1, col2));
		}

		private void PiePiece_MouseIn(PiePiece sender)
		{
			Panel.SetZIndex(sender, ElementToForegroundIndex);
			var curScope = Scopes[sender.EnumType];
			piePieceHeaderTextBlock.Text = $"{curScope.EnumMember}";

			DiagramInfo.Clear();
			DiagramInfo.Header = $"{curScope.Sum:C2} ({curScope.Ratio: #0.##%})";
			curScope.OutputData((col1, col2) => DiagramInfo.Add(col1, col2));
		}

		//TODO: mb solve bug with 100% piePiece initialization
		private void InitializeLegend()
		{
			legend.Children.Clear();

			int amount = 0;
			for (int i = 0; i < Scopes.Count(); i++)
			{
				if (Scopes[i].Sum != 0)                      //Initialize LegendItems only for not empty Pies
				{
					var legendItem = new PieLegendItem(Scopes[i].EnumMember, UsersBrushes[amount]);
					amount++;
					legendItem.MouseOn += LegendItem_MouseOn;
					legendItem.MouseOut += LegendItem_MouseOut;
					legend.Children.Add(legendItem);
				}
			}
		}

		private void LegendItem_MouseOut(IEnumType enumType)
		{
			piePieces.FirstOrDefault(x => x.EnumType.Equals(enumType))
					?.Unselect();
		}

		private void LegendItem_MouseOn(IEnumType enumType)
		{
			piePieces.FirstOrDefault(x => x.EnumType.Equals(enumType))
					?.Select();
		}
	}
}
