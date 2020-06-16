﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DiagramsModel
{
	public partial class Scopes<EType, DType>
						where EType : IEnumType
						where DType : IScopeSelectionItem
	{
		public bool IsEmpty => TotalSum == 0;
		public DateTime InitialDate { get; }
		public DateTime? FinalDate { get; }
		public IEnumerable<EType> EnumValues { get; }
		public decimal TotalSum => scopes.Sum(x => x.Sum);
		public int NotEmptyScopesAmount => scopes.Count() - scopes.Count(x => x.Sum == 0);

		private readonly List<Scope<EType, DType>> scopes = new List<Scope<EType, DType>>();

		/// <summary>
		/// Scope for range of dates
		/// </summary>
		/// <param name="dataProvider">Provides a way to get data according to curtain IEnumType and Dates range</param>
		/// <param name="initialDate">Start of Date range</param>
		/// <param name="finalDate">End of Date range</param>
		public Scopes(Func<IEnumerable<EType>> typesProvider, Func<EType, DateTime, DateTime?, IEnumerable<DType>> dataProvider, DateTime initialDate, DateTime? finalDate)
		{
			InitialDate = initialDate;
			FinalDate = finalDate;

			EnumValues = typesProvider.Invoke();
			Initialize(dataProvider);
		}

		private void Initialize(Func<EType, DateTime, DateTime?, IEnumerable<DType>> dataProvider)
		{
			foreach (var value in EnumValues)
			{
				var result = dataProvider.Invoke(value, InitialDate, FinalDate);
				Scope<EType, DType> scope;
				if (FinalDate.HasValue)
				{
					scope = new Scope<EType, DType>(result, InitialDate, FinalDate.Value);
				}
				else
				{
					scope = new Scope<EType, DType>(result, InitialDate);
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
				item.Ratio = item.Sum / TotalSum;
			}
		}
	}

	public partial class Scopes<EType, DType> : IEnumerable<Scope<EType, DType>>, IEnumerable
	{
		IEnumerator IEnumerable.GetEnumerator()
		{
			return scopes.GetEnumerator();
		}

		public Scope<EType, DType> this[int ind]
		{
			get
			{
				if (ind < 0 || ind >= scopes.Count)
					throw new ArgumentOutOfRangeException("Index was out of range");

				return scopes[ind];
			}
		}

		public Scope<EType, DType> this[EType typeName]
		{
			get
			{
				return scopes.FirstOrDefault(x => x.EnumMember.Equals(typeName));
			}
		}

		public IEnumerator<Scope<EType, DType>> GetEnumerator()
		{
			return scopes.GetEnumerator();
		}

		///// <summary>
		///// Returns top-3 the most expensive items in each category
		///// </summary>
		///// <param name="OutputHandler">Handler for output</param>
		//public void OutputData(Action<string, string> OutputHandler)
		//{
		//	var categories = scopes.Select(x => x.GetTopExpensive());
		//	foreach (var category in categories)
		//	{
		//		foreach (var item in category)
		//		{
		//			OutputHandler?.Invoke(item.ToString(), item.GetTotal.ToString("C2"));
		//		}
		//	}
		//}
	}
}
