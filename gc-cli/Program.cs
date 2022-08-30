using gc_cli.Common;
using Spectre.Console;
using System.Collections.Generic;
using System.CommandLine;
using System.Threading.Tasks;

namespace gc_cli
{
    class Program
    {

        enum InstallType
        {
            core,
            res,
        }

        enum FileType
        {
            //plugin
            plu,
            //package
            pac,
        }
        static async Task<int> Main(string[] args)
        {
            if (args.Length == 0)
            {
                AnsiConsole.Write(
                    new FigletText("GC-CLI")
                        .LeftAligned()
                        .Color(Color.Blue));
            }



            #region 命令
            var rootCommand = new RootCommand("Package Manager for GrassCutter");
            var addpkgCommand = new Command("addpkg", "添加Package");  //完成
            var addpluCommand = new Command("addplu", "添加插件");  //完成
            var updateCommand = new Command("update", "更新仓库信息");  //完成
            var listpkgiCommand = new Command("listpkgi", "列出仓库中的插件");  //完成
            var listpluiCommand = new Command("listplui", "列出仓库中Package");  //完成
            var rmpkgCommand = new Command("rmpkg", "删除已安装的Pakcage");  //完成
            var rmpluCommand = new Command("rmplu", "删除已安装的Pakcage");  //完成
            var listCommand = new Command("list", "列出已安装的插件");  //完成
            var installCommand = new Command("install", "在文件夹下安装GrassCutter");  //完成
            var runCommand = new Command("run", "开启服务器");  //咕咕
            var checkCommand = new Command("check", "检查运行环境");  //咕咕
            var infoCommand = new Command("info", "列出所有信息");    //咕咕
            #endregion


            #region 选项
            var ProxyOption = new Option<bool>(
                name: "-p",
                description: "启用请求代理.",
                getDefaultValue: () => false
                );


            var InstallOpthon = new Option<InstallType>(
                name: "-t",
                description: "安装资源类型",
                getDefaultValue: () => InstallType.core

                );

            var verOption = new Option<string>(
                name: "-v",
                description: "指定安装的版本"
                );

            #endregion


            #region 参数
            var addArgument = new Argument<List<string>>(name: "要添加的包名")
            {
                Arity = ArgumentArity.OneOrMore
            };  //完成
            var removeArgument = new Argument<List<string>>(name: "要删除的包名")
            {
                Arity = ArgumentArity.OneOrMore
            };  //完成
            #endregion

            //string url = "https://nightly.link/Grasscutters/Grasscutter/workflows/build/development/Grasscutter.zip";
            //string filepath = @"D:\Grasscutter.zip";
            //new DownLoader().DownLoadWithProgressBar(url, filepath);



            updateCommand.AddOption(ProxyOption);

            updateCommand.SetHandler(async (proxy) =>
            {
                await Handlers.Core.Update(proxy);
                await Handlers.Plugin.Update(proxy);
                await Handlers.Resources.Update(proxy);
                await Handlers.Packages.Update(proxy);
                MsgHelper.I("源信息更新完成");
            }, ProxyOption);

            installCommand.AddOption(InstallOpthon);
            installCommand.AddOption(ProxyOption);
            installCommand.AddOption(verOption);
            installCommand.SetHandler(async (IType, ver, proxy) =>
            {

                switch (IType)
                {
                    case InstallType.res:
                        {


                            await Handlers.Resources.Install(ver, proxy);

                        }
                        break;
                    case InstallType.core:
                        {
                            string sha = new Common.CoreVersionHelper.VersionInfo(ver).GetSha();
                            await Handlers.Core.Install(sha, proxy);
                        }
                        break;
                    default:
                        break;
                }



            }, InstallOpthon, verOption, ProxyOption);


            listpkgiCommand.SetHandler(async () =>
            {
                //case FileType.plu: await Handlers.Plugin.ListRepo(); break;
                await Handlers.Packages.ListRepo();

            });
            listpluiCommand.SetHandler(async () =>
            {
                //case FileType.plu: await Handlers.Plugin.ListRepo(); break;
                await Handlers.Plugin.ListRepo();

            });

            listCommand.SetHandler(async () => { await Handlers.Plugin.List(); });

            addpkgCommand.AddArgument(addArgument);
            addpkgCommand.AddOption(ProxyOption);
            addpkgCommand.SetHandler(async (pkgs, proxy) =>
            {
                //await Handlers.Plugin.Add(pkgs,proxy); 
                await Handlers.Packages.Add(pkgs, proxy);

            }, addArgument, ProxyOption);
            addpluCommand.AddArgument(addArgument);
            addpluCommand.AddOption(ProxyOption);
            addpluCommand.SetHandler(async (pkgs, proxy) =>
            {
                //await Handlers.Plugin.Add(pkgs,proxy); 
                await Handlers.Plugin.Add(pkgs, proxy);

            }, addArgument, ProxyOption);

            rmpluCommand.AddArgument(removeArgument);
            rmpluCommand.SetHandler(async (pkgs) =>
            {
                await Handlers.Plugin.Remove(pkgs);
                //await Handlers.Packages.Remove(pkgs);


            }, removeArgument);
            rmpkgCommand.AddArgument(removeArgument);
            rmpkgCommand.SetHandler(async (pkgs) =>
            {
                await Handlers.Packages.Remove(pkgs);
                //await Handlers.Packages.Remove(pkgs);


            }, removeArgument);

            runCommand.SetHandler(() =>
            {
                Handlers.Run.Start();
            });

            rootCommand.AddCommand(addpkgCommand);
            rootCommand.AddCommand(addpluCommand);
            rootCommand.AddCommand(updateCommand);
            rootCommand.AddCommand(listpluiCommand);
            rootCommand.AddCommand(listpkgiCommand);
            rootCommand.AddCommand(rmpkgCommand);
            rootCommand.AddCommand(rmpluCommand);
            rootCommand.AddCommand(listCommand);
            rootCommand.AddCommand(installCommand);
            rootCommand.AddCommand(runCommand);

            return await rootCommand.InvokeAsync(args);
        }
    }
}
