using System;
using System.IO;

namespace gc_cli.ConstProps
{
    public static class Paths
    {
        public static string DATA_DIR = Path.Combine(Environment.CurrentDirectory, ".gcm");

        public static string GC_CFG_FILEPATH = Path.Combine(Environment.CurrentDirectory, "config.json");

        public static string PLUGIN_DIR = Path.Combine(Environment.CurrentDirectory, "plugins");

        public static string RESOURCE_DIR = Path.Combine(Environment.CurrentDirectory, "resources");

        public static string METADATA_DIR = Path.Combine(DATA_DIR, "metadata");

        public static string DOWNLOAD_CACHE_DIR = Path.Combine(DATA_DIR, "cache");


        public const string PLUGIN_METADATA_FILE = "plugin.json";

        public const string CORE_METADATA_FILE = "core.json";

        public const string PACKAGE_METADATA_FILE = "package.json";

        public const string RESOURCE_METADATA_FILE = "resource.json";



    }
}
