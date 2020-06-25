using System;

namespace Model
{
	public static class DataValidation
	{
		/// <summary>
		/// Validates <paramref name="input"/> string to be a Name
		/// </summary>
		/// <param name="input">String to be validated</param>
		/// <returns>Can <paramref name="input"/> string be Name or not: true - can, otherwise - false</returns>
		public static bool IsNameValid(string input) => string.IsNullOrWhiteSpace(input) == false &&
															IsFirstCharCapital(input);
		/// <summary>
		/// Validates <paramref name="input"/> string to be started with capital letter
		/// </summary>
		/// <param name="input">String to be validated</param>
		/// <returns>True - <paramref name="input"/> starts from catital, otherwise - false</returns>
		public static bool IsFirstCharCapital(string input)
		{
			return input.Length > 0 && input.ToUpper()[0] == input[0];
		}


		public static bool IsValueValid(decimal value) => Math.Abs(value) < (decimal)1e9;
	}
}
