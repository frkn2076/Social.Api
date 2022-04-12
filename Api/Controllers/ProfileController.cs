using Api.Data.Repositories.Contracts;
using Api.Infra;
using Api.Service.Contracts;
using Api.ViewModels.Request;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController : ExtendedControllerBase
{
    private readonly ILogger<ProfileController> _logger;
    private readonly ISocialRepository _socialRepository;
    private readonly IProfileService _profileService;
    private readonly CurrentUser _currentUser;

    public ProfileController(ILogger<ProfileController> logger, ISocialRepository socialRepository, CurrentUser currentUser,
        IProfileService profileService)
    {
        _logger = logger;
        _socialRepository = socialRepository;
        _currentUser = currentUser;
        _profileService = profileService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok($"{nameof(ProfileController)} works properly!");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _profileService.GetProfileAsync(id);
        return HandleServiceResponse(response);
    }

    [HttpGet("private")]
    public async Task<IActionResult> GetPrivate()
    {
        var id = _currentUser.GetId();
        var response = await _profileService.GetProfileAsync(id);
        return HandleServiceResponse(response);
    }

    [HttpPut("private")]
    public async Task<IActionResult> UpdatePrivate(ProfileDetailRequestModel request)
    {
        var id = _currentUser.GetId();
        var response = await _profileService.UpdateProfileAsync(id, request.Name, request.Surname, request.Photo);
        return HandleServiceResponse(response);
    }
}
