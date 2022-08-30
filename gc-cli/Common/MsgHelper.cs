using Spectre.Console;

namespace gc_cli.Common
{
    public static class MsgHelper
    {
        public static string type = "";

        public static void E(string msg)
        {
            type = "[bold red][[错误]] [/]";
            Show(msg);

        }
        public static void I(string msg)
        {
            type = "[bold green][[信息]] [/]";
            Show(msg);

        }
        public static void W(string msg)
        {
            type = "[bold yellow][[警告]] [/]";
            Show(msg);

        }
        public static void D(string msg)
        {
            type = "[bold gray][[调试]] [/]";
            Show(msg);

        }

        private static void Show(string str)
        {
            AnsiConsole.MarkupLine(type + str);
        }
    }

}
