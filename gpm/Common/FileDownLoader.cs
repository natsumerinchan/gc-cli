using gpm.Hanlder;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Text;
using System.Threading.Tasks;
using static gpm.Program;

namespace gpm.Common
{
    public static class FileDownLoader
    {
        /// <summary>
        /// 下载实时返回下载进度
        /// </summary>
        /// <param name="URL">下载地址</param>
        /// <param name="filename">本地存储地址</param>
        /// <param name="action">委托回调函数</param>
        public async static Task DownloadFileData(string URL, string filename, Action<int> action,bool proxy)
        {
            if (proxy)
            {
                URL = PluginHandler.GetProxyString(URL);
            }
            else
            {

            }


            if(!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }

            var progressMessageHandler = new ProgressMessageHandler(new HttpClientHandler());

            progressMessageHandler.HttpReceiveProgress += (j, m) =>
            {
                var p = m.ProgressPercentage;

                action.Invoke(p);
            };


            using (HttpClient client = new HttpClient(progressMessageHandler))
            {

                using (var filestream = new FileStream(filename, FileMode.Create))
                {
                    var netstream = await client.GetStreamAsync(URL);
                    await netstream.CopyToAsync(filestream);
                }
                ////wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                //try
                //{
                //    await wc.DownloadFileTaskAsync(new Uri(URL), filename);

                //}
                //catch (Exception e)
                //{
                //    MsgHelper.E(e.Message);
                //    throw;
                //}
            }


        }


        public async static Task DownloadFileData(string URL, string filename,Action<string>action, bool proxy)
        {
            if (proxy)
            {
                URL = PluginHandler.GetProxyString(URL);
            }
            else
            {

            }



            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }

            using (WebClient wc = new WebClient())
            {
                try
                {
                    //await wc.DownloadFileTaskAsync(new Uri(URL), filename);
                    wc.DownloadFile(new Uri(URL), filename);

                }
                catch (Exception e)
                {
                    MsgHelper.E(e.Message);
                    throw;
                }
            }
        }
    }
}
