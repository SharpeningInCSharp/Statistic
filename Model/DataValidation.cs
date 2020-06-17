using System;

namespace Model
{
	public static class DataValidation
	{
		public static bool NameValid(string input) => string.IsNullOrWhiteSpace(input) == false && input.Length > 0 &&
															IsFirstCharCapital(input);

		private static bool IsFirstCharCapital(string input)
		{
			return input.ToUpper()[0] == input[0];
		}

		public static bool IsValueValid(decimal value) => Math.Abs(value) < (decimal)1e9;
	}
}
