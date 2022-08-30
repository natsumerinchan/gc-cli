namespace gc_cli.Common
{
    internal static class ProxyHelper
    {
        const string RAW_PROXY = "https://ghproxy.com/";
        public static string GetRawProxy(string origin)
        {
            return $"{RAW_PROXY}{origin}";
        }
        public static string GetApiProxy(string origin)
        {
            return $"{RAW_PROXY}{origin}";
        }

    }
}
