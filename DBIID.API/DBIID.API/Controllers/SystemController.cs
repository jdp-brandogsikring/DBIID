using DBIID.Application;
using DBIID.Shared.Features.Applications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DBIID.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        CacheStorage _cacheStorage = CacheStorage.Instance;

        [HttpGet("{key}")]
        public IActionResult Get(string key)
        {
            var value = _cacheStorage.Get<ApplicationLoginDto>(key);
            if (value != null)
            {
                _cacheStorage.Remove(key);
                return Ok(value);
            }

            return BadRequest();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
