using System;

namespace DiagramsModel
{
	/// <summary>
	/// Provides interface to ger enumeration item
	/// </summary>
	public interface IEnumType : IEquatable<IEnumType>
	{
		/// <summary>
		/// Item's name
		/// </summary>
		string Item { get; }
	}
}
