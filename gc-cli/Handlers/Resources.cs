using Flurl.Http;
using gc_cli.Common;
using gpm.DataTemplates;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static gc_cli.ConstProps.Paths;

namespace gc_cli.Handlers
{
    internal class Resources
    {
        const string REPO = "https://raw.githubusercontent.com/gc-toolkit/GPM-Index/main/metadata/zh/gc-resource/index.json";

        private static void EnsureInit()
        {
            EnvHandler.Init(false);
        }
        private static bool checkMetaData()
        {
            return EnvHandler.CheckMetaData(RESOURCE_METADATA_FILE);
        }

        private static List<ResMetaData.Root> ReadMetaData()
        {
            try
            {
                return JsonConvert
                    .DeserializeObject<List<ResMetaData.Root>>
                    (File.ReadAllText(Path.Combine(METADATA_DIR, RESOURCE_METADATA_FILE)));
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
                    await ProxyHelper.GetRawProxy(REPO).DownloadFileAsync(METADATA_DIR, RESOURCE_METADATA_FILE);


                }
                else
                {
                    await REPO.DownloadFileAsync(METADATA_DIR, RESOURCE_METADATA_FILE);

                }
                MsgHelper.I($"成功更新了Resources的源信息！");

            }
            catch (FlurlHttpException ex)
            {
                MsgHelper.E($"更新Resources源信息失败！");
                AnsiConsole.WriteException(ex);
            }


        }

        public static async Task Install(string ver, bool proxy = false)
        {

            EnsureInit();

            if (!checkMetaData())
            {
                return;
            }
            var metadata = ReadMetaData();
            ResMetaData.Root selMetadata = new ResMetaData.Root(); ;
            string selrepo = "";

            if (metadata.Count > 1)
            {
                var selList = from r in metadata select r.name;

                if (!string.IsNullOrEmpty(ver))
                {
                    selrepo = ver;
                }
                else
                {

                    selrepo = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("选择一个你要下载的项目.")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                        .AddChoices(
                            selList.ToArray<string>()
                        ));
                    AnsiConsole.WriteLine($"You selected {selrepo}");

                }




                selMetadata = (metadata.Where(r => r.name == selrepo)).FirstOrDefault();


                if (selMetadata == null)
                {
                    MsgHelper.E($"未找到名称为 {ver} 的资源，请检查拼写后重试！");
                    return;
                }
            }
            else
            {
                selMetadata = metadata.FirstOrDefault();
            }


            var downLoadUrl = selMetadata.archive.url;

            if (selMetadata.type == "anonfiles")
            {
                downLoadUrl = await Common.AnonfileTools.GetDownloadUrl(downLoadUrl);
            }
            if (proxy)
            {
                downLoadUrl = ProxyHelper.GetRawProxy(downLoadUrl);
            }

            var originPath = selMetadata.archive.path.target;

            var filep = Path.GetFileName(downLoadUrl);

            var zipfile = Path.Combine(DOWNLOAD_CACHE_DIR, filep);

            AnsiConsole.Markup("[[1/3]]下载文件...\n");

            new DownLoader().DownLoadWithProgressBar(downLoadUrl, zipfile);

            AnsiConsole.Markup("[[2/3]]解压文件...\n");

            Unziper.UnzipFile(zipfile, RESOURCE_DIR);

            FolderMover.MoveFolder(Path.Combine(RESOURCE_DIR, originPath), RESOURCE_DIR);


            AnsiConsole.Markup($"[[3/3]]清理文件...\n");

            File.Delete(zipfile);

            DirectoryInfo di = new DirectoryInfo(
                        Path.Combine(RESOURCE_DIR, originPath.Split("/").FirstOrDefault())
                        );
            di.Delete(true);

            MsgHelper.I($"成功安装了Grasscutter资源文件！");
        }
    }
}
