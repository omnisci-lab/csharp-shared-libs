using khothemegiatot.WebApi.Attributes;
using khothemegiatot.WebApi.Enums;
using khothemegiatot.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace khothemegiatot.WebApi.CQRS.Controllers;

[ApiController]
[HandlerException]
public class ApiControllerBase : ControllerBase
{
    private readonly IMediator _mediator;

    public ApiControllerBase(IMediator mediator)
    {
        _mediator = mediator;
    }

    [NonAction]
    public async Task<IActionResult> GetObjectResult<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : ExecResult
    {
        TResponse response = await _mediator.Send(request, cancellationToken);
        switch (response.Status)
        {
            case ExecStatus.Success: return Ok(response);
            case ExecStatus.NotFound: return NotFound(response);
            case ExecStatus.AlreadyExists: return Conflict(response);
            case ExecStatus.Invalid:
            case ExecStatus.Failed: return BadRequest(response);
            default:
                return Ok(response);
        }
    }

    [NonAction]
    public async Task<IActionResult> GetFileResult<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : Models.FileResult
    {
        TResponse response = await _mediator.Send(request, cancellationToken);

        switch (response.Status)
        {
            case ExecStatus.Success: return File(response.Data!, response.ContentType!);
            case ExecStatus.NotFound: return NotFound(response);
            case ExecStatus.AlreadyExists: return Conflict(response);
            case ExecStatus.Invalid:
            case ExecStatus.Failed: return BadRequest(response);
            default:
                return File(response.Data!, response.ContentType!);
        }
    }

    [NonAction]
    public async Task<IActionResult> GetObjectResult(IRequest request, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(request, cancellationToken);
        return Ok(new ExecResult { Status = ExecStatus.Success });
    }
}