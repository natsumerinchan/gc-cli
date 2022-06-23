using System;
using System.Collections.Generic;
using System.Text;

namespace gpm.DataTemplates
{
    public class RealeaseInfo
    {
        public class Author
        {
            /// <summary>
            /// 
            /// </summary>
            public string login { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string node_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string avatar_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string gravatar_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string html_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string followers_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string following_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string gists_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string starred_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string subscriptions_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string organizations_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string repos_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string events_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string received_events_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string site_admin { get; set; }
        }

        public class Uploader
        {
            /// <summary>
            /// 
            /// </summary>
            public string login { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string node_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string avatar_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string gravatar_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string html_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string followers_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string following_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string gists_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string starred_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string subscriptions_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string organizations_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string repos_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string events_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string received_events_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string site_admin { get; set; }
        }

        public class AssetsItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
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
            public string label { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Uploader uploader { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string content_type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string state { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int size { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int download_count { get; set; }
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
            public string browser_download_url { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string assets_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string upload_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string html_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Author author { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string node_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tag_name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string target_commitish { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string draft { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string prerelease { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string created_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string published_at { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<AssetsItem> assets { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string tarball_url { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string zipball_url { get; set; }
            /// <summary>
            /// New auth key protocal.
            ///* Get rid of sessionKey! Much more secure.
            ///* Support get auth key for another player in game.
            ///* Support get auth key for another player via http api request.
            ///* Now mail&auth key expires in a short time(default 1 hour), and make it configurable as well! see `mojoconfig.json`
            ///
            ///-----
            ///
            ///全新认证协议
            ///* 不再使用sessionKey作为认证方式，更加安全。
            ///* 支持在游戏内为另一角色获取认证密钥。
            ///* 支持外部通过api的形式，在游戏外为另一角色获取密钥。
            ///* 邮件和认证密钥会在一定时间内过期（默认为1小时，与邮件过期时间相同），这个时间可以在`mojoconfig.json`中设置。
            /// </summary>
            public string body { get; set; }
        }

    }
}
