using System;

namespace DiagramsModel
{
	/// <summary>
	/// Provides interface to get some string data in 2 values
	/// </summary>
	public interface IPairOutputStringData
	{
		void OutputData(Action<string, string> OutputHandler);
	}
}
