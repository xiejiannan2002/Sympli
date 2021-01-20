using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Runtime.Caching;
using Microsoft.Extensions.Configuration;
using HtmlAgilityPack;

namespace Sympli.Business
{
    public class Engine
    {
        public string name { get; set; }
        public string parse { get; set; }
    }
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
    public class DataProcessing
    {
        private readonly string website;
        private readonly string[] keywords;
        private readonly Engine[] engines;
        private readonly int timeout;
        private readonly IConfiguration _config;
        public DataProcessing(IConfiguration config)
        {
            _config = config;
            website = config.GetValue<string>("SearchPara:website");
            keywords = config.GetSection("SearchPara:keywords").Get<string[]>();
            engines = config.GetSection("SearchPara:engines").Get<Engine[]>();
            timeout = config.GetValue<int>("SearchPara:timeout");
        }

        public string ConstructWebRequestUrl(string keyword, string engine)
        {
            return engine + "search?q=" + keyword;
        }

        public int FindPosition(HtmlDocument doc, string website, string rex)
        {
            try
            {
                var selectNodes = doc.DocumentNode.SelectNodes(rex);
                for (int i = 0; selectNodes != null && i < selectNodes.Count; i++)
                {
                    if (selectNodes[i].InnerHtml.Contains(website))
                        return i + 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            
        }

        public IEnumerable<SearchResult> SearchFromInternet()
        {
            IEnumerable<SearchResult> result = GetResultFromCache() as IEnumerable<SearchResult>;
            if (result != null)
                return result;
            else
                return GetResultFromInternet();
        }

        public IEnumerable<SearchResult> GetResultFromInternet()
        {
            List<SearchResult> result = new List<SearchResult>();
            for (int i = 0; i < this.keywords.Length; i++)
            {
                for (int j = 0; j < this.engines.Length; j++)
                {
                    try
                    {
                        string url = ConstructWebRequestUrl(keywords[i], engines[j].name);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.ASCII))
                            {
                                //string html = reader.ReadToEnd();
                                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                doc.Load(reader);
                                int rank = FindPosition(doc, this.website, this.engines[j].parse);
                                result.Add(new SearchResult(keywords[i], engines[j].name, rank));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            SaveResultToCache(result);
            return result;
        }

        public object GetResultFromCache()
        {
            var cache = MemoryCache.Default;
            if (cache.Contains("SearchingResult"))
            {
                return cache["SearchingResult"];
            }

            return null;
        }

        public void SaveResultToCache(List<SearchResult> result)
        {
            var cache = MemoryCache.Default;
            cache.Remove("SearchingResult");
            if (!cache.Contains("SearchingResult"))
            {
                var expiration = DateTimeOffset.UtcNow.AddMinutes(this.timeout);
                cache.Add("SearchingResult", result, expiration);
            }
        }
    }
}
