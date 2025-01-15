using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net;
namespace NAB.APP.Core.Utils.Ftp
{
    public class FtpUtil
    {
        public string ftpServerIP;
        public string ftpUserID;
        public string ftpPassword;
        public string rootFolder;
        public FtpUtil() { }
        public FtpUtil(string _ftpServerIP, string _ftpUserID, string _ftpPassword, string _rootFolder)
        {
            ftpServerIP = _ftpServerIP;
            ftpUserID = _ftpUserID;
            ftpPassword = _ftpPassword;
            rootFolder = _rootFolder;
        }
        public bool Upload(string filename, byte[] content)

        {

            //FileInfo fileInf = new FileInfo(filename);

            string uri = "ftp://" + ftpServerIP + "/" + rootFolder + filename;

            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + rootFolder + filename));

            // Provide the WebPermission Credintials

            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

            // By default KeepAlive is true, where the control connection is not closed

            // after a command is executed.

            reqFTP.KeepAlive = false;

            // Specify the command to be executed.

            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            // Specify the data transfer type.

            reqFTP.UseBinary = true;

            // Notify the server about the size of the uploaded file

            reqFTP.ContentLength = content.Length;

            // The buffer size is set to 2kb

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded

            MemoryStream mStream = new MemoryStream(content);

            //mStream.Write(content, 0, content.Length);

            //FileStream fs = fileInf.OpenRead();

            try

            {

                // Stream to which the file to be upload is written

                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time

                contentLen = mStream.Read(buff, 0, buffLength);

                // Until Stream content ends

                while (contentLen != 0)

                {

                    // Write Content from the file stream to the FTP Upload Stream

                    strm.Write(buff, 0, contentLen);

                    contentLen = mStream.Read(buff, 0, buffLength);

                }

                // Close the file stream and the Request Stream

                strm.Close();

                mStream.Close();
                return true;
            }

            catch (Exception ex)

            {

                return false;

            }

        }


        public bool Download(string filePath, string fileName)

        {

            FtpWebRequest reqFTP;

            try

            {

                //filePath = <<The full path where the file is to be created. the>>,

                //fileName = <<Name of the file to be createdNeed not name on FTP server. name name()>>

                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/Export/" + fileName));

                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                Stream ftpStream = response.GetResponseStream();

                long cl = response.ContentLength;

                int bufferSize = 2048;

                int readCount;

                byte[] buffer = new byte[bufferSize];


                readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);

                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }


                ftpStream.Close();

                outputStream.Close();

                response.Close();

                return true;

            }

            catch (Exception ex)

            {

                return false;

            }

        }

        public void Delete(string fileName)

        {

            FtpWebRequest reqFTP;

            try

            {


                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/Export" + "/" + fileName));

                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                response.Close();

            }

            catch (Exception ex)

            {

                Console.WriteLine(ex.Message);

            }

        }

        public string[] GetFileList(string gate)

        {

            string[] downloadFiles;

            StringBuilder result = new StringBuilder();

            FtpWebRequest reqFTP;

            try

            {

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/Export/"));

                reqFTP.UseBinary = true;

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

                WebResponse response = reqFTP.GetResponse();

                StreamReader reader = new StreamReader(response.GetResponseStream());


                string line = reader.ReadLine();

                while (line != null)
                {
                    if (line.Contains(gate))
                    {
                        result.Append(line);

                        result.Append("\n");


                    }
                    line = reader.ReadLine();
                }

                // to remove the trailing '\n'

                result.Remove(result.ToString().LastIndexOf('\n'), 1);

                reader.Close();

                response.Close();

                return result.ToString().Split('\n');

            }

            catch (Exception ex)

            {

                Console.WriteLine(ex.Message);

                downloadFiles = null;

                return downloadFiles;

            }

        }
    }
}