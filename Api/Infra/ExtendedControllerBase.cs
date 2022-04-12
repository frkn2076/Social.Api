using Microsoft.AspNetCore.Mvc;

namespace Api.Infra;

public abstract class ExtendedControllerBase : ControllerBase
{
    public virtual IActionResult HandleServiceResponse<T>(ServiceResponse<T> serviceResponse)
    {
        if (!serviceResponse.IsSuccessful)
        {
            return StatusCode(500, serviceResponse.Error);
        }

        return Ok(serviceResponse.Response);
    }
}
