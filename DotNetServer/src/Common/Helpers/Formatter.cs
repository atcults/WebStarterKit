using System;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public class Formatter
    {
    	public static string YesNo(bool? yesNo)
    	{
    		return yesNo == null ? "No" : YesNo(yesNo.Value);
    	}

    	public static string YesNo(bool yesNo)
    	{
    		return yesNo ? "Yes" : "No";
    	}

    	public static string PhoneNumber(string number)
    	{
    		return String.IsNullOrWhiteSpace(number) ? String.Empty : Regex.Replace(number, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
    	}

        public static string CleanupPhoneNumber(string value)
        {
            if (value == null)
                return string.Empty;

            const string pattern = @"(\()?(\))?(\s)?(-)?";
            var rgx = new Regex(pattern);
            return rgx.Replace(value, string.Empty);
        }

    	public static string Date(DateTime date)
    	{
    		return date.ToString("MM/dd/yyyy");
    	}

        public static string Date(DateTime? date)
        {
            return date.HasValue ? Date(date.Value) : null;
        }

    	public static string DateAndTime(DateTime dateTime)
    	{
    		return dateTime.ToString("MM/dd/yyyy HH:mm");
    	}

        public static string DateAndTime(DateTime? dateTime)
        {
            return dateTime.HasValue ? DateAndTime(dateTime.Value) : null;
        }

    	public static string HourMinute(TimeSpan time)
    	{
    		var hours = time.Hours;
    		string ampm;
    		if (hours >= 12)
    		{
    			if (hours > 12) hours -= 12;
    			ampm = "PM";
    		}
    		else
    		{
    			ampm = "AM";
    		}

    		return String.Format("{0:D2}:{1:D2} {2}", hours, time.Minutes, ampm);
    	}

    	public static string Money(decimal amount)
    	{
    		if (amount == 0)
    			return "$0";

    		var formattedAmount = amount.ToString("C");
    		return formattedAmount;
    	}

    	public static string Money(int? amount)
    	{
    		if (!amount.HasValue)
    			return "$0";

    		var formattedAmount = amount.Value.ToString("C");
    		return formattedAmount;
    	}

    	public static string GetShortDate(DateTime? dateTime, string defaultValue = "Not specified")
    	{
    		return dateTime.HasValue ? dateTime.Value.ToShortDateString() : defaultValue;
    	}

    	public static string DateAsAge(DateTime date)
    	{
    		var days = (SystemTime.Now() - date).TotalDays;
    		if (days < 31)
    		{
    			return String.Format("{0} day(s)", (int)days);
    		}
    		return days < 365 ? String.Format("{0} month(s)", (int)days / 30) : String.Format("{0} year(s)", (int)days / 365);
    	}

        public static bool EmailId(string emailid)
        {
            if (string.IsNullOrEmpty(emailid)) return false;
            const string strRegex = @"((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))";
            var regex = new Regex(strRegex);
            return regex.IsMatch(emailid);
        }

        public static bool MobileNumber(string mobile)
        {
            if (string.IsNullOrEmpty(mobile)) return false;
            var regex = new Regex(@"^\d{10}$");
            return regex.IsMatch(mobile);
        }

        public static bool HasAtLeast1Uppercase(string password)
        {
            const string strRegex = @"(?=.*[A-Z])";
            var regex = new Regex(strRegex);
            return regex.IsMatch(password);
        }

        public static bool HasAtLeast1Lowercase(string password)
        {
            const string strRegex = @"(?=.*[a-z])";
            var regex = new Regex(strRegex);
            return regex.IsMatch(password);
        }
        public static bool HasAtLeast1Number(string password)
        {
            const string strRegex = @"(?=.*\d)";
            var regex = new Regex(strRegex);
            return regex.IsMatch(password);
        }

        public static bool HasAtLeast1SpecialChar(string password)
        {
            const string strRegex = @"(?=.*[_#$%])";
            var regex = new Regex(strRegex);
            return regex.IsMatch(password);
        }
    }
}