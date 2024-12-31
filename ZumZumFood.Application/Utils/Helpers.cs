namespace ZumZumFood.Application.Utils
{
    public class Helpers
    {
        public static bool IsMobileDevice(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return false;

            // Danh sách các từ khóa phổ biến trong User-Agent của thiết bị di động
            var mobileKeywords = new[]
            {
                "Android", "iPhone", "iPad", "iPod", "Opera Mini", "Mobile", "BlackBerry", "Windows Phone",
                "webOS", "IEMobile", "SamsungBrowser", "MeeGo", "AvantGo"
            };

            // Kiểm tra nếu User-Agent chứa bất kỳ từ khóa nào
            return mobileKeywords.Any(keyword =>
                userAgent.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        public static string GenerateSlug(string value)
        {
            string slug = RemoveVietnameseAccents(value.ToLowerInvariant());
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-").Trim('-');
            return slug;
        }

        private static string RemoveVietnameseAccents(string input)
        {
            string[] vietnameseSigns = new string[]
            {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
            };

            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                {
                    input = input.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
                }
            }

            return input;
        }

    }
}
