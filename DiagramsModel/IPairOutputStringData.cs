using System;

namespace DiagramsModel
{
	public interface IPairOutputStringData
	{
		void OutputData(Action<string, string> OutputHandler);
	}
}
