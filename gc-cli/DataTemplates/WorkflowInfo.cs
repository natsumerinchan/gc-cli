using System.Collections.Generic;

namespace gpm.DataTemplates
{
    class WorkflowInfo
    {
        public class Workflow_run
        {
            /// <summary>
            /// 
            /// </summary>
            public long id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long repository_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long head_repository_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string head_branch { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string head_sha { get; set; }
        }

        public class ArtifactsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public long id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string node_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long size_in_bytes { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string archive_download_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string expired { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string created_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string updated_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string expires_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Workflow_run workflow_run { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public int total_count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ArtifactsItem> artifacts { get; set; }
        }

    }
}
