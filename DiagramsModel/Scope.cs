using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagramsModel
{
	public partial class Scope<EType, DType>
				where EType : IEnumType
				where DType : IScopeSelectionItem
	{
		public decimal Sum => Items.Sum(x => x.GetTotal);

		public decimal Ratio { get; internal set; } = 0;

		public EType EnumMember { get; internal set; }

		private IEnumerable<DType> Items { get; }

		public DateTime InitialDate { get; }

		public DateTime? FinalDate { get; }

		internal Scope(IEnumerable<DType> items, DateTime dateTime)
		{
			Items = items;
			InitialDate = dateTime;
		}

		internal Scope(IEnumerable<DType> items, DateTime initialDate, DateTime finalDate) : this(items, initialDate)
		{
			FinalDate = finalDate;
		}

		internal IEnumerable<DType> GetTopExpensive()
		{
			return Items.OrderByDescending(x => x.GetTotal).Take(3);
		}
	}

	public partial class Scope<EType, DType> : IMaxMinDiagramStat
	{
		public decimal Min => Items.Min(x=>x.GetTotal);

		public decimal Max => Items.Max(x => x.GetTotal);

		public override string ToString()
		{
			return $"{EnumMember} {Sum:C2} ({Ratio:P2})";
		}

		///// <summary>
		///// Using Handler output line by line items
		///// </summary>
		///// <param name="OutputHandler"></param>
		//public void OutputData(Action<string, string> OutputHandler)
		//{
		//	foreach (var item in Items)
		//	{
		//		OutputHandler?.Invoke(item.ToString(), item.GetTotal.ToString("C2"/*,CultureInfo.CreateSpecificCulture()*/));
		//	}
		//}
	}
}
