using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
	public partial class ValuesBunch
	{
		public DateTime Date { get; }

		public int Count => values.Count;

		private readonly List<ValueItem> values = new List<ValueItem>();

		public ValuesBunch(DateTime date, List<ValueItem> items)
		{
			Date = date;
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
