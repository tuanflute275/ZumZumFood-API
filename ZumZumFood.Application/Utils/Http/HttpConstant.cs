using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAB.APP.Core.Utils.Http

{
    public class HttpConstant
    {

        public static class RequestMethod
        {
            public const string GET = "GET";
            public const string POST = "POST";
            public const string PUT = "PUT";
            public const string DELETE = "DELETE";
        }

        /// <summary>
        /// Almost for POST or PUT Request Method 
        /// (For instance : The Type of data that client will send)
        /// </summary>
        public static class ContentType
        {
            public const string URL_ENCODED = "application/x-www-form-urlencoded";
            public const string FORM_DATA = "multipart/form-data";
            public const string TEXT_XML = "text/xml";
        }

        public static class AcceptType
        {
            public const string APPLICATION_XML = "application/xml";
            public const string APPLICATION_JSON = "application/json";
        }

    }
}
