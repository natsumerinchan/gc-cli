using Flurl.Http;
using gc_cli.Common;
using gpm.DataTemplates;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static gc_cli.ConstProps.Paths;
namespace gc_cli.Handlers
{
    internal class Core
    {
        const string REPO = "https://raw.githubusercontent.com/gc-toolkit/GPM-Index/main/metadata/zh/gc-core/index.json";

        private static void EnsureInit()
        {
            EnvHandler.Init(false);
        }

        private static bool checkMetaData()
        {
            return EnvHandler.CheckMetaData(CORE_METADATA_FILE);

        }

        private static CoreMetaData.Root ReadMetaData()
        {
            try
            {
                return JsonConvert
                    .DeserializeObject<CoreMetaData.Root>
                    (File.ReadAllText(Path.Combine(METADATA_DIR, CORE_METADATA_FILE)));
            }
            catch (Exception)
            {

                throw;
            }

        }


        public static async Task Update(bool Proxy = false)
        {

            EnsureInit();

            MsgHelper.I($"正在从:{REPO}下载源信息...");

            try
            {
                if (Proxy)
                {
                    await ProxyHelper.GetRawProxy(REPO).DownloadFileAsync(METADATA_DIR, CORE_METADATA_FILE);

                }
                else
                {
                    await REPO.DownloadFileAsync(METADATA_DIR, CORE_METADATA_FILE);

                }
                MsgHelper.I($"成功更新了core的源信息！");

            }
            catch (FlurlHttpException ex)
            {
                MsgHelper.E($"更新core源信息失败！");
                AnsiConsole.WriteException(ex);
            }


        }

        public static async Task Install(string sha = null, bool proxy = false)
        {

            EnsureInit();

            if (!checkMetaData())
            {
                return;
            }

            var metadata = ReadMetaData();

            string downLoadUrl = "";

            if (String.IsNullOrEmpty(sha))
            {
                MsgHelper.W("未指定版本，将安装最新的GrassCutter核心文件！");
                downLoadUrl = metadata.workflow.latest;
            }
            else
            {
                var wr = new WorkflowInfo.Root();

                try
                {
                    MsgHelper.I("正在从 actions 获取信息...");

                    try
                    {
                        //不需要代理
                        if (false)
                        {


                        }
                        else
                        {

                            wr = await $"{metadata.workflow.all}?per_page=100".GetJsonAsync<WorkflowInfo.Root>();


                        }
                    }
                    catch (FlurlHttpException ex)
                    {
                        MsgHelper.E($"获取信息失败！");
                        AnsiConsole.WriteException(ex);
                    }


                    MsgHelper.I($"在 actions 中找到了 {wr.artifacts.Count} 条信息");

                }
                catch (Exception ex)
                {
                    MsgHelper.E(ex.Message);
                    Environment.Exit(0);
                }


                var artifacts = wr.artifacts;
                long[] runId = (from r in artifacts
                                where r.workflow_run.head_sha.StartsWith(sha)
                                select r.id).ToArray();

                if (runId.Count() == 0)
                {
                    MsgHelper.E($"找不到 sha 为 {sha} 版本的GrassCutter.");
                    return;
                }
                downLoadUrl = $"https://nightly.link/Grasscutters/Grasscutter/actions/artifacts/{runId[0]}.zip";


            }

            var filep = Path.GetFileName(downLoadUrl);

            var zipfile = Path.Combine(DOWNLOAD_CACHE_DIR, filep);

            AnsiConsole.Markup("[[1/3]]下载文件...\n");

            new DownLoader().DownLoadWithProgressBar(downLoadUrl, zipfile);

            AnsiConsole.Markup("[[2/3]]解压文件...\n");

            Unziper.UnzipFile(zipfile, Environment.CurrentDirectory);

            AnsiConsole.Markup($"[[3/3]]清理文件...\n");

            File.Delete(zipfile);


            MsgHelper.I($"成功安装了Grasscutter核心文件！");


        }


    }
}
