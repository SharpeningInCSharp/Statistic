using DiagramsModel;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Model
{
	public partial class StatEnumItem
	{
		public StatEnumItem(string value)
		{
			if (DataValidation.NameValid(value) == false)
				throw new ArgumentException($"{nameof(value)} is invalid {value}");

			Item = value;
		}

		public override bool Equals(object obj)
		{
			if (obj is StatEnumItem statEnumItem)
				return Equals(statEnumItem);

			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Item);
		}

		public override string ToString()
		{
			return Item;
		}

		public static bool operator ==(StatEnumItem item1, StatEnumItem item2)
		{
			return item1.Equals(item2);
		}

		public static bool operator !=(StatEnumItem item1, StatEnumItem item2)
		{
			return !(item1 == item2);
		}
	}

	public partial class StatEnumItem : IEnumType, IEquatable<StatEnumItem>
	{
		public string Item { get; }

		public bool Equals([AllowNull] StatEnumItem other)
		{
			if (other is null)
				return false;

			return Item.Equals(other.Item);
		}
		
	}
}
