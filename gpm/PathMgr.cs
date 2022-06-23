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



        public static string  metadataDir = Path.Combine(dataDir, "cache","metadata");
    }
}
