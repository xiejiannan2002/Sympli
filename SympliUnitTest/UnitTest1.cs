using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Sympli.Business;
using System.Linq;
using System.Runtime.Caching;
namespace SympliUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestConstructWebRequestUrl()
        {
            Assert.AreEqual(Data.ConstructWebRequestUrl("e-settlements", "https://www.google.com.au/"), "https://www.google.com.au/search?q=e-settlements");
            Assert.AreEqual(Data.ConstructWebRequestUrl("e-settlements", "https://www.bing.com/"), "https://www.bing.com/search?q=e-settlements");
        }

        [TestMethod]
        public void TestFindPosition()
        {
            string html = "<div class=\"BNeawe UPmit AP7Wnd\">www.sympli.com.au</div><div class=\"BNeawe UPmit AP7Wnd\">www.infotrack.com.au</div>";
            Assert.AreEqual(Data.FindPosition(html, "www.sympli.com.au"), 1);
            Assert.AreEqual(Data.FindPosition(html, "www.smypil.com.au"), 0);
        }

        [TestMethod]
        public void TestSearchFromInternet()
        {
            string[] keywords = new string[] { "key1", "key2" };
            string[] engines = new string[] { "https://www.google.com.au/", "https://www.bing.com/" };
            string website = "123";
            Assert.AreEqual(Data.SearchFromInternet(website, keywords, engines).ToArray().Length, 4);
            
        }

        [TestMethod]
        public void TestGetResultFromCache()
        {
            var cache = MemoryCache.Default;
            Assert.AreEqual(Data.GetResultFromCache(), cache["SearchingResult"]);
        }

        [TestMethod]
        public void TestSaveResultToCache()
        {
            var cache = MemoryCache.Default;
            SearchResult[] sr = new SearchResult[4];
            sr[0] = new SearchResult("1", "2", 3);
            sr[1] = new SearchResult("1", "2", 3);
            sr[2] = new SearchResult("1", "2", 3);
            sr[3] = new SearchResult("1", "2", 3);
            Data.SaveResultToCache(sr);
            var result = cache["SearchingResult"] as IEnumerable<SearchResult>;
            Assert.AreEqual(result.Count(), 4);

        }
    }
}
