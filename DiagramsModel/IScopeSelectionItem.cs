namespace DiagramsModel
{
	public interface IScopeSelectionItem
	{
		decimal GetTotal { get; }

		IEnumType EnumType { get; }
	}
}
