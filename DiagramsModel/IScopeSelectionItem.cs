namespace DiagramsModel
{
	/// <summary>
	/// Provides interface for statistic value item with curtain <see cref="IEnumType"/>
	/// </summary>
	public interface IScopeSelectionItem
	{
		/// <summary>
		/// Item's value
		/// </summary>
		decimal GetTotal { get; }

		/// <summary>
		/// Item's type
		/// </summary>
		IEnumType EnumType { get; }
	}
}
