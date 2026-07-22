using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/version")]
    [ApiController]
    public class version2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("This is Version 2.0");
    }
}
