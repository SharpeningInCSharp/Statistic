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
		public delegate void ExcelExportDelegateHandler(ExcelExportManager exportManager, IEnumerable<ValuesBunch> items);
		public static event ExcelExportDelegateHandler ParsingComplete;

		public static async void ParseFileAsync(string path)
		{
			await Task.Run(() => ParseFile(path));
		}

		private static void ParseFile(string path)
		{
			var items = new List<ValuesBunch>();

			try
			{
				using var wb = new XLWorkbook(path);
				foreach (var sheet in wb.Worksheets)
				{
					items.AddRange(ReadWorkSheet(sheet));
				}
			}
			catch
			{ }

			ParsingComplete?.Invoke(null, items);
		}

		private static IEnumerable<ValuesBunch> ReadWorkSheet(IXLWorksheet worksheet)
		{
			var items = new List<ValuesBunch>();

			var firstRow = worksheet.FirstRowUsed();
			var lastRow = worksheet.LastRowUsed();

			IXLRow currentRow = firstRow;
			ValuesBunch valueItems = null;

			while (currentRow.RowNumber() <= lastRow.RowNumber())
			{
				var currentRowFirstCell = currentRow.FirstCellUsed();
				var correntRowLastCell = currentRow.LastCellUsed();

				if (DateTime.TryParse(currentRowFirstCell?.CachedValue.ToString(), out var initialDate) && initialDate < DateTime.Now)
				{
					InitializeFinalDate(currentRowFirstCell, correntRowLastCell, initialDate, out var finalDate);

					if (valueItems != null && valueItems.Count > 0)
						items.Add(valueItems);

					if (finalDate.HasValue)
						valueItems = new ValuesBunch(initialDate, finalDate.Value);
					else
						valueItems = new ValuesBunch(initialDate);
				}
				else if (valueItems != null && currentRow != null && currentRow.CellsUsed().Count() > 0)
				{
					var currentTypeCell = currentRow.FirstCellUsed();
					var rowDelta = ParseValueItems(valueItems, currentTypeCell);

					if (valueItems != null && valueItems.Count > 0)
					{
						items.Add(valueItems);
						valueItems = null;
					}

					currentRow = currentRow.RowBelow(rowDelta - currentRow.RowNumber());
				}

				currentRow = currentRow.RowBelow();
			}

			if (valueItems != null && valueItems.Count > 0)
				items.Add(valueItems);

			return items;
		}

		private static void InitializeFinalDate(IXLCell firstCell, IXLCell lastCell, DateTime initialDate, out DateTime? finalDate)
		{
			if (lastCell.CachedValue.Equals(firstCell.CachedValue))
			{
				finalDate = null;
			}
			else
			{
				if (DateTime.TryParse(lastCell.CachedValue.ToString(), out var date) && date > initialDate)
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
				var type = currentTypeCell.CachedValue.ToString();
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
				if (DateTime.TryParse(currentValueCell.CachedValue.ToString(), out var d))
					return;

				if (decimal.TryParse(currentValueCell.CachedValue?.ToString(), out var num))
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
