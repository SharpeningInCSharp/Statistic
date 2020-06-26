using System.Windows;
using System.Windows.Input;

namespace Statistic.Commands
{
	class AppCommands
	{
		static AppCommands()
		{
			ShowInfo = new RoutedCommand("ShowInfo", typeof(MainWindow));
			OpenSample = new RoutedCommand("OpenSample", typeof(Window));
		}

		public static RoutedCommand ShowInfo { get; set; }

		public static RoutedCommand OpenSample { get; set; }
	}
}
