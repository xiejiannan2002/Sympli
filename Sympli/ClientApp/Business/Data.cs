using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Sympli.ClientApp.Business
{
    public static class Data
    {
        public static string ConstructWebRequestUrl(string keyword, string option)
        {
            return option + "search?q=" + keyword;
        }

        public static int FindPosition(string html, string website)
        {
            string lookup = "<div class=\"BNeawe UPmit AP7Wnd\">www.*</div></a></div><div class=\"x54gtf\">";
            MatchCollection matches = Regex.Matches(html, lookup);
            for (int i = 0; i < matches.Count; i++)
            {
                string match = matches[i].Groups[0].Value;
                if (match.Contains(website))
                    return i + 1;
            }
            return 0;
        }
  }
}
