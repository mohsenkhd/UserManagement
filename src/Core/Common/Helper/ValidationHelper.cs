using System.Text.RegularExpressions;

namespace Common.Helper
{
    public static class ValidationHelper
    {
        public static bool CheckMobileNo(this string mobileNo)
        {
            if (string.IsNullOrEmpty(mobileNo))
            {
                return false;
            }

            if (mobileNo.Trim().Length < 11)
            {
                return false;
            }

            Regex rgx = new(@"^(?:0|98|\+98|\+980|0098|098|00980)?(9\d{9})$");
            return rgx.IsMatch(mobileNo);
        }


        private static readonly Regex NationalCodeRegex = new Regex(@"^\d{10}$", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly string[] InvalidNationalCode =
        {
            "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666",
            "7777777777", "8888888888", "9999999999"
        };

        public static bool IsValid(this string value)
        {
            var isValid = false;
            if (value == null)
            {
                return isValid;
            }

            var nationalCode = value.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(nationalCode))
            {
                return isValid;
            }

            if (!NationalCodeRegex.IsMatch(nationalCode))
            {
                return isValid;
            }

            if (InvalidNationalCode.Contains(nationalCode))
            {
                return isValid;
            }
            var check = Convert.ToInt32(nationalCode.Substring(9, 1));
            var sum = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(nationalCode.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            isValid = (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);

            return isValid;
        }
        public static bool CheckPassword(this string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (password.Trim().Length < 8)
            {
                return false;
            }
            List<string> list = new List<string>();
            list.Add("!");
            list.Add("@");
            list.Add("#");
            list.Add("$");
            list.Add("%");
            list.Add("^");
            list.Add("&");
            list.Add("*");
            foreach (var item in list)
            {
                if (password.Contains(item))
                {
                    return true;
                }
            }
            Regex rgx = new(@"(^(09|9)[1][1-9]\d{7}$)|(^(09|9)[3][12456]\d{7}$)");
            if (rgx.IsMatch(password))
            {
                return true;
            }
            return false;
        }


    }
}