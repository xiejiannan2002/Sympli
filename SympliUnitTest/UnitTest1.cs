using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Sympli.Business;
using System.Linq;
using System.Runtime.Caching;
using Microsoft.Extensions.Configuration;

namespace SympliUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IConfiguration _mockConfig;
        private readonly DataProcessing _dtProcessor;
        public UnitTest1()
        {
            _mockConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _dtProcessor = new DataProcessing(_mockConfig);
        }

        [TestMethod]
        public void TestConstructWebRequestUrl()
        {
            Assert.AreEqual(_dtProcessor.ConstructWebRequestUrl("e-settlements", "https://www.google.com.au/"), "https://www.google.com.au/search?q=e-settlements");
            Assert.AreEqual(_dtProcessor.ConstructWebRequestUrl("e-settlements", "https://www.bing.com/"), "https://www.bing.com/search?q=e-settlements");
        }

        [TestMethod]
        public void TestFindPosition()
        {
            string html = "<div class=\"BNeawe UPmit AP7Wnd\">www.sympli.com.au</div><div class=\"BNeawe UPmit AP7Wnd\">www.infotrack.com.au</div>";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            Assert.AreEqual(_dtProcessor.FindPosition(doc, "www.sympli.com.au", "//div[@class='BNeawe UPmit AP7Wnd']"), 1);
        }

        [TestMethod]
        public void TestGetResultFromInternet()
        {
            var result = _dtProcessor.GetResultFromInternet();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<SearchResult>));
            Assert.AreEqual(result.ToArray().Length, 4);         
        }

        [TestMethod]
        public void TestGetResultFromCache()
        {
            var cache = MemoryCache.Default;
            Assert.AreEqual(_dtProcessor.GetResultFromCache(), cache["SearchingResult"]);
        }

        [TestMethod]
        public void TestSaveResultToCache()
        {
            var cache = MemoryCache.Default;
            List<SearchResult> sr = new List<SearchResult>();
            sr.Add(new SearchResult("1", "2", 3));
            sr.Add(new SearchResult("1", "2", 3));
            sr.Add(new SearchResult("1", "2", 3));
            _dtProcessor.SaveResultToCache(sr);
            var result = cache["SearchingResult"] as IEnumerable<SearchResult>;
            Assert.AreEqual(result.Count(), 3);
        }
    }
}
