using Flurl;
using Flurl.Http;
using gc_cli.Common;
using gpm.Common;
using gpm.DataTemplates;
using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static gc_cli.ConstProps.Paths;

namespace gc_cli.Handlers
{
    internal class Plugin
    {
        const string REPO = "https://raw.githubusercontent.com/gc-toolkit/GPM-Index/main/metadata/zh/gc-plugin/index.json";
        const string GITHUB_API = "https://api.github.com";
        const string GITHUB_API_PROXY = "https://github.lyric.today";

        private static void EnsureInit()
        {
            EnvHandler.Init(false);
        }

        private static string List2Tags(List<string> tags)
        {
            var r = "";
            foreach (var item in tags)
            {
                r += $"[invert]{item}[/] ";
            }
            return r;
        }

        private static bool checkMetaData()
        {
            return EnvHandler.CheckMetaData(PLUGIN_METADATA_FILE);

        }


        public static async Task Update(bool Proxy = false)
        {

            EnsureInit();

            MsgHelper.I($"正在从:{REPO}下载源信息...");

            try
            {
                if (Proxy)
                {
                    await ProxyHelper.GetRawProxy(REPO).DownloadFileAsync(METADATA_DIR, PLUGIN_METADATA_FILE);

                }
                else
                {
                    await REPO.DownloadFileAsync(METADATA_DIR, PLUGIN_METADATA_FILE);

                }
                MsgHelper.I($"成功更新了Plugins的源信息！");

            }
            catch (FlurlHttpException ex)
            {
                MsgHelper.E($"更新Plugins源信息失败！");
                AnsiConsole.WriteException(ex);
            }


        }


        public static async Task Add(List<string> pkgs, bool Proxy = true)
        {
            EnsureInit();

            if (!checkMetaData())
            {
                return;
            }


            string gh_api;
            if (Proxy)
            {
                gh_api = GITHUB_API_PROXY;
            }
            else
            {
                gh_api = GITHUB_API;
            }

            var raw_metaData = File.ReadAllText(Path.Combine(METADATA_DIR, PLUGIN_METADATA_FILE));

            var metaData = JsonConvert.DeserializeObject<List<PluginMetaData>>(raw_metaData);

            var index = 0;

            foreach (var item in pkgs)
            {
                PluginMetaData temp = metaData.Find(t => t.name == item);

                if (temp == null)
                {
                    MsgHelper.E($"无法找到名称为 {temp.name} 的插件");
                    continue;
                }
                var realeaseUrl = $"{gh_api}/repos/{temp.github}/{temp.releases}";

                MsgHelper.I($"获取插件的发布信息...");

                var pluginInfo = await gh_api
                    .AppendPathSegment($"/repos/{temp.github}/{temp.releases}")
                    .GetJsonAsync<RealeaseInfo.Root>();

                var downLoadUrl = pluginInfo.assets[0].browser_download_url;

                if (Proxy)
                {
                    downLoadUrl = ProxyHelper.GetRawProxy(downLoadUrl);
                }

                var filep = Path.GetFileName(downLoadUrl);

                AnsiConsole.Markup(Markup.Escape($"[{index}/{pkgs.Count}] 正在准备安装 {item} {pluginInfo.tag_name}\n"));



                var table = new Table();
                table.AddColumn(new TableColumn("更新日志").Centered());
                table.AddRow(Markup.Escape(pluginInfo.body));
                AnsiConsole.Write(table);


                new DownLoader().DownLoadWithProgressBar(downLoadUrl, Path.Combine(PLUGIN_DIR, filep));



                MsgHelper.I($"成功安装了插件 {item}");
                index += 1;
            }

        }

        public static async Task ListRepo()
        {


            EnsureInit();


            if (!EnvHandler.CheckMetaData(PLUGIN_METADATA_FILE))
            {
                return;
            }

            var raw_metaData = File.ReadAllText(Path.Combine(METADATA_DIR, PLUGIN_METADATA_FILE));

            var metaData = JsonConvert.DeserializeObject<List<PluginMetaData>>(raw_metaData);

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn(new TableColumn("插件名称").Centered());
            table.AddColumn(new TableColumn("标签").Centered());
            table.AddColumn(new TableColumn("描述").Centered());


            foreach (var item in metaData)
            {
                table.AddRow(item.name, List2Tags(item.tags), item.description);
                //, , 
            }
            // Render the table to the console
            AnsiConsole.Write(table);
        }

        public static async Task List()
        {
            EnsureInit();

            DirectoryInfo directoryInfo = new DirectoryInfo(PLUGIN_DIR);
            var plugins = directoryInfo.GetFiles();
            List<PluginInfoReader.PluginInfo> pluginInfos = new List<PluginInfoReader.PluginInfo>(); ;
            foreach (var item in plugins)
            {
                if (item.Extension.ToLower() == ".jar")
                {
                    var r = PluginInfoReader.Read(item.FullName);

                    pluginInfos.Add(r);

                }
            }

            var table = new Table();

            // Add some columns
            table.AddColumn(new TableColumn("名称").Centered());
            table.AddColumn(new TableColumn("已安装版本").Centered());
            table.AddColumn(new TableColumn("描述").Centered());


            foreach (var item in pluginInfos)
            {
                table.AddRow(item.name, item.version, item.description);

            }
            // Render the table to the console
            AnsiConsole.Write(table);
        }

        public static async Task Remove(List<string> pkgs)
        {
            EnsureInit();

            if (pkgs.Count == 0)
            {
                Environment.Exit(0);
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(PLUGIN_DIR);

            var plugins = directoryInfo.GetFiles();

            List<PluginInfoReader.PluginInfo> pluginInfos = new List<PluginInfoReader.PluginInfo>();

            MsgHelper.I($"读取已安装的插件...");

            foreach (var item in plugins)
            {
                if (item.Extension.ToLower() == ".jar")
                {
                    var r = PluginInfoReader.Read(item.FullName);

                    pluginInfos.Add(r);

                }
            }

            foreach (var item in pkgs)
            {
                PluginInfoReader.PluginInfo temp = pluginInfos.Find(t => t.name == item);

                if (temp == null)
                {
                    MsgHelper.E($"找不到目标插件: {temp.name}");
                    continue;
                }


                File.Delete(temp.localPath);

                MsgHelper.I($"成功移除了插件: {temp.name}");


            }

        }

    }
}
