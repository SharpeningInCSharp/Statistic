using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagramsModel
{
	/// <summary>
	/// An abstraction of statistic item
	/// </summary>
	public partial class Scope
	{
		public decimal Sum => Items.Sum(x => x.GetTotal);

		public double Ratio { get; internal set; } = 0;

		public IEnumType EnumMember { get; }

		private IEnumerable<IScopeSelectionItem> Items { get; }

		public DateTime InitialDate { get; }

		public DateTime? FinalDate { get; }

		internal Scope(IEnumerable<IScopeSelectionItem> items, DateTime dateTime)
		{
			if (items is null)
				throw new ArgumentNullException(nameof(items));

			if (items.Count() < 1)
				throw new ArgumentException($"{nameof(items)} collection can't be empty.");

			if (items.Select(x => x.EnumType).Distinct().Count() > 1)
				throw new ArgumentException($"{nameof(items)} collection must contain the same {typeof(IEnumType)} for each item.");

			EnumMember = items.First().EnumType;
			Items = items;
			InitialDate = dateTime;
		}

		internal Scope(IEnumerable<IScopeSelectionItem> items, DateTime initialDate, DateTime finalDate) : this(items, initialDate)
		{
			FinalDate = finalDate;
		}

		internal IEnumerable<IScopeSelectionItem> GetTopExpensive()
		{
			return Items.OrderByDescending(x => x.GetTotal).Take(3);
		}
	}

	public partial class Scope : IMaxMinDiagramStat, IPairOutputStringData
	{
		public decimal Min => Items.Min(x => x.GetTotal);

		public decimal Max => Items.Max(x => x.GetTotal);

		public override string ToString()
		{
			return $"{EnumMember} {Sum:C2} ({Ratio:P2})";
		}

		/// <summary>
		/// Using Handler output line by line items
		/// </summary>
		/// <param name="OutputHandler"></param>
		public void OutputData(Action<string, string> OutputHandler)
		{
			foreach (var item in Items)
			{
				OutputHandler?.Invoke(item.ToString(), item.GetTotal.ToString("C2"/*,CultureInfo.CreateSpecificCulture()*/));
			}
		}
	}
}
