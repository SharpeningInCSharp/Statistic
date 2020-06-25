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
					//here must be new method
					var currentTypeCell = currentRow.FirstCellUsed();

					int lastRowNum = currentTypeCell.Address.RowNumber;
					while (currentTypeCell.IsEmpty() == false)
					{
						//or here
						var type = currentTypeCell.Value.ToString();
						if (DataValidation.IsNameValid(type))
						{
							var currentValueCell = currentTypeCell.CellBelow();
							while (currentValueCell.IsEmpty() == false)
							{
								if (currentValueCell.Value is DateTime)
									break;

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

						currentTypeCell = currentTypeCell.CellRight();
					}
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

				finalDate = null;
			}
		}
	}
}
