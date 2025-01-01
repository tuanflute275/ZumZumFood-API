using System.Text.RegularExpressions;

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

        public static string BodyRegisterMail(string fullName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "RegisterSuccessMail.cshtml");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Template file not found at: {path}");
            }

            string body = File.ReadAllText(path);
            return body.Replace("{{fullName}}", fullName);
        }

        public static string BodyResetPasswordMail(string pass)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "ForgotPasswordMail.cshtml");
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Template file not found at: {path}");
            }

            string body = File.ReadAllText(path);
            return body.Replace("{{Password}}", pass);
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


        public static class InputValidator
        {
            /// <summary>
            /// Kiểm tra một chuỗi có chứa ký tự đặc biệt hay không.
            /// </summary>
            /// <param name="input">Chuỗi cần kiểm tra.</param>
            /// <returns>True nếu chứa ký tự đặc biệt, ngược lại là False.</returns>
            public static bool ContainsSpecialCharacters(string input)
            {
                if (string.IsNullOrEmpty(input)) return false;
                var regex = new Regex(@"[^a-zA-Z0-9\s]");
                return regex.IsMatch(input);
            }

            /// <summary>
            /// Kiểm tra giá trị sắp xếp có hợp lệ hay không (chỉ chấp nhận "asc" hoặc "desc").
            /// </summary>
            /// <param name="sort">Chuỗi cần kiểm tra.</param>
            /// <returns>True nếu hợp lệ, ngược lại là False.</returns>
            public static bool IsValidSortOption(string sort)
            {
                if (string.IsNullOrEmpty(sort)) return true; // Cho phép null hoặc rỗng.

                var sortParts = sort.Split('-');
                if (sortParts.Length != 2) return false; // Định dạng phải là "Field-Direction".

                string field = sortParts[0];
                string direction = sortParts[1].ToLower();

                // Kiểm tra trường và thứ tự sắp xếp hợp lệ
                return (field == "Id" || field == "Name") && (direction == "asc" || direction == "desc");
            }

            /// <summary>
            /// Kiểm tra số trang có hợp lệ hay không (phải là số nguyên dương).
            /// </summary>
            /// <param name="pageNo">Số trang cần kiểm tra.</param>
            /// <returns>True nếu hợp lệ, ngược lại là False.</returns>
            public static bool IsValidNumber(int number)
            {
                return number > 0 && number <= int.MaxValue;
            }

            /// <summary>
            /// Tổng hợp validate tất cả các tham số.
            /// </summary>
            /// <param name="keyword">Từ khóa tìm kiếm.</param>
            /// <param name="sort">Giá trị sắp xếp.</param>
            /// <param name="pageNo">Số trang.</param>
            /// <returns>Thông báo lỗi nếu không hợp lệ, null nếu hợp lệ.</returns>
            public static string? ValidateInput(string? keyword, string? sort, int pageNo)
            {
                if (!string.IsNullOrEmpty(keyword) && ContainsSpecialCharacters(keyword))
                {
                    return "Keyword must not contain special characters!";
                }

                if (!IsValidSortOption(sort))
                {
                    return "Invalid sort! The format must be 'Field-Direction' (e.g., 'Id-DESC' or 'Name-ASC').";
                }

                if (!IsValidNumber(pageNo))
                {
                    return "Page number (pageNo) must be greater than 0 and less than or equal to the maximum value of int!";
                }

                return null; // Tất cả đều hợp lệ.
            }
        }

    }
}
