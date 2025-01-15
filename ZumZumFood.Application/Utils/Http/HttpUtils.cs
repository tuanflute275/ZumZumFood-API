using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace NAB.APP.Core.Utils.Http
{
    public class HttpUtils
    {

        public static string MakeRequest(HttRequestData requestData)
        {
            string strResult = string.Empty;
            StreamReader strReader = null;
            HttpWebResponse response = null;

            try
            {
                if (requestData.IsEnableSSL)
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    (
                        senderX, certificate, chain, sslPolicyErrors
                     ) => { return false; };
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestData.Endpoint);

                request.Method = requestData.Method;
                request.ContentType = requestData.ContentType; // "application/x-www-form-urlencoded"; // data type client request
                request.Accept = requestData.AcceptType; // "application/json"; // data type that server will send back
                request.ContentLength = 0; // length of data that client will be send to server

                // POST method with DATA
                if (!string.IsNullOrEmpty(requestData.PostData) && requestData.Method == HttpConstant.RequestMethod.POST)
                {
                    var encoding = new UTF8Encoding();
                    var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(requestData.PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                        writeStream.Flush();
                    }
                }

                // HTTP Basic Authentication
                if (!string.IsNullOrWhiteSpace(requestData.BasicAuthenUser))
                {
                    string authInfo = requestData.BasicAuthenUser + ":" + requestData.BasicAuthenPassword;

                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    request.Headers["Authorization"] = "Basic " + authInfo;
                }

                if (!string.IsNullOrWhiteSpace(requestData.NetworkCredentialUsername))
                {
                    request.Credentials = new NetworkCredential(requestData.NetworkCredentialUsername, requestData.NetworkCredentialPassword);
                }

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = String.Format("Request failed. Received HTTP Status : {0} , Des : {1}",
                                                            response.StatusCode, response.StatusDescription);
                    throw new ApplicationException(message);
                }

                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");

                if (response != null)
                {
                    strReader = new StreamReader(response.GetResponseStream(), enc);

                    // read string from stream data
                    strResult =  strReader.ReadToEnd();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                try
                {
                    if (strReader != null)
                        strReader.Close();

                    if (response != null)
                        response.Close();
                }
                catch { }
            }

           // HttpResponse _response = JsonConvert.DeserializeObject<HttpResponse>(strResult);
            return strResult;
        }

    }
}
