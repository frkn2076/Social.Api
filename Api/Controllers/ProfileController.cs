using Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Works properly!");
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var response = new ProfileDetailViewModel()
        {
            Id = id,
            Name = "Furkan",
            Surname = "Öztürk",
            Image = "Image1"
        };

        return Ok(response);
    }


}
