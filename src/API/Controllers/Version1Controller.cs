using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/version")]
    [ApiController]
    public class Version1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("This is Version 1.0");
    }
}
