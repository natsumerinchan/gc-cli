using gpm.Common;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static gpm.Program;
using static gpm.PathMgr;
using gpm.DataTemplates;
using Newtonsoft.Json;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace gpm.Hanlder
{
    class ResHandler
    {
        const string REPO_URL = "https://raw.githubusercontent.com/gc-toolkit/GPM-Index/main/metadata/zh/gc-resource/index.json";

        private static void EnsureInit()
        {
            EnvHandler.Init(false);
        }

        private static bool checkMetaData()
        {
            if (!File.Exists(Path.Combine(metadataDir, RESOURCE_METADATA_FILE)))
            {
                MsgHelper.I("No metadata found in disk, please run [bold]gpm update[/] first");
                return false;
            }

            MsgHelper.I($"Reading cached metadata");
            return true;
        }

        private static List< ResMetaData.Root> ReadMetaData()
        {
            try
            {
                return JsonConvert
                    .DeserializeObject< List<ResMetaData.Root>>
                    (File.ReadAllText(Path.Combine(metadataDir, RESOURCE_METADATA_FILE)));
            }
            catch (Exception)
            {

                throw;
            }

        }

        private static void MoveFolder(string origin,string target)
        {
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
    

        private static void UnzipFile(string file,string folder)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(file));
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);
                if (directoryName != String.Empty)
                {
                    Directory.CreateDirectory(Path.Combine( folder ,directoryName));
                }
                if (fileName != String.Empty)
                {
                    FileStream streamWriter = File.Create(Path.Combine(folder ,theEntry.Name));
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
        }

        public static async Task Install(bool proxy = false)
        {

            EnsureInit();

            if (!checkMetaData())
            {
                return;
            }
            var metadata = ReadMetaData();
            ResMetaData.Root selMetadata = new ResMetaData.Root(); ;
            string selrepo = "";
            if (metadata.Count>1)
            {
                var selList = from r in metadata select r.repo;


                selrepo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select the repo you want to install.")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more items)[/]")
                    .AddChoices(
                        selList.ToArray<string>()
                    ));


                AnsiConsole.WriteLine($"You selected {selrepo}");

                selMetadata = (metadata.Where(r => r.repo == selrepo)).FirstOrDefault();
            }
            else
            {
                selMetadata = metadata.FirstOrDefault();
            }

            // Echo the fruit back to the terminal

            var downLoadUrl = selMetadata.archive.url;

            var originPath = selMetadata.archive.path.target;


            var filep = Path.GetFileName(downLoadUrl);


            AnsiConsole.Status()
                .Start("Downloading file...", ctx =>
                {
                    //FileDownLoader.DownloadFileData(downLoadUrl, Path.Combine(resourceDir, filep), false);

                    ctx.Status = "Unpacking data...";
                    //UnzipFile(Path.Combine(resourceDir, filep), resourceDir);

                    ctx.Status = "Moving file...";
                    MoveFolder(Path.Combine(resourceDir, originPath), resourceDir);




                    ctx.Status = "Removing unused files";

                    File.Delete(Path.Combine(resourceDir, filep));

                    

                });

            MsgHelper.I($"Successfully installed Resources ");

        }

        public static async Task Update(bool Proxy = false)
        {
            EnsureInit();


            MsgHelper.I($"Fetching metadata from {REPO_URL}");

            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    // Define tasks
                    var task1 = ctx.AddTask("[green]DownLoading[/]");


                    await FileDownLoader.DownloadFileData(REPO_URL, Path.Combine(metadataDir, RESOURCE_METADATA_FILE), delegate (int a) {
                        task1.Increment(a - task1.Percentage);
                    }, Proxy);

                });

            MsgHelper.I($"Successfully updated resource metadata");
        }
    }
}
