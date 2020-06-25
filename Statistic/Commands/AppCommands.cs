using System.Windows.Input;

namespace Statistic.Commands
{
	class AppCommands
	{
		static AppCommands()
		{
			ShowInfo = new RoutedCommand("ShowInfo", typeof(MainWindow));
		}

		public static RoutedCommand ShowInfo { get; set; }
	}
}
