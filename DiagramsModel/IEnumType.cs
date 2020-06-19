using System;

namespace DiagramsModel
{
	public interface IEnumType : IEquatable<IEnumType>
	{
		string Item { get; }
	}
}
