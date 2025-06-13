using Microsoft.AspNetCore.Mvc;
using OmniSciLab.WebApi.Attributes;
using OmniSciLab.WebApi.Enums;
using OmniSciLab.WebApi.Models;

namespace OmniSciLab.WebApi.Controllers;

[ApiController]
[HandlerException]
public class ApiControlllerBase : ControllerBase
{
    public ApiControlllerBase()
    {

    }

    [NonAction]
    protected IActionResult RetResult<TResult>(TResult result) where TResult : ExecResult 
    {
        if (result.Status == ExecStatus.Success)
            return Ok(result);         
        else if (result.Status == ExecStatus.NotFound)
            return NotFound(result);
        else if(result.Status == ExecStatus.AlreadyExists)
            return Conflict(result);
        else if (result.Status == ExecStatus.Invalid)
            return BadRequest(result);
        else 
            return StatusCode(500, result);
    }
}
