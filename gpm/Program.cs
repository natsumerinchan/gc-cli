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
            core,
            res,
        }

        static async Task<int> Main(string[] args)
        {

            var rootCommand = new RootCommand("Package Manager for GrassCutter");

            var ProxyOption = new Option<bool>(
                name: "-p",
                description: "启用请求代理.",
                getDefaultValue:()=>false
                );


            var InstallOpthon = new Option<InstallType>(
                name: "-t",
                description: "安装资源类型",
                getDefaultValue: () => InstallType.core

                ) ;

            var verOption = new Option<string>(
                name: "v",
                description: "指定安装的版本"
                );

            var initCommand = new Command("init", "已弃用")
            {

            };

            var addArgument = new Argument<List<string>>(name:"要添加的包名" )
            {
                Arity=ArgumentArity.OneOrMore
            };  //完成
            var removeArgument = new Argument<List<string>>(name: "要删除的包名")
            {
                Arity = ArgumentArity.OneOrMore
            };  //完成

            var addCommand = new Command("add", "添加插件");  //完成
            var updateCommand = new Command("update", "更新仓库信息");  //完成
            var listrepoCommand = new Command("listrepo", "列出仓库中的插件");  //完成
            var removeCommand = new Command("remove", "删除已安装的插件");  //完成
            var listCommand = new Command("list", "列出已安装的插件");  //完成
            var installCommand = new Command("install", "在文件夹下安装GrassCutter");  //完成
            var runCommand = new Command("run", "开启服务器");  //咕咕
            var checkCommand = new Command("check", "检查运行环境");  //咕咕
            var infoCommand = new Command("info", "列出所有信息");    //咕咕


            addCommand.AddArgument(addArgument);
            removeCommand.AddArgument(removeArgument);

            installCommand.AddOption(InstallOpthon);


            //proxy
            listrepoCommand.AddOption(ProxyOption);
            updateCommand.AddOption(ProxyOption);
            addCommand.AddOption(ProxyOption);
            installCommand.AddOption(ProxyOption);


            initCommand.SetHandler(() => 
            {
                EnvHandler.Init();
            });

            addCommand.SetHandler(async(pkgs,proxy)=> {await PluginHandler.Add(pkgs,proxy); }, addArgument,ProxyOption);
            removeCommand.SetHandler(async(pkgs)=> {await PluginHandler.Remove(pkgs); }, removeArgument);
            updateCommand.SetHandler(async (proxy) => {
                await PluginHandler.Update(proxy);
                await CoreHandler.Update(proxy);
                await ResHandler.Update(proxy);
            },ProxyOption);
            listrepoCommand.SetHandler(async()=> {await PluginHandler.ListRepo(); });
            listCommand.SetHandler(async()=> {await PluginHandler.List(); });
            installCommand.SetHandler((IType,ver,proxy) =>
            {
                    string sha = new Common.CoreVersionHelper.VersionInfo(ver).GetSha();

                    switch (IType)
                    {
                        case InstallType.res: ResHandler.Install(proxy); break;
                        case InstallType.core: CoreHandler.Install(sha, proxy); break;
                        default:
                            break;
                    }
                

                
            }, InstallOpthon,verOption,ProxyOption);

            rootCommand.AddCommand(initCommand);
            rootCommand.AddCommand(addCommand);
            rootCommand.AddCommand(updateCommand);
            rootCommand.AddCommand(listrepoCommand);
            rootCommand.AddCommand(removeCommand);
            rootCommand.AddCommand(listCommand);
            rootCommand.AddCommand(installCommand);


            return await rootCommand.InvokeAsync(args);

        }


        
    }
}
