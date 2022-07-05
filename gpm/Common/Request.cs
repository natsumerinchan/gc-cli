using gpm.Hanlder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace gpm
{
    public class Request
    {
        private HttpClient httpClient;

        public Request()
        {
            HttpClientHandler handler = new HttpClientHandler();
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.74 Safari/537.36 Edg/99.0.1150.46");
          
        }

        public async Task<string> Get(string url, Dictionary<string, string> dic = null,bool proxy=false)
        {
            //参数添加
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic != null)
            {

                if (dic.Count > 0)
                {
                    builder.Append("?");
                    int i = 0;
                    foreach (var item in dic)
                    {
                        if (i > 0)
                            builder.Append("&");
                        builder.AppendFormat("{0}={1}", item.Key, item.Value);
                        i++;
                    }
                }
            }

            string URL;
            if (proxy)
            {
                URL = PluginHandler.GetProxyString(builder.ToString());
            }
            else
            {
                URL = builder.ToString();
            }
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
                

        }




    }
}
