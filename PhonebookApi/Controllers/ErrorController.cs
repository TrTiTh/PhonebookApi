using Microsoft.AspNetCore.Mvc;

namespace PhonebookApi.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [Route("error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Index()
    {
        return Problem();
    }
}
