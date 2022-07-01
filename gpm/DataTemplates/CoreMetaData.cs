using System;
using System.Collections.Generic;
using System.Text;

namespace gpm.DataTemplates
{
    class CoreMetaData
    {
        public class Workflow
        {
            /// <summary>
            /// 
            /// </summary>
            public string latest { get; set; }

            public string all { get; set; }
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
            public Workflow workflow { get; set; }
        }

    }
}
