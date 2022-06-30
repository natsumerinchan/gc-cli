using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace gpm
{
    public static class PathMgr
    {


        public static string  dataDir = Path.Combine(Environment.CurrentDirectory, ".gcm");
        public static string  cfgDir = Path.Combine(Environment.CurrentDirectory, "config.json");

        public static string  pluginDir = Path.Combine(Environment.CurrentDirectory, "plugins");

        public static string resourceDir= Path.Combine(Environment.CurrentDirectory, "resources");

        public static string  metadataDir = Path.Combine(dataDir, "cache","metadata");


        //file name
        public const string PLUGIN_METADATA_FILE= "plugin.json";
        public const string CORE_METADATA_FILE= "core.json";
        public const string PACKAGE_METADATA_FILE= "package.json";
        public const string RESOURCE_METADATA_FILE= "resource.json";



    }
}
