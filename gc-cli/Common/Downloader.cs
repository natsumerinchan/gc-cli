using Spectre.Console;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading.Tasks;
namespace gc_cli.Common
{
    public class DownLoader
    {
        public int NowProcess = 0;
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">下载host</param>
        /// <param name="filepath">下载文件的保存地址</param>
        /// <returns></returns>
        public async Task DownloadFile(string url, string filepath, Action<DownArgs> action)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            }
            var progressMessageHandler = new ProgressMessageHandler(new HttpClientHandler());
            progressMessageHandler.HttpReceiveProgress += (j, m) =>
            {
                var b = new DownArgs()
                {
                    NowProcess = m.ProgressPercentage
                    ,
                    NowLength = m.BytesTransferred
                };
                action.Invoke(b);
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


        public void DownLoadWithProgressBar(string url, string filepath)
        {

            MsgHelper.I($"正在从 {url} 下载到 {filepath}");

            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    // Define tasks
                    var task2 = ctx.AddTask("[green]准备中[/]");

                    task2.MaxValue = 100;

                    Task.Run(async () =>
                    {

                        await new DownLoader().DownloadFile(url, filepath, (a) =>
                        {
                            task2.Description = $"[green]已下载 {a.NowLength / 1024} kb[/]";
                            task2.Value = a.NowProcess;

                        });

                        task2.Value = 100;
                    }).Wait();


                });

            MsgHelper.I($"下载完成");

        }
    }

    public class DownArgs
    {
        public long NowProcess = 0;
        public long NowLength = 0;
    }
}
