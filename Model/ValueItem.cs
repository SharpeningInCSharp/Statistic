﻿using DiagramsModel;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Model
{
	public partial class ValueItem
	{
		public ValueItem(decimal value, StatEnumItem statEnum)
		{
			if (DataValidation.IsValueValid(value) == false)
				throw new ArgumentException($"{nameof(value)} is invalid {value}. Probably it's too big");

			if (statEnum is null)
				throw new ArgumentNullException($"{nameof(statEnum)} can't be null!");

			EnumType = statEnum;
			GetTotal = value;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(GetTotal);
		}

		public override bool Equals(object obj)
		{
			if (obj is ValueItem valueItem)
				return Equals(valueItem);

			return false;
		}

		public override string ToString()
		{
			return $"{EnumType} {GetTotal:0#.##}";
		}

		public static bool operator ==(ValueItem item1, ValueItem item2)
		{
			return item1.Equals(item2);
		}

		public static bool operator !=(ValueItem item1, ValueItem item2)
		{
			return !(item1 == item2);
		}
	}

	public partial class ValueItem : IScopeSelectionItem, IEquatable<ValueItem>
	{
		public decimal GetTotal { get; }

		public IEnumType EnumType { get; }

		public bool Equals([AllowNull] ValueItem other)
		{
			if (other is null)
				return false;

			return GetTotal == other.GetTotal && EnumType.Equals(other.EnumType);
		}
	}
}
