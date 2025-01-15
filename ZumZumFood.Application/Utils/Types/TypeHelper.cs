using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NAB.APP.Core.Utils.Types
{
    public class TypeHelper
    {
        /// <summary>
        /// Lấy tên của property (để tránh hard-code)
        /// Sử dụng :  string nameOfCodeProperty = TypeHelper.GetPropertyName(() => model.Code);
        /// => Kết quả là chuỗi 'model.Code'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda"></param>
        /// <returns></returns>
        public static string GetPropertyNameIncludeMemberName<T>(Expression<Func<T>> propertyLambda)
        {
            MemberExpression me = propertyLambda.Body as MemberExpression;
            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            string result = string.Empty;
            do
            {
                result = me.Member.Name + "." + result;
                me = me.Expression as MemberExpression;
            } while (me != null);

            result = result.Remove(result.Length - 1); // remove the trailing "."
            return result;
        }

        /// <summary>
        /// Lấy tên của property (để tránh hard-code)
        /// Sử dụng :  string nameOfCodeProperty = TypeHelper.GetPropertyName(() => model.Code);
        /// => Kết quả là chỉ là chuỗi 'Code'
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return (propertyExpression.Body as MemberExpression).Member.Name;
        }

    }
}
