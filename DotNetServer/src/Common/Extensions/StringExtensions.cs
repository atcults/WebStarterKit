using System;
using System.Globalization;

namespace Common.Extensions
{
	public static class StringExtensions
	{
		public static bool HasValue(this string value)
		{
			return !String.IsNullOrWhiteSpace(value);
		}

		public static bool IsEmpty(this string value)
		{
			return !HasValue(value);
		}

		public static DateTime ParseDate(this string dateTime)
		{
			return DateTime.ParseExact(dateTime, "MM/dd/yyyy", CultureInfo.InvariantCulture);
		}

        public static DateTime? ParseNullableDate(this string dateTime)
        {
            DateTime dateValue;
            return DateTime.TryParseExact(dateTime, "MM/dd/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue) ? dateValue : default(DateTime?);
        }

		public static DateTime ParseDateAndTime(this string dateTime)
		{
		    return DateTime.ParseExact(dateTime, dateTime.EndsWith("000Z") ? "yyyy-MM-ddTHH:mm:00.000Z" : "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture);
		}

        public static DateTime? ParseNullableDateAndTime(this string dateTime)
        {
            DateTime dateValue;
            return DateTime.TryParseExact(dateTime, dateTime.EndsWith("000Z") ? "yyyy-MM-ddTHH:mm:00.000Z" : "MM/dd/yyyy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue) ? dateValue : default(DateTime?);
        }

	    /// <summary>
		/// This function is useful in parsing time from 00 AM TO 12:59 PM into timespan.
		/// If parse fails then it will return null value.
		/// It supports 12/24 hours format with or without separator
		/// Test cases: Format.ParseTime("090").ShouldEqual(null);
		/// Format.ParseTime("060").ShouldEqual(null);
		/// Format.ParseTime("0930").ShouldEqual(new TimeSpan(0, 9, 30, 0));
		/// Format.ParseTime("09:30 PM").ShouldEqual(new TimeSpan(0, 21, 30, 0));
		/// </summary>
		/// <param name="inputTime">Input time</param>
		/// <returns>Nullable TimeSpan</returns>
		public static TimeSpan? ParseTime(this string inputTime)
		{
			if (String.IsNullOrEmpty(inputTime)) return null;

			var tc = new[] { 'a', 'A', 'p', 'P', 'm', 'M' };
			var time = inputTime.Trim(tc).Replace(" ", "").Replace(":", "").Replace(".", "").Replace("-", "");

			if (time.Length == 0 || time.Length > 4) return null;

			switch (time.Length)
			{
				case 1:
					time = time.PadLeft(2, '0');
					time = time.PadRight(4, '0');
					break;
				case 2:
					time = time.PadRight(4, '0');
					break;
				case 3:
					time = time.PadLeft(4, '0');
					break;
			}

			var chars = time.ToCharArray();

			if (inputTime.ToLower().Contains("pm") && !inputTime.StartsWith("12"))
			{
				chars[0]++;
				chars[1]++;
				chars[1]++;
			}

			var hour = (chars[0] - 48) * 10 + (chars[1] - 48);

			if (hour > 23) return null;

			var min = (chars[2] - 48) * 10 + (chars[3] - 48);

			if (min > 59) return null;

			return new TimeSpan(0, hour, min, 0);
		}
	}
}