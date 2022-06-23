using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static gpm.Program;
using static gpm.PathMgr;
namespace gpm.Hanlder
{
    public static class EnvHandler
    {

        public static void Init(bool showDetail=true)
        {

            if (File.Exists(cfgDir))
            {
                MsgHelper.I("Found config.json");

                if (!Directory.Exists(dataDir))
                {
                    MsgHelper.W("Data dir not exist,Creating...");


                    Directory.CreateDirectory(dataDir);


                    MsgHelper.I("Inited successfully");

                }
                else
                {
                    if (showDetail)
                    {
                        MsgHelper.I("Already Inited");

                    }


                }


            }
            else
            {
                MsgHelper.E("此路径不是一个有效的gc目录,请运行 gcm install 来安装一个core！");
                //throw new Exception("此目录不是一个gc目录。");
                Environment.Exit(-1);
            }


        }
    }
}
