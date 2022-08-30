using Flurl.Http;
using gc_cli.Common;
using gc_cli.DataTemplates;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static gc_cli.ConstProps.Paths;

namespace gc_cli.Handlers
{
    public static class Packages
    {

        const string REPO = "https://raw.githubusercontent.com/gc-toolkit/GPM-Index/main/metadata/zh/gc-packages/index.json";

        private static void EnsureInit()
        {
            EnvHandler.Init(false);
        }
        private static bool checkMetaData()
        {
            return EnvHandler.CheckMetaData(PACKAGE_METADATA_FILE);

        }

        private static List<DataTemplates.PackageMetaData.Root> ReadMetaData()
        {
            List<DataTemplates.PackageMetaData.Root> ret = new List<DataTemplates.PackageMetaData.Root>();

            try
            {
                ret = JsonConvert.DeserializeObject<List<DataTemplates.PackageMetaData.Root>>
                    (File.ReadAllText(Path.Combine(METADATA_DIR, PACKAGE_METADATA_FILE)));
            }
            catch (Exception)
            {

                throw;
            }
            return ret;
        }

        public static async Task Update(bool Proxy = false)
        {

            EnsureInit();

            MsgHelper.I($"正在从:{REPO}下载源信息...");

            try
            {
                if (Proxy)
                {
                    await ProxyHelper.GetRawProxy(REPO).DownloadFileAsync(METADATA_DIR, PACKAGE_METADATA_FILE);

                }
                else
                {
                    await REPO.DownloadFileAsync(METADATA_DIR, PACKAGE_METADATA_FILE);

                }
                MsgHelper.I($"成功更新了Packages的源信息！");

            }
            catch (FlurlHttpException ex)
            {
                MsgHelper.E($"更新Packages源信息失败！");
                AnsiConsole.WriteException(ex);
            }


        }



        public static async Task ListRepo()
        {
            EnsureInit();


            if (!EnvHandler.CheckMetaData(PLUGIN_METADATA_FILE))
            {
                return;
            }


            var metaData = ReadMetaData();

            // Create a table
            var table = new Table();

            table.AddColumn(new TableColumn("Package Name").Centered());
            table.AddColumn(new TableColumn("作者").Centered());
            table.AddColumn(new TableColumn("描述").Centered());

            foreach (var item in metaData)
            {
                table.AddRow(item.name, item.author, item.description);
                //, , 
            }
            // Render the table to the console
            AnsiConsole.Write(table);

        }


        public static async Task Add(List<string> pkgs, bool Proxy = false)
        {
            EnsureInit();

            if (!checkMetaData())
            {
                return;
            }



            var metaData = ReadMetaData();

            var index = 0;

            foreach (var item in pkgs)
            {
                PackageMetaData.Root temp = metaData.Find(t => t.name == item);

                if (temp == null)
                {
                    MsgHelper.E($"无法找到名称为 {temp.name} 的Package");
                    continue;
                }




                var downLoadUrl = temp.file;


                if (temp.type == "anonfiles")
                {
                    MsgHelper.W($"类型为{temp.type}的文件不支持内置代理下载，将忽略-p选项!");
                    downLoadUrl = await Common.AnonfileTools.GetDownloadUrl(downLoadUrl);
                }
                else
                {
                    if (Proxy)
                    {
                        downLoadUrl = ProxyHelper.GetRawProxy(downLoadUrl);
                    }
                }



                AnsiConsole.Markup(Markup.Escape($"[{index}/{pkgs.Count}] 正在准备安装 {temp.name}\n"));


                var table1 = new Table();
                table1.Border = TableBorder.None;

                table1.AddColumn(new TableColumn("[blue]建议安装:[/]"));
                table1.AddRow(Markup.Escape(String.Join("\n", temp.dependencies.ToArray())));
                AnsiConsole.Write(table1);

                var table = new Table();
                table.Border = TableBorder.None;
                table.AddColumn(new TableColumn("[blue]文件列表:[/]"));
                table.AddRow(Markup.Escape(String.Join("\n", temp.uninstall.ToArray())));
                AnsiConsole.Write(table);

                var filep = Path.GetFileName(downLoadUrl);

                var zipfile = Path.Combine(DOWNLOAD_CACHE_DIR, filep);

                AnsiConsole.Markup("[[1/3]]下载文件...\n");

                new DownLoader().DownLoadWithProgressBar(downLoadUrl, zipfile);

                AnsiConsole.Markup("[[2/3]]解压文件...\n");

                Unziper.UnzipFile(zipfile, Path.Combine(Environment.CurrentDirectory, temp.installationPath));

                AnsiConsole.Markup($"[[3/3]]清理文件...\n");

                File.Delete(zipfile);



                MsgHelper.I($"成功安装了Package: {item}");


                index += 1;
            }

        }


        public static async Task Remove(List<string> pkgs)
        {
            EnsureInit();

            if (!checkMetaData())
            {
                return;
            }



            var metaData = ReadMetaData();

            var index = 0;

            foreach (var item in pkgs)
            {
                PackageMetaData.Root temp = metaData.Find(t => t.name == item);

                if (temp == null)
                {
                    MsgHelper.E($"无法找到名称为 {temp.name} 的Package");
                    continue;
                }

                AnsiConsole.Markup(Markup.Escape($"[{index}/{pkgs.Count}] 正在准备移除{item} {temp.name}\n"));


                var table = new Table();
                table.Border = TableBorder.None;
                table.AddColumn(new TableColumn("[red]要删除的文件列表:[/]").Centered());
                table.AddRow(Markup.Escape(String.Join("\n", temp.uninstall.ToArray())));
                AnsiConsole.Write(table);

                if (!AnsiConsole.Confirm("确定执行删除?"))
                {
                    AnsiConsole.MarkupLine("用户已拒绝. :(");
                    continue;
                }

                foreach (var i in temp.uninstall)
                {
                    var t = Path.Combine(Environment.CurrentDirectory, i);
                    try
                    {
                        if (Directory.Exists(t))
                        {
                            MsgHelper.I($"尝试删除文件夹: {t}");
                            new DirectoryInfo(t).Delete(true);
                        }
                        else if (File.Exists(t))
                        {
                            MsgHelper.I($"尝试删除文件: {t}");
                            File.Delete(t);

                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
                    }
                }


                MsgHelper.I($"成功移除了Package: {item}");
                index += 1;
            }

        }



    }
}
