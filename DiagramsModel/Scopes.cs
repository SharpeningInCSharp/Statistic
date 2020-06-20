using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiagramsModel
{
	public partial class Scopes
	{
		public int Count => EnumValues.Count();
		public bool IsEmpty => TotalSum == 0;
		public DateTime InitialDate { get; }
		public DateTime? FinalDate { get; }
		public IEnumerable<IEnumType> EnumValues { get; }
		public decimal TotalSum => scopes.Sum(x => x.Sum);
		public int NotEmptyScopesAmount => scopes.Count() - scopes.Count(x => x.Sum == 0);

		private readonly List<Scope> scopes = new List<Scope>();

		/// <summary>
		/// Scope for range of dates
		/// </summary>
		/// <param name="dataProvider">Provides a way to get data according to curtain IEnumType and Dates range</param>
		/// <param name="initialDate">Start of Date range</param>
		/// <param name="finalDate">End of Date range</param>
		public Scopes(Func<IEnumerable<IEnumType>> typesProvider, Func<IEnumType, DateTime, DateTime?, IEnumerable<IScopeSelectionItem>> dataProvider, DateTime initialDate, DateTime? finalDate)
		{
			InitialDate = initialDate;
			FinalDate = finalDate;

			EnumValues = typesProvider.Invoke();
			Initialize(dataProvider);
		}

		private void Initialize(Func<IEnumType, DateTime, DateTime?, IEnumerable<IScopeSelectionItem>> dataProvider)
		{
			foreach (var value in EnumValues)
			{
				var result = dataProvider.Invoke(value, InitialDate, FinalDate);
				Scope scope;
				if (FinalDate.HasValue)
				{
					scope = new Scope(result, InitialDate, FinalDate.Value);
				}
				else
				{
					scope = new Scope(result, InitialDate);
				}

				scope.EnumMember = value;
				scopes.Add(scope);
			}

			if (IsEmpty == false)
				SetPerCents();
		}

		private void SetPerCents()
		{
			foreach (var item in scopes)
			{
				item.Ratio = (double)(item.Sum / TotalSum);
			}
		}

		public string DatesToString()
		{
			if (FinalDate.HasValue)
			{
				return $"{InitialDate.ToShortDateString()}-{FinalDate.Value.ToShortDateString()}";
			}
			else
			{
				return InitialDate.ToShortDateString();
			}
		}
	}

	public partial class Scopes: IEnumerable<Scope>, IEnumerable, IPairOutputStringData
	{
		IEnumerator IEnumerable.GetEnumerator()
		{
			return scopes.GetEnumerator();
		}

		public Scope this[int ind]
		{
			get
			{
				if (ind < 0 || ind >= scopes.Count)
					throw new ArgumentOutOfRangeException("Index was out of range");

				return scopes[ind];
			}
		}

		public Scope this[IEnumType typeName]
		{
			get
			{
				return scopes.FirstOrDefault(x => x.EnumMember.Equals(typeName));
			}
		}

		public IEnumerator<Scope> GetEnumerator()
		{
			return scopes.GetEnumerator();
		}

		/// <summary>
		/// Returns top-3 the most expensive items in each category
		/// </summary>
		/// <param name="OutputHandler">Handler for output</param>
		public void OutputData(Action<string, string> OutputHandler)
		{
			var categories = scopes.Select(x => x.GetTopExpensive());
			foreach (var category in categories)
			{
				foreach (var item in category)
				{
					OutputHandler?.Invoke(item.ToString(), item.GetTotal.ToString("C2"));
				}
			}
		}
	}
}
