using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace gpm.Common
{
    public static class CoreVersionHelper
    {



        public static string GetShortSha(string o)
        {
            return o.Substring(0, 7);
        }
        public class VersionInfo
        {
            public VersionInfo(string ver)
            {
                origin = ver;
            }

            public string origin { get; set; }


            public string GetSha()
            {
                try
                {
                    return origin.Split('-')[2];

                }
                catch (Exception)
                {

                    return null;
                }
                
            }

        }

        public static VersionInfo Read(string target)
        {
            ZipFile zipFile = new ZipFile(target);
            ZipEntry zipEntry = zipFile.GetEntry("emu/grasscutter/BuildConfig.class");

            Regex regex = new Regex(@"VERSION[\s\S]*ConstantValue[^0-9\-.a-zA-Z]*([0-9.\-a-zA-Z]+)[\s\S]*GIT_HASH[^0-9a-f]*([0-9a-f]+)[\s\S]*Code");



            Stream stream = zipFile.GetInputStream(zipEntry);
            using (StreamReader sr = new StreamReader(stream))
            {
                string result = sr.ReadToEnd();

                sr.Close();

                zipFile.Close();
                if (regex.IsMatch(result))
                {
                    var ret= regex.Matches(result);


                    return new VersionInfo( ret[0].Value +"-"+ ret[1].Value);
                    
                }


                return null;
            }
        }
    }
}
