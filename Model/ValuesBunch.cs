using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
	/// <summary>
	/// Abstraction for items range for curtain period
	/// </summary>
	public partial class ValuesBunch
	{
		/// <summary>
		/// Initial date of range
		/// </summary>
		public DateTime InitialDate { get; }

		/// <summary>
		/// Final date of range. Is null if range constist from a single date
		/// </summary>
		public DateTime? FinalDate { get; }

		/// <summary>
		/// Amount of statistic items
		/// </summary>
		public int Count => values.Count;

		private readonly List<ValueItem> values = new List<ValueItem>();

		/// <summary>
		/// Creates <see cref="ValuesBunch"/> object for single <paramref name="date"/>
		/// </summary>
		/// <param name="date">Date of statistic selection</param>
		public ValuesBunch(DateTime date)
		{
			InitialDate = date;
			FinalDate = null;
		}

		/// <summary>
		/// Creates <see cref="ValuesBunch"/> object for dates range from <paramref name="initialDate"/> to <paramref name="finalDate"/>
		/// </summary>
		/// <param name="initialDate">The first date in range</param>
		/// <param name="finalDate">The last one date in range</param>
		public ValuesBunch(DateTime initialDate, DateTime finalDate)
		{
			InitialDate = initialDate;
			FinalDate = finalDate;
		}

		public void Add(ValueItem item)
		{
			if (item != null)
				values.Add(item);
		}

	}

	//TODO: should I add one more alternative ctor to Scopes, which takes some interfaced item
	public partial class ValuesBunch : IEnumerable<ValueItem>
	{
		public ValueItem this[int ind]
		{
			get
			{
				if (ind >= 0 && ind < values.Count)
					return values[ind];

				throw new ArgumentOutOfRangeException();
			}
		}

		public IEnumerator<ValueItem> GetEnumerator()
		{
			return values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return values.GetEnumerator();
		}
	}
}
