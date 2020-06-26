using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiagramsModel
{
	/// <summary>
	/// An abstraction to work with statistic group of items - <see cref="Scope"/>
	/// </summary>
	public partial class Scopes
	{
		public int Count => EnumValues.Count();

		public bool IsEmpty => TotalSum == 0;

		public IEnumerable<IEnumType> EnumValues { get; }

		public decimal TotalSum => scopes.Sum(x => x.Sum);

		public int NotEmptyScopesAmount => scopes.Count() - scopes.Count(x => x.Sum == 0);

		private readonly List<Scope> scopes = new List<Scope>();

		private Scopes(Func<IEnumerable<IEnumType>> typesProvider, DateTime initialDate, DateTime? finalDate)
		{
			if (typesProvider is null)
				throw new ArgumentNullException(nameof(typesProvider));

			InitialDate = initialDate;
			FinalDate = finalDate;

			EnumValues = (typesProvider.Invoke() ?? throw new ArgumentNullException($"{nameof(typesProvider)} can't return null.")).Distinct();
		}

		/// <summary>
		/// You should use that ctor to get data using date.
		/// </summary>
		/// <param name="typesProvider">Provides a way to get all range of types <see cref="IEnumType"/></param>
		/// <param name="dataProvider">Provides a way to get data according to curtain <see cref="IEnumType"/> and Dates range</param>
		/// <param name="initialDate">Start of date range</param>
		/// <param name="finalDate">End of date range</param>
		public Scopes(Func<IEnumerable<IEnumType>> typesProvider,
						Func<IEnumType, DateTime, DateTime?, IEnumerable<IScopeSelectionItem>> dataProvider,
						DateTime initialDate, DateTime? finalDate)
				: this(typesProvider, initialDate, finalDate)
		{
			if (dataProvider is null)
				throw new ArgumentNullException(nameof(dataProvider));

			Initialize(dataProvider);
		}

		/// <summary>
		/// According to <paramref name="dataProvider"/>, using that ctor you can get data without dates
		/// </summary>
		/// <param name="typesProvider">Provides a way to get all range of types <see cref="IEnumType"/></param>
		/// <param name="dataProvider">Provides a way to get data according to curtain <see cref="IEnumType"/> without any dates</param>
		/// <param name="initialDate">Initial date of the date range</param>
		/// <param name="finalDate">Final date of the date range (is null if equals <paramref name="initialDate"/>)</param>
		public Scopes(Func<IEnumerable<IEnumType>> typesProvider,
						Func<IEnumType, IEnumerable<IScopeSelectionItem>> dataProvider,
						DateTime initialDate, DateTime? finalDate)
				: this(typesProvider, initialDate, finalDate)
		{
			if (dataProvider is null)
				throw new ArgumentNullException(nameof(dataProvider));

			Initialize(dataProvider);
		}

		/// <summary>
		/// Creates object using <paramref name="source"/>
		/// </summary>
		/// <param name="source">Object with implemented <see cref="IScopeSource"/></param>
		public Scopes(IScopeSource source) : this(source.GetTypes, source.GetData, source.InitialDate, source.FinalDate)
		{ }

		private void Initialize(Func<IEnumType, IEnumerable<IScopeSelectionItem>> dataProvider)
		{
			foreach (var value in EnumValues)
			{
				var result = dataProvider.Invoke(value);

				OnScopeAddition(result);
			}

			if (IsEmpty == false)
				SetPerCents();
		}

		private void Initialize(Func<IEnumType, DateTime, DateTime?, IEnumerable<IScopeSelectionItem>> dataProvider)
		{
			foreach (var value in EnumValues)
			{
				var result = dataProvider.Invoke(value, InitialDate, FinalDate);

				OnScopeAddition(result);
			}

			if (IsEmpty == false)
				SetPerCents();
		}

		private void OnScopeAddition(IEnumerable<IScopeSelectionItem> result)
		{
			if (result != null)
			{
				InitScope(result, out Scope scope);

				scopes.Add(scope);
			}
		}

		private void InitScope(IEnumerable<IScopeSelectionItem> result, out Scope scope)
		{
			if (FinalDate.HasValue)
			{
				scope = new Scope(result, InitialDate, FinalDate.Value);
			}
			else
			{
				scope = new Scope(result, InitialDate);
			}
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

	public partial class Scopes : IEnumerable<Scope>, IEnumerable, IPairOutputStringData, IDatesRange
	{
		public DateTime InitialDate { get; }

		public DateTime? FinalDate { get; }

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
