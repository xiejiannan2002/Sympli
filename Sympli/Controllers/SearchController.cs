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
                DataProcessing dtProcessor = new DataProcessing(_config);
                return dtProcessor.SearchFromInternet();
            }
            catch(Exception ex)
            {
                return new List<SearchResult>();
            }

        }
    }
}
