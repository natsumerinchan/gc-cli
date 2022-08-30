using System.Collections.Generic;

namespace gc_cli.DataTemplates
{
    public class PackageMetaData
    {
        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string author { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string installationPath { get; set; }
            /// <summary>
            /// 启用https功能所需的key文件
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> dependencies { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string file { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<string> uninstall { get; set; }
        }

    }
}
