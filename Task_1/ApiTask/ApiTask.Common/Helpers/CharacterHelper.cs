using System.Text;
using System.Text.RegularExpressions;

namespace ApiTask.Common.Helpers
{
    public static class CharacterHelper
    {
        public static string ToEnglishNumbers(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            var persianDigits = new char[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            var englishDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            var result = new StringBuilder(input.Length);

            foreach (var c in input)
            {
                int index = Array.IndexOf(persianDigits, c);
                result.Append(index >= 0 ? englishDigits[index] : c);
            }

            return result.ToString();
        }

        public static bool IsValidPersianPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            string pattern = @"^(?:\+98|0)9[0-9]{9}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
