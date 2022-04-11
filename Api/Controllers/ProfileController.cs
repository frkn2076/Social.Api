using Api.Data.Entities;
using Api.Data.Repositories.Contracts;
using Api.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController : ControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly ISocialRepository _socialRepository;

    public ProfileController(ILogger<ProfileController> logger, ISocialRepository socialRepository)
    {
        _logger = logger;
        _socialRepository = socialRepository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok($"{nameof(ProfileController)} works properly!");
    }

    [HttpGet("dbtest")]
    public async Task<IActionResult> DbTest()
    {
        var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

        var response = new Profile()
        {
            UserName = "Furkan",
            Email = "furkan@gmail.com",
            Password = "12345"
        };

        var a = await _socialRepository.CreateProfileAsync(response);

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
