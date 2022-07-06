using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static gpm.Program;
using static gpm.PathMgr;
using static gpm.ApiMgr;
using gpm.Common;
using System.IO;
using Newtonsoft.Json;
using gpm.DataTemplates;
using Flurl;
using Flurl.Http;

namespace gpm.Hanlder
{
    


    static class PluginHandler
    {

        public const string REPO_URL = "https://raw.githubusercontent.com/gc-toolkit/GPM-Index/main/metadata/zh/gc-plugin/index.json";

        public const string PROXY_URL = "https://ghproxy.com/";


        private static string List2Tags(List<string> tags)
        {
            var r = "";
            foreach (var item in tags)
            {
                r += $"[invert aqua]{item}[/] ";
            }
            return r;
        }

        private static void EnsureInit()
        {
            EnvHandler.Init(false);
        }
        
        public static string GetProxyString(string origin)
        {

            return $"{PROXY_URL}{origin}";
        }
        
        public static async Task Add(List<string> pkgs,bool Proxy=false)
        {
            EnsureInit();

            if (!File.Exists(Path.Combine(metadataDir, PLUGIN_METADATA_FILE)))
            {
                MsgHelper.I("No metadata found in disk, please run [bold]gpm update[/] first");
                return;
            }

            MsgHelper.I($"Reading cached metadata");


            var raw_metaData = File.ReadAllText(Path.Combine(metadataDir, PLUGIN_METADATA_FILE));

            var metaData = JsonConvert.DeserializeObject<List<PluginMetaData> >(raw_metaData);
            var index = 0;
            foreach (var item in pkgs)
            {
                PluginMetaData temp = metaData.Find(t => t.name == item);

                if (temp==null)
                {
                    MsgHelper.E($"Can't found a package named {temp.name}");
                    continue;
                }
                var realeaseUrl = $"{GITHUB_API}/repos/{temp.github}/{temp.releases}";

                MsgHelper.I($"Getting plugin realeaseinfo");

                //不需要代理
                var pluginInfo = await GITHUB_API
                    .AppendPathSegment($"/repos/{temp.github}/{temp.releases}")
                    .GetJsonAsync<RealeaseInfo.Root>();

                var downLoadUrl = pluginInfo.assets[0].browser_download_url;
                var filep = Path.GetFileName(downLoadUrl);

                MsgHelper.I(Markup.Escape($"[{index}/{pkgs.Count}] Installing {item} {pluginInfo.tag_name}"));



                var rule = new Rule("[green]更新日志[/]");

                AnsiConsole.Write(rule);

                AnsiConsole.WriteLine(Markup.Escape(pluginInfo.body));



                // Asynchronous
                await AnsiConsole.Progress()
                    .StartAsync(async ctx =>
                    {
                        var task1 = ctx.AddTask("[green]DownLoading[/]");


                        await FileDownLoader.DownloadFileData(downLoadUrl, Path.Combine(pluginDir, filep), delegate (int a) {
                            task1.Increment(a - task1.Percentage);
                        },Proxy);

                    });


                MsgHelper.I($"Successfully installed {item}");
                index += 1;
            }

        }

        public static async Task Remove(List<string> pkgs)
        {
            EnsureInit();
            DirectoryInfo directoryInfo = new DirectoryInfo(pluginDir);
            var plugins = directoryInfo.GetFiles();
            List<PluginInfoReader.PluginInfo> pluginInfos = new List<PluginInfoReader.PluginInfo>();
            MsgHelper.I($"Reading installed plugins");

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
                    MsgHelper.E($"Can't found a plugin named {temp.name}");
                    continue;
                }


                File.Delete(temp.localPath);

                MsgHelper.I($"Successfully removed {temp.name}");


            }

        }

        public static async Task Update(bool Proxy = false)
        {
            EnsureInit();

            MsgHelper.I($"Fetching metadata from {REPO_URL}");
            //var file = Path.GetFileName(repo);

            // Asynchronous
            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    // Define tasks
                    var task1 = ctx.AddTask("[green]DownLoading[/]");


                    await FileDownLoader.DownloadFileData(REPO_URL, Path.Combine(metadataDir, PLUGIN_METADATA_FILE), delegate (int a) {
                        task1.Increment(a - task1.Percentage);
                    },Proxy);

                });

            MsgHelper.I($"Successfully updated plugin metadata");



        }

        public static async Task ListRepo()
        {


            EnsureInit();



            
            if (!File.Exists(Path.Combine(metadataDir, PLUGIN_METADATA_FILE)))
            {
                MsgHelper.E("No metadata found in disk, please run [bold]gpm update[/] first");
                return;
            }
            var raw_metaData = File.ReadAllText(Path.Combine(metadataDir, PLUGIN_METADATA_FILE));

            var metaData = JsonConvert.DeserializeObject<List<PluginMetaData>>(raw_metaData);

            // Create a table
            var table = new Table();

            // Add some columns
            table.AddColumn(new TableColumn("name").Centered());
            table.AddColumn(new TableColumn("tags").Centered());
            table.AddColumn(new TableColumn("description").Centered());


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


            DirectoryInfo directoryInfo = new DirectoryInfo(pluginDir);
            var plugins = directoryInfo.GetFiles();
            List<PluginInfoReader.PluginInfo> pluginInfos= new List<PluginInfoReader.PluginInfo>(); ;
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
            table.AddColumn(new TableColumn("name").Centered());
            table.AddColumn(new TableColumn("version").Centered());
            table.AddColumn(new TableColumn("description").Centered());


            foreach (var item in pluginInfos)
            {
                table.AddRow(item.name, item.version, item.description);
                
            }
            // Render the table to the console
            AnsiConsole.Write(table);
        }

    }
}
