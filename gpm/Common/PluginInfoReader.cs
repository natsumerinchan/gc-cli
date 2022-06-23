using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace gpm.Common
{
    public static class PluginInfoReader
    {
        public class PluginInfo
        {
                /// <summary>
                /// 
                /// </summary>
                public string name { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string description { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string version { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public List<string> author { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string mainClass { get; set; }

                public string localPath { get; set; }



        }


        public static PluginInfo Read(string target)
        {
            ZipFile zipFile = new ZipFile(target);
            ZipEntry zipEntry = zipFile.GetEntry("plugin.json");
            Stream stream = zipFile.GetInputStream(zipEntry);
            using (StreamReader sr = new StreamReader(stream))
            {
                string result = sr.ReadToEnd();

                sr.Close();
                var r = JsonConvert.DeserializeObject<PluginInfo>(result);
                r.localPath = target;

                zipFile.Close();

                return r;
            }
        }
    }
}
