namespace gpm.DataTemplates
{
    class ResMetaData
    {
        public class Path
        {
            /// <summary>
            /// 
            /// </summary>
            public string target { get; set; }
        }

        public class Archive
        {
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Path path { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string repo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Archive archive { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
        }

    }
}
