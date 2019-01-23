using Clockwork.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Clockwork.Web
{
    public class Common
    {
        /// <summary>
        /// Gets value from application config file.
        /// </summary>
        /// <param name="name">the key name of data from the config file.</param>
        /// <returns></returns>
        public static string GetConfigurationValue(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            return value;
        }

        public static string ConvertObjectToJSON(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return json;
        }

        //Convert JSON To Typed Object
        public static T ConvertJSONToObject<T>(string json)
        {
            T entity = (T)JsonConvert.DeserializeObject<T>(json);

            return entity;
        }

        //Convert json into a Typed object DataContainer.
        public static DataContainer GetData<T>(string json)
        {
            var messageList = new List<string>();
            var dataContainer = new DataContainer();

            return ConvertJSONToObject<DataContainer>(json);
        }

        public static string UppercaseFirstLetter(string value)
        {
            value = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            return value; 
        }

        public static string GetParameterStrings(Dictionary<string, string> parametersWithdata)
        {
            var parameterBuilder = new StringBuilder();
            int totalData = 0;
            int count = 0;

            if (parametersWithdata != null)
            {
                totalData = parametersWithdata.Values.Count;

                foreach (var entry in parametersWithdata)
                {
                    count++;

                    parameterBuilder.Append(entry.Key + "=" + entry.Value.Trim());

                    if (count < totalData)
                    {
                        parameterBuilder.Append("&");
                    }
                }
            }

            return !string.IsNullOrEmpty(parameterBuilder.ToString().Trim()) ? "?" + parameterBuilder.ToString() : string.Empty;
        }


        public static string GetFullURL(string url, string methodName)
        {
            return url.Trim() + "/" + methodName.Trim();
        }

        /// <summary>
        /// Request POST Connection (HTTP Post)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="objData"></param>
        /// <returns></returns>
        public static DataContainer PostDataWithParameters<T>(string url, object objData)
        {
            var strResult = string.Empty;
            var normalizedParameters = string.Empty;

            var postdata = "{\"data\":" + Common.ConvertObjectToJSON(objData) + "}";

            var encoding = new ASCIIEncoding();
            //*********
            // convert xmlstring to byte using ascii encoding
            byte[] data = encoding.GetBytes(postdata);

            // declare httpwebrequet wrt url defined above
            var webrequest = (HttpWebRequest)WebRequest.Create(url);

            // set method as post
            webrequest.Method = "POST";

            // set content type
            webrequest.ContentType = "application/json; charset=utf-8";

            // set content length
            webrequest.ContentLength = data.Length;

            // get stream data out of webrequest object
            var newStream = webrequest.GetRequestStream();

            newStream.Write(data, 0, data.Length);
            newStream.Close();

            // declare & read response from service
            var webresponse = (HttpWebResponse)webrequest.GetResponse();


            // read response stream from response object
            var loResponseStream = new StreamReader(webresponse.GetResponseStream(), Encoding.UTF8);

            // read string from stream data
            strResult = loResponseStream.ReadToEnd();


            // close the stream object
            loResponseStream.Close();

            // close the response object
            webresponse.Close();
            strResult = System.Net.WebUtility.HtmlDecode(strResult);

            var dataContainer = Common.GetData<T>(strResult);

            return dataContainer;
        }

        /// <summary>
        /// Request GET Request (HTTP Get)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parametersWithData"></param>
        /// <returns></returns>
        public static DataContainer GetDataWithParameter<T>(string url, Dictionary<string, string> parametersWithData)
        {
            var strResult = string.Empty;
            var normalizedParameters = string.Empty;

            url = url + Common.GetParameterStrings(parametersWithData);

            // declare httpwebrequet wrt url defined above
            var webrequest = (HttpWebRequest)WebRequest.Create(url);

            // set method as post
            webrequest.Method = "GET";

            // set content type 
            webrequest.ContentType = "application/x-www-form-urlencoded";

            // declare & read response from service
            var webresponse = (HttpWebResponse)webrequest.GetResponse();

            // set utf8 encoding

            // read response stream from response object
            var loResponseStream = new StreamReader
                (webresponse.GetResponseStream(), Encoding.UTF8);

            // read string from stream data
            strResult = loResponseStream.ReadToEnd();

            // close the stream object
            loResponseStream.Close();

            // close the response object
            webresponse.Close();

            // return strResult;

            strResult = System.Net.WebUtility.HtmlDecode(strResult);

            var dataContainer = Common.GetData<T>(strResult);

            return dataContainer;
        }

        public static DataContainer GetDataResult<T>(string _url, string methodName, HttpType type, object data)
        {
            _url = GetFullURL(_url, methodName);
            DataContainer dataContainer = null;

            if (type == HttpType.GET)
            {
                dataContainer = GetDataWithParameter<T>(_url, (Dictionary<string, string>)data);
            }
            else
            {
                dataContainer = PostDataWithParameters<T>(_url, data);
            }

            return dataContainer;
        }

        public static string CheckMessage(string message)
        {
            var errorMessage = string.Empty;

            if (message.ToLower().Contains("unable to connect"))
            {
                errorMessage = "Opss! Something's wrong with the API!";
            }
            else
            {
                errorMessage = "Error encountered: " + message;
            }

            return errorMessage;
        }
    }
}