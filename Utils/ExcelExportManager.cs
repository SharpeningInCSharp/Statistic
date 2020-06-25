using ClosedXML.Excel;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
	public class ExcelExportManager
	{
		public static async Task<IEnumerable<ValuesBunch>> ParseFileAsync(string path)
		{
			return await Task.Run(() => ParseFile(path));
		}

		private static IEnumerable<ValuesBunch> ParseFile(string path)
		{
			var items = new List<ValuesBunch>();

			var wb = new XLWorkbook(path);
			foreach (var sheet in wb.Worksheets)
			{
				items.AddRange(ReadWorkSheet(sheet));
			}

			return items;
		}

		private static IEnumerable<ValuesBunch> ReadWorkSheet(IXLWorksheet worksheet)
		{
			var firstRow = worksheet.FirstRowUsed();
			var firstColumn = worksheet.FirstColumnUsed();

			var lastRow = worksheet.LastRowUsed();
			var lastColumn = worksheet.LastColumnUsed();
 


			return null;
		}
	}
}
