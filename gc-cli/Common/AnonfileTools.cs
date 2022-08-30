using HtmlAgilityPack;
using System.Threading.Tasks;
namespace gc_cli.Common;
using Flurl.Http;

public static class AnonfileTools
{
    public static async Task<string> GetDownloadUrl(string origin)
    {
        MsgHelper.I($"从{origin}获取下载地址..");
        string ret = "";
        var html = await origin.GetStringAsync();
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);
        HtmlNode node = doc.DocumentNode.SelectSingleNode("//*[@id=\"download-url\"]");
        // Console.WriteLine(node.InnerText);
        HtmlAttributeCollection attrs = node.Attributes;
        ret = attrs["href"].Value;
        return ret;
    }
}