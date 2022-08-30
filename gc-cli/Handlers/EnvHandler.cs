using gc_cli.Common;
using System.IO;
using static gc_cli.ConstProps.Paths;
namespace gc_cli.Handlers
{
    class EnvHandler
    {
        public static void Init(bool showDetail = true)
        {
            if (!Directory.Exists(DATA_DIR))
            {
                MsgHelper.W("缓存数据文件夹不存在，尝试创建...");


                Directory.CreateDirectory(DATA_DIR);


                MsgHelper.I("成功创建了缓存数据文件夹");

            }
            else
            {
                if (showDetail)
                {
                    MsgHelper.I("数据文件夹已经存在！");

                }


            }



        }

        public static bool CheckMetaData(string file)
        {
            if (!File.Exists(Path.Combine(METADATA_DIR, file)))
            {
                MsgHelper.I("未在本地找到源信息，请先执行 [bold]gpm update[/] 命令");
                return false;
            }

            MsgHelper.I($"使用本地的源信息");
            return true;
        }

    }
}
