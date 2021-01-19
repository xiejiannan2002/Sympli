using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Sympli.Business;


namespace Sympli.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SearchController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IEnumerable<SearchResult> Get()
        {
            try
            {
                string website = _config.GetValue<string>("SearchPara:website");
                string[] keywords = _config.GetSection("SearchPara:keywords").Get<string[]>();
                string[] engines = _config.GetSection("SearchPara:engines").Get<string[]>();
                return Data.SearchFromInternet(website, keywords, engines);
            }
            catch(Exception ex)
            {
                return null;
            }

        }
    }
}
