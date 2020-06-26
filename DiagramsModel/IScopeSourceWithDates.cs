using System;
using System.Collections.Generic;

namespace DiagramsModel
{
	/// <summary>
	/// Provides interface to be a source for <see cref="Scopes"/> for fixed date ranges
	/// </summary>
	public interface IScopeSourceWithDates : IEnumTypeSelectable, IDatesRange
	{
		/// <summary>
		/// Provides interface to get <see cref="IScopeSelectionItem"/> data at dates range from <paramref name="initialDate"/> to <paramref name="finalDate"/>
		/// </summary>
		/// <param name="enumType">Type to be selected</param>
		/// <param name="initialDate">Initial date of range</param>
		/// <param name="finalDate">Final date of range</param>
		/// <returns><see cref="IScopeSelectionItem"/> collection for curtain <paramref name="enumType"/></returns>
		IEnumerable<IScopeSelectionItem> GetData(IEnumType enumType, DateTime initialDate, DateTime? finalDate);
	}
}
