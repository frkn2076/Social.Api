using Api.Data.Contracts;
using Api.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ActivityController : ControllerBase
{
    private readonly ILogger<ActivityController> _logger;
    private readonly ISocialRepository _socialRepository;

    public ActivityController(ILogger<ActivityController> logger, ISocialRepository socialRepository)
    {
        _logger = logger;
        _socialRepository = socialRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _socialRepository.Create();
        return Ok("Works properly!");
    }
}
