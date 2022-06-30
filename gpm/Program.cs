using gpm.Hanlder;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace gpm
{
    class Program
    {
        public static class MsgHelper
        {
            public static string type = "";

            public static void E(string msg)
            {
                type = "[bold red][[Error]] [/]";
                Show(msg);

            }
            public static void I(string msg)
            {
                type = "[bold green][[Info]] [/]";
                Show(msg);

            }
            public static void W(string msg)
            {
                type = "[bold yellow][[Warn]] [/]";
                Show(msg);

            }

            private static void Show(string str)
            {
                AnsiConsole.MarkupLine(type+str);
            }
        }


        enum InstallType
        {
            Core,
            Res,
        }

        static async Task<int> Main(string[] args)
        {

            var rootCommand = new RootCommand("Package Manager for GrassCutter");

            var ProxyOption = new Option<bool>(
                name: "-p",
                description: "Enable Proxy in GPM.");


            var InstallOpthon = new Option<InstallType>(
                name:"-t",
                description:"安装资源类型"
                );



            var initCommand = new Command("init", "已弃用")
            {

            };

            var addArgument = new Argument<List<string>>(name:"要添加的包名" )
            {
                Arity=ArgumentArity.OneOrMore
            };
            var removeArgument = new Argument<List<string>>(name: "要删除的包名")
            {
                Arity = ArgumentArity.OneOrMore
            };

            var addCommand = new Command("add", "添加插件");
            var updateCommand = new Command("update", "更新插件仓库信息");
            var listrepoCommand = new Command("listrepo", "列出仓库中的插件");
            var removeCommand = new Command("remove", "删除已安装的插件");
            var listCommand = new Command("list", "列出已安装的插件");
            var installCommand = new Command("install", "在文件夹下安装GrassCutter");
            var runCommand = new Command("run", "开启服务器");
            var checkCommand = new Command("check", "检查运行环境");
            var infoCommand = new Command("info", "列出所有信息");


            addCommand.AddArgument(addArgument);
            removeCommand.AddArgument(removeArgument);



            //proxy
            listrepoCommand.AddOption(ProxyOption);
            updateCommand.AddOption(ProxyOption);
            addCommand.AddOption(ProxyOption);



            initCommand.SetHandler(() => 
            {
                EnvHandler.Init();
            });

            addCommand.SetHandler(async(pkgs,proxy)=> {await PluginHandler.Add(pkgs,proxy); }, addArgument,ProxyOption);
            removeCommand.SetHandler(async(pkgs)=> {await PluginHandler.Remove(pkgs); }, removeArgument);
            updateCommand.SetHandler(async (proxy) => { await PluginHandler.Update(proxy); },ProxyOption);
            listrepoCommand.SetHandler(async()=> {await PluginHandler.ListRepo(); });
            listCommand.SetHandler(async()=> {await PluginHandler.List(); });


            rootCommand.AddCommand(initCommand);
            rootCommand.AddCommand(addCommand);
            rootCommand.AddCommand(updateCommand);
            rootCommand.AddCommand(listrepoCommand);
            rootCommand.AddCommand(removeCommand);
            rootCommand.AddCommand(listCommand);


            return await rootCommand.InvokeAsync(args);

        }


        
    }
}
