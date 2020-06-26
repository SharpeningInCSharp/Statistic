using System.Collections.Generic;

namespace DiagramsModel
{
	/// <summary>
	/// Provides interface to receive types <see cref="IEnumType"/> for statistic
	/// </summary>
	public interface IEnumTypeSelectable
	{
		/// <summary>
		/// Unique sequence of <see cref="IEnumType"/>
		/// </summary>
		/// <returns></returns>
		IEnumerable<IEnumType> GetTypes();
	}
}
