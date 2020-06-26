using System.Collections.Generic;

namespace DiagramsModel
{
	/// <summary>
	/// Provides interface to be a source for <see cref="Scopes"/> for curtain dates range
	/// </summary>
	public interface IScopeSource : IEnumTypeSelectable, IDatesRange
	{
		/// <summary>
		/// Provides interface to get <see cref="IScopeSelectionItem"/> data
		/// </summary>
		/// <param name="enumType">Type to be selected</param>
		/// <returns><see cref="IScopeSelectionItem"/> collection for curtain <paramref name="enumType"/></returns>
		IEnumerable<IScopeSelectionItem> GetData(IEnumType enumType);
	}
}
