using ICSharpCode.SharpZipLib.Zip;
using Spectre.Console;
using System;
using System.IO;

namespace gc_cli.Common
{
    internal static class Unziper
    {
        /// <summary>
        /// 解压文件    
        /// </summary>
        /// <param name="file">文件位置</param>
        /// <param name="folder">目标位置</param>
        /// <param name="remove">解压完成是否删除</param>
        public static void UnzipFile(string file, string folder, bool remove = false)
        {
            MsgHelper.I($"正在把 {file} 解压到 {folder}文件夹内");
            AnsiConsole.Status()
                .Start("解压中...", ctx =>
                {
                    #region 文件解压
                    ZipInputStream s = new ZipInputStream(File.OpenRead(file));
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);
                        if (directoryName != String.Empty)
                        {
                            Directory.CreateDirectory(Path.Combine(folder, directoryName));
                        }
                        if (fileName != String.Empty)
                        {
                            FileStream streamWriter = File.Create(Path.Combine(folder, theEntry.Name));
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            streamWriter.Close();
                        }
                    }
                    s.Close();
                    #endregion
                });

            MsgHelper.I($"解压完成!");

        }
    }
}
