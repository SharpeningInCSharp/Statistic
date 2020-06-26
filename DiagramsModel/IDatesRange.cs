using System;

namespace DiagramsModel
{
	/// <summary>
	/// Provides interface for dates range
	/// </summary>
	public interface IDatesRange
	{
		/// <summary>
		/// Initial date of range
		/// </summary>
		DateTime InitialDate { get; }

		/// <summary>
		/// Final date of range
		/// Is null if equals to InitialDate
		/// </summary>
		DateTime? FinalDate { get; }
	}
}
