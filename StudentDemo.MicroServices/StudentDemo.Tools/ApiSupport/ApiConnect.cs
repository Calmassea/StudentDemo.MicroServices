using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace StudentDemo.Tools.ApiSupport
{
    public static class ApiConnect
    {
        public enum RequestFunction
        {
            Get,
            Post,
            Put,
            Delete
        }
        /// <summary>
        /// 匹配完整格式的URL
        /// </summary>
        /// <param name="s">源字符串</param>
        /// <returns>是否匹配成功</returns>
        public static bool MatchUrl(this string s)
        {
            MatchUrl(s, out var isMatch);
            return isMatch;
        }
        public static Uri MatchUrl(this string s, out bool isMatch)
        {
            try
            {
                isMatch = true;
                return new Uri(s);
            }
            catch
            {
                isMatch = false;
                return null;
            }
        }
        public static T InvokeApi<T>(string url,RequestFunction requestFun) where T:class 
        {
            using (HttpClient link =new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
               
                switch (requestFun)
                {
                    case RequestFunction.Get:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                    case RequestFunction.Post:
                        requestMessage.Method = HttpMethod.Post;
                        break;
                    case RequestFunction.Put:
                        requestMessage.Method = HttpMethod.Put;
                        break;
                    case RequestFunction.Delete:
                        requestMessage.Method = HttpMethod.Delete;
                        break;
                    default:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                }
                requestMessage.RequestUri =new Uri(url);
                var result = link.SendAsync(requestMessage).Result;
                T context = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result);
                return context;
            }
        }
    }
}
