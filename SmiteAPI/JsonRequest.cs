using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmiteAPI
{
    public class JsonRequest
    {
        public static async Task<T> Get<T>(string url, bool unboxArray = false) where T : class
        {
            T response = null;
            string data = await Get(url);

            if (data != null)
            {
                if (unboxArray)
                {
                    var arr = JsonConvert.DeserializeObject<T[]>(data);
                    if (arr != null && arr.Length > 0)
                        response = arr[0];
                }
                else
                { 
                    response = JsonConvert.DeserializeObject<T>(data);
                }
            }

            return response;
        }

        private static async Task<string> Get(string url)
        {
            string jsonString = string.Empty;
            try
            {
                // Create HTTP-Request
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webRequest.Method = "GET";                
                // Get Response via awaitable
                HttpWebResponse webResponse = (await webRequest.GetResponseAsync()) as HttpWebResponse;
                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (TextReader reader = new StreamReader(webResponse.GetResponseStream()))
                        jsonString = reader.ReadToEnd();

                }
            }
            catch (Exception ex)
            {
                // TODO: Log
            }

            return jsonString;
        }

        public static T DeserializeJson<T>(string jsonData) where T : class
        {
            T value = default(T);

            if (!string.IsNullOrEmpty(jsonData))
            {
                try { value = JsonConvert.DeserializeObject<T>(jsonData); }
                catch { }
            }

            return value;
        }

        public static string SerializeJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
