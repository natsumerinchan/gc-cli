using gpm.Common;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static gpm.Program;
using static gpm.PathMgr;
using ICSharpCode.SharpZipLib.Zip;
using gpm.DataTemplates;
using Newtonsoft.Json;

namespace gpm.Hanlder
{
    class CoreHandler
    {
        const string repo = "https://raw.githubusercontent.com/gc-toolkit/GPM-Index/main/metadata/zh/gc-core/index.json";

        private static void EnsureInit()
        {
            EnvHandler.Init(false);
        }

        public static async Task Update(bool Proxy = false)
        {
            EnsureInit();

            MsgHelper.I($"Fetching metadata from {repo}");
            //var file = Path.GetFileName(repo);

            // Asynchronous
            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                {
                    // Define tasks
                    var task1 = ctx.AddTask("[green]DownLoading[/]");


                    await FileDownLoader.DownloadFileData(repo, Path.Combine(metadataDir, CORE_METADATA_FILE), delegate (int a) {
                        task1.Increment(a - task1.Percentage);
                    }, Proxy);

                });

            MsgHelper.I($"Successfully updated core metadata");
        }

        private static bool checkMetaData()
        {
            if (!File.Exists(Path.Combine(metadataDir, CORE_METADATA_FILE)))
            {
                MsgHelper.I("No metadata found in disk, please run [bold]gpm update[/] first");
                return false;
            }

            MsgHelper.I($"Reading cached metadata");
            return true;
        }

        private static CoreMetaData.Root ReadMetaData()
        {
            try
            {
                return JsonConvert
                    .DeserializeObject< CoreMetaData.Root>
                    (File.ReadAllText(Path.Combine(metadataDir, CORE_METADATA_FILE)));
            }
            catch (Exception)
            {

                throw;
            }

        }

        private static void UnzipFile(string file, string folder)
        {
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
        }

        public static async Task Install(bool proxy = false)
        {

            EnsureInit();

            if (!checkMetaData())
            {
                return;
            }
            var metadata = ReadMetaData();

            var downLoadUrl = metadata.workflow.latest;
            // Echo the fruit back to the terminal


            var filep = Path.GetFileName(downLoadUrl);

            //MsgHelper.I(Markup.Escape($"Installing Resource..."));


            AnsiConsole.Status()
                .Start("Downloading data...", ctx =>
                {

                    FileDownLoader.DownloadFileData(downLoadUrl, Path.Combine(Environment.CurrentDirectory, filep), false);

                    ctx.Status = "UnPacking data...";

                    UnzipFile(Path.Combine(Environment.CurrentDirectory, filep), Environment.CurrentDirectory);


                    MsgHelper.I($"Removing unused file...");

                    File.Delete(Path.Combine(Environment.CurrentDirectory, filep));
                });


            MsgHelper.I($"Successfully installed Core");

        }
    }
}
