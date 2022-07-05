using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading;
using System.Threading.Tasks;
namespace gpm.Common
{
    public class DownLoad
    {
        public int NowProcess = 0;
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">下载host</param>
        /// <param name="filepath">下载文件的保存地址</param>
        /// <returns></returns>
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">下载host</param>
        /// <param name="filepath">下载文件的保存地址</param>
        /// <returns></returns>
        public async Task DownloadFile(string url, string filepath, Action<DownArgs> action=null)
        {
            var progressMessageHandler = new ProgressMessageHandler(new HttpClientHandler());
            progressMessageHandler.HttpReceiveProgress += (j, m) =>
            {
                var b = new DownArgs()
                {
                    NowProcess = m.ProgressPercentage
                    ,
                    NowLength = m.BytesTransferred
                };
                //action.Invoke(b);
            };
            using (var client = new HttpClient(progressMessageHandler))
            {
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    var netstream = await client.GetStreamAsync(url);
                    await netstream.CopyToAsync(filestream);
                }
            }
        }


    }

    public class DownArgs
    {
        public long NowProcess = 0;
        public long NowLength = 0;
    }
}
