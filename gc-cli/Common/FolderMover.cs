using System.IO;

namespace gc_cli.Common
{
    internal static class FolderMover
    {
        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        public static void MoveFolder(string origin, string target)
        {
            MsgHelper.I($"正在把 {origin} 移动到 {target}文件夹内");


            DirectoryInfo di = new DirectoryInfo(origin);

            var dirs = di.GetDirectories();
            foreach (var item in dirs)
            {
                var t = Path.Combine(target, item.Name);
                DirectoryInfo di2 = new DirectoryInfo(t);
                if (!item.Exists)
                {
                    //源文件不存在
                    return;
                }
                if (di2.Exists)
                {
                    //目标文件夹已存在
                    return;
                }
                item.MoveTo(t);
            }

        }
    }
}
