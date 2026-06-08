using Microsoft.AspNetCore.Mvc;

namespace MyHiep.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public object Get() => new { status = "ok", service = "MyHiep.Api" };
}
