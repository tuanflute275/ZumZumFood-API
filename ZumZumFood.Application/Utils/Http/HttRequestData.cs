using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAB.APP.Core.Utils.Http
{
    public class HttRequestData
    {
        public string Method { get; set; }
        public string Endpoint { set; get; }
        public string ContentType { set; get; }
        public string AcceptType { set; get; }

        public string PostData { set; get; }
        public string BasicAuthenUser { set; get; }
        public string BasicAuthenPassword { set; get; }
        public string NetworkCredentialUsername { set; get; }
        public string NetworkCredentialPassword { set; get; }

        public bool IsEnableSSL { set; get; }


    }

    public class HttpResponse
    {
        string response { set; get; }
       // string property { set; get; }
    }

    public class Property
    {
        int status { set; get; }
        string message { set; get; }
    }
}