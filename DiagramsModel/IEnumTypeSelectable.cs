using System.Collections.Generic;

namespace DiagramsModel
{
	/// <summary>
	/// Provides interface to receive types <see cref="IEnumType"/> for statistic
	/// </summary>
	public interface IEnumTypeSelectable
	{
		IEnumerable<IEnumType> GetTypes();
	}
}
