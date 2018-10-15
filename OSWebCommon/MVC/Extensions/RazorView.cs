using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Text;

namespace OS.Web.MVC.Extensions
{

    // THIS EXTENSION SUPPORT CLASS HELPS TO GENERATE STRINGS FROM RAZOR 'ActionResult' OBJECTS
    public class ResponseCapture : IDisposable
    {
        private readonly HttpResponseBase response;
        private readonly TextWriter originalWriter;
        private StringWriter localWriter;
        public ResponseCapture(HttpResponseBase response)
        {
            this.response = response;
            originalWriter = response.Output;
            localWriter = new StringWriter();
            response.Output = localWriter;
        }
        public override string ToString()
        {
            localWriter.Flush();
            return localWriter.ToString();
        }
        public void Dispose()
        {
            if (localWriter != null)
            {
                localWriter.Dispose();
                localWriter = null;
                response.Output = originalWriter;
            }
        }
    }

    /// THIS EXTENSION GENERATES STRINGS FROM RAZOR 'ActionResult' OBJECTS
    public static class ActionResultExtensions
    {
        public static string Capture(this ActionResult result, ControllerContext controllerContext)
        {
            using (var it = new ResponseCapture(controllerContext.RequestContext.HttpContext.Response))
            {
                result.ExecuteResult(controllerContext);
                return it.ToString();
            }
        }
    }
    public static class ControllerExtensions
    {
        public static T GetPostedObjectsFromHttpRequest<T>(this Controller ctrl, HttpRequestBase request)
        {

            HttpRequestBase req = ctrl.HttpContext.Request;
            string documentContents = "";
            Stream receiveStream = null;
            try
            {
                receiveStream = req.InputStream;
                using (StreamReader readStream = new StreamReader(receiveStream, req.ContentEncoding))
                {
                    try
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        receiveStream = null;
                    }
                }
                receiveStream = null;
            }
            finally
            {
                if (receiveStream != null) receiveStream.Dispose();
            }

            var json = new JavaScriptSerializer();
            return json.Deserialize<T>(documentContents);
        }

        /// <summary>
        /// parses out a C# object from a json response, input object is posted to url in json format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="inputObject"></param>
        /// <returns></returns>
        public static T GetObjectFromUrl<T>(this Controller ctrl, string url, object inputObject)
        {
            // Get the session userId from the service

            var http = (HttpWebRequest)WebRequest.Create(new Uri(url));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";

            var json = new JavaScriptSerializer();

            string parsedContent = json.Serialize(inputObject);
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(parsedContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            // Get the returned session userId and data
            var rcvData = json.Deserialize<T>(content);
            return rcvData;

        }


    }
}
