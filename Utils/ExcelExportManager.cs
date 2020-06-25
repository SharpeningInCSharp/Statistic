using ClosedXML.Excel;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

			using var wb = new XLWorkbook(path);
			foreach (var sheet in wb.Worksheets)
			{
				items.AddRange(ReadWorkSheet(sheet));
			}

			return items;
		}

		private static IEnumerable<ValuesBunch> ReadWorkSheet(IXLWorksheet worksheet)
		{
			var items = new List<ValuesBunch>();

			var firstRow = worksheet.FirstRowUsed();
			var lastRow = worksheet.LastRowUsed();

			IXLRow currentRow = firstRow;
			ValuesBunch valueItems = null;

			while (currentRow != lastRow)
			{
				var currentRowFirstCell = currentRow.FirstCellUsed();
				var correntRowLastCell = currentRow.LastCellUsed();

				if (currentRowFirstCell?.Value is DateTime initialDate)
				{
					InitializeFinalDate(currentRowFirstCell, correntRowLastCell, initialDate, out var finalDate);

					if (valueItems != null)
						items.Add(valueItems);

					if (finalDate.HasValue)
						valueItems = new ValuesBunch(initialDate, finalDate.Value);
					else
						valueItems = new ValuesBunch(initialDate);
				}
				else if (valueItems != null && currentRow != null && currentRow.CellsUsed().Count() > 0)
				{
					var currentTypeCell = currentRow.FirstCellUsed();
					var dRow = ParseValueItems(valueItems, currentTypeCell);		//TODO: use that
				}

				currentRow = currentRow.RowBelow();
			}

			return items;
		}

		private static void InitializeFinalDate(IXLCell firstCell, IXLCell lastCell, DateTime initialDate, out DateTime? finalDate)
		{
			if (lastCell.Value.Equals(firstCell.Value))
			{
				finalDate = null;
			}
			else
			{
				if (lastCell.Value is DateTime date && date > initialDate)
					finalDate = date;
				else
					finalDate = null;
			}
		}

		private static int ParseValueItems(ValuesBunch valueItems, IXLCell currentTypeCell)
		{
			int lastRowNum = currentTypeCell.Address.RowNumber;
			while (currentTypeCell.IsEmpty() == false)
			{
				var type = currentTypeCell.Value.ToString();
				if (DataValidation.IsNameValid(type))
				{
					ParseValueItem(valueItems, type, currentTypeCell.CellBelow(), ref lastRowNum);
				}

				currentTypeCell = currentTypeCell.CellRight();
			}

			return lastRowNum;
		}

		private static void ParseValueItem(ValuesBunch valueItems, string type, IXLCell currentValueCell, ref int lastRowNum)
		{
			while (currentValueCell.IsEmpty() == false)
			{
				if (currentValueCell.Value is DateTime)
					return;

				if (decimal.TryParse(currentValueCell.Value?.ToString(), out var num))
				{
					try
					{
						var value = new ValueItem(num, new StatEnumItem(type));
						valueItems.Add(value);
					}
					catch
					{
						continue;
					}
				}

				lastRowNum = Math.Max(lastRowNum, currentValueCell.Address.RowNumber);
				currentValueCell = currentValueCell.CellBelow();
			}
		}
	}
}
