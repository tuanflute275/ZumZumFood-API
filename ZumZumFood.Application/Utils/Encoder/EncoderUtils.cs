using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAB.APP.Core.Utils.Encoder
{
    public class EncoderUtils
    {
        public static string XacDinhNguyenAm(string searchKey)
        {
            if(string.IsNullOrWhiteSpace(searchKey))
                return string.Empty;

            searchKey = searchKey.ToLower();
            char[] charArr = searchKey.ToCharArray();
            searchKey = "";
            foreach (char c in charArr)
            {
                string tmp = "";
                tmp = c.ToString();

                if (c == 'a')
                {
                    tmp = "[aáàạảãâấầậẩẫăắằặẳẵ]";
                }
                if (c == 'e')
                {
                    tmp = "[eéèẹẻẽêếềệểễ]";
                }
                if (c == 'o')
                {
                    tmp = "[oóòọỏõôốồộổỗơớờợởỡ]";
                }
                if (c == 'i')
                {
                    tmp = "[iíìịỉĩ]";
                }
                if (c == 'u')
                {
                    tmp = "[uúùụủũưứừựửữ]";
                }
                if (c == 'd')
                {
                    tmp = "[dđ]";
                }
                if (c == 'y')
                {
                    tmp = "[ýyỳỷỹ]";
                }
                searchKey += tmp;
            }
            return searchKey;
        }
    }
}
