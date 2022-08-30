using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.IO;


namespace gc_cli.Common
{
    internal class ConfigReader
    {
        private static string GetArrayStr(string[] s)
        {
            var ret = String.Concat(s);
            return ret;
        }

        public static DataTemplates.ServerConfig.Root Read()
        {
            var cfgObj = new DataTemplates.ServerConfig.Root();
            string cfg = string.Empty;
            if (!File.Exists(ConstProps.Paths.GC_CFG_FILEPATH))
            {
                MsgHelper.W("未找到服务器的配置文件.");
                return null;
            }
            else
            {
                cfg = File.ReadAllText(ConstProps.Paths.GC_CFG_FILEPATH);
                try
                {
                    cfgObj = JsonConvert.DeserializeObject<DataTemplates.ServerConfig.Root>(cfg);
                }
                catch (Exception ex)
                {

                    AnsiConsole.WriteException(ex);
                }

            }
            return cfgObj;
        }

        public static void RenderCfg()
        {
            var cfg = Read();

            if (cfg != null)
            {
                Table tb = new Table();

                tb.Border(TableBorder.Rounded);

                tb.AddColumn("配置信息");

                var pn1 = $"语言:{cfg.language.language}    " +
                    $"运行模式:{cfg.server.runMode}    " +
                    $"调试等级:{cfg.server.debugLevel}";

                var pn2 = $"游戏服务器:{cfg.server.game.accessAddress}:{cfg.server.game.bindPort}    \n" +
                    $"数据库:{cfg.databaseInfo.game.connectionUri}";

                var pn3 = $"Http服务器:" +
                    $"{cfg.server.http.accessAddress}:{cfg.server.http.bindPort}    \n" +
                    $"CORS:{cfg.server.http.policies.cors.enabled}    " +
                    $"Allowed Origins:{GetArrayStr(cfg.server.http.policies.cors.allowedOrigins.ToArray())}";
                var pn4 = $"自动创建账户:{cfg.account.autoCreate}    \n" +
                    $"默认权限:{GetArrayStr(cfg.account.defaultPermissions.ToArray())}";

                tb.AddRow(pn1);

                tb.AddRow(pn2);

                tb.AddRow(pn3);

                tb.AddRow(pn4);

                AnsiConsole.Write(tb);



            }
        }
    }
}
