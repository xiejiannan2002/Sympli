using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Runtime.Caching;


namespace Sympli.Business
{
    public class SearchResult
    {
        public string keyword { get; set; }
        public string source { get; set; }
        public int rank { get; set; }

        public SearchResult(string key, string source, int rank)
        {
            this.keyword = key;
            this.source = source;
            this.rank = rank;
        }
    }
    public static class Data
    {
        public static string ConstructWebRequestUrl(string keyword, string engine)
        {
            return engine + "search?q=" + keyword;
        }

        public static int FindPosition(string html, string website)
        {
            string lookup = "<div class=\"BNeawe UPmit AP7Wnd\">www.*</div>";
            MatchCollection matches = Regex.Matches(html, lookup);
            for (int i = 0; i < matches.Count; i++)
            {
                string match = matches[i].Groups[0].Value;
                if (match.Contains(website))
                    return i + 1;
            }
            return 0;
        }

        public static IEnumerable<SearchResult> SearchFromInternet(string website, string[] keywords, string[] engines)
        {
            if (GetResultFromCache() != null)
                return GetResultFromCache() as IEnumerable<SearchResult>;

            SearchResult[] result = new SearchResult[keywords.Length * engines.Length];
            int index = 0;
            for (int i = 0; i < keywords.Length; i++)
            {
                for (int j = 0; j < engines.Length; j++)
                {
                    string url = ConstructWebRequestUrl(keywords[i], engines[j]);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.ASCII))
                        {
                            string html = reader.ReadToEnd();
                            //int rank = FindPosition(html, website);
                            int rank = 1;
                            result[index] = new SearchResult(keywords[i], engines[j], rank);
                            index++;
                        }
                    }
                }
            }
            SaveResultToCache(result);
            return result;
        }

        public static object GetResultFromCache()
        {
            var cache = MemoryCache.Default;
            if (cache.Contains("SearchingResult"))
            {
                return cache["SearchingResult"];
            }

            return null;
        }

        public static void SaveResultToCache(SearchResult[] result)
        {
            var cache = MemoryCache.Default;
            if (!cache.Contains("SearchingResult"))
            {
                var expiration = DateTimeOffset.UtcNow.AddMinutes(60);
                cache.Add("SearchingResult", result.ToList(), expiration);
            }
        }
    }
}
