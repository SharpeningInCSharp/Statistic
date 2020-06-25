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
		public DateTime InitialDate { get; }

		public DateTime? FinalDate { get; }

		public int Count => values.Count;

		private readonly List<ValueItem> values = new List<ValueItem>();

		/// <summary>
		/// Creates <see cref="ValuesBunch"/> object from <paramref name="items"/> for single <paramref name="date"/>
		/// </summary>
		/// <param name="date"></param>
		/// <param name="items"></param>
		public ValuesBunch(DateTime date, List<ValueItem> items)
		{
			InitialDate = date;
			FinalDate = null;
			values = items ?? throw new ArgumentNullException(nameof(items));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="initialDate"></param>
		/// <param name="finalDate"></param>
		/// <param name="items"></param>
		public ValuesBunch(DateTime initialDate, DateTime finalDate, List<ValueItem> items)
		{
			InitialDate = initialDate;
			FinalDate = finalDate;
			values = items ?? throw new ArgumentNullException(nameof(items));
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
