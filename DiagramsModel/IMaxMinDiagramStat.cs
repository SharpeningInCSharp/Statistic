namespace DiagramsModel
{
	/// <summary>
	/// Provides interface to get some more statistic info (Min and Max values)
	/// </summary>
	interface IMaxMinDiagramStat
	{
		decimal Min { get; }
		decimal Max { get; }
	}
}
