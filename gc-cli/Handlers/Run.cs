using gc_cli.Common;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace gc_cli.Handlers
{
    internal class Run
    {
        public static void Start()
        {

            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
            var files = directoryInfo.GetFiles();

            List<string> jarfiles = new List<string>();

            foreach (var item in files)
            {
                if (item.Extension.ToLower() == ".jar")
                {
                    jarfiles.Add(item.Name);

                }
            }
            string targetFile = null;

            if (jarfiles.Count == 0)
            {
                MsgHelper.E("请至少安装一个核心文件!");
            }
            else if (jarfiles.Count == 1)
            {
                targetFile = jarfiles.First();
            }
            else
            {
                targetFile = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("你要启动哪一个服务端?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](按键盘的 ↑ ↓ 来选择一个核心文件)[/]")
                    .AddChoices(jarfiles.ToArray())
                    );

            }


            if (targetFile == null)
            {
                return;
            }

            Common.ConfigReader.RenderCfg();

            var Startrule = new Rule("[green]服务器正在启动[/]\n");
            AnsiConsole.Write(Startrule);
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                FileName = "java",
                Arguments = $"-jar {targetFile}",
                UseShellExecute = false,
                CreateNoWindow = false,
            };
            process.Start();
            process.WaitForExit();
            var Endrule = new Rule("[red]服务器已经退出[/]\n");
            AnsiConsole.Write(Endrule);
        }
    }
}
