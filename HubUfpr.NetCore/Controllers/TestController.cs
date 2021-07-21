using EasyMemoryCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HubUfpr.API.Controllers
{
    [Route("produtos")]
    public class ProdutosController : Controller
    {
        private readonly ICaching _caching;

        public ProdutosController(ICaching caching)
        {
            _caching = caching;
        }

        [Authorize("Bearer")]
        [HttpGet]
        public IActionResult Get()
        {
            var CacheKeyName = "dictionary";

            var dict = _caching.GetOrSetObjectFromCache(CacheKeyName, 20, GenerateDictionary);

            _caching.Invalidate(CacheKeyName);
            return Ok(dict);
        }

        private Dictionary<string, string> GenerateDictionary()
        {
            return new Dictionary<string, string> { { "01", "Pastel" }, { "02", "Bolinho" } };
        }
    }
}