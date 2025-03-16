using MediatR;
using Microsoft.AspNetCore.Mvc;
using QBUtils.Data.Services;
using QBUtils.Data.Services.SQLConnection;
using QBUtils.Data.SQLConnection;
using System.Security.Claims;

namespace QBUtils.Web.Controllers;

public class SQLConnectionController : Controller
{
    private readonly IMediator _mediator;

    public SQLConnectionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> List(GetSQLConnectionsByUser request, CancellationToken cancellationToken)
    {
        request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var result = await _mediator.Send(request, cancellationToken);

        if (result.WasSuccessful)
        {
            return PartialView(result.Value);
        }

        return StatusCode(result.StatusCode);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSQLConnection request, CancellationToken cancellationToken)
    {
        request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var result = await _mediator.Send(request, cancellationToken);

        if (result.WasSuccessful)
        {
            Response.Headers.TryAdd("HX-Location", "/sqlconnection/");
            return Ok();
        }

        if (result.HasErrors)
        {
            foreach (var error in result.Errors!.OfType<NotFoundError>())
            {
                ModelState.Merge(error.ModelState);
            }
        }
        else
        {
            ModelState.TryAddModelError(string.Empty, "There was an unexpected error creating the SQLConnection.");
        }

        return PartialView();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteSQLConnection request, CancellationToken cancellationToken)
    {
        request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var result = await _mediator.Send(request, cancellationToken);

        if (result.WasSuccessful)
        {
            Response.Headers.TryAdd("HX-Location", "/sqlconnection/");
            return Ok();
        }

        return StatusCode(result.StatusCode);
    }

    [HttpGet]
    public async Task<IActionResult> Editable(GetSQLConnectionById request, CancellationToken cancellationToken)
    {
        request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var result = await _mediator.Send(request, cancellationToken);

        if (result.WasSuccessful)
        {
            return PartialView(result.Value);
        }

        return StatusCode(result.StatusCode);
    }

    [HttpGet]
    public async Task<IActionResult> Editing(GetSQLConnectionById request, CancellationToken cancellationToken)
    {
        request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var result = await _mediator.Send(request, cancellationToken);

        if (result.WasSuccessful)
        {
            return PartialView(result.Value);
        }

        return StatusCode(result.StatusCode);
    }

    [HttpPost]
    public async Task<IActionResult> Editing(UpdateSQLCommand request, CancellationToken cancellationToken)
    {
        request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var result = await _mediator.Send(request, cancellationToken);

        if (result.WasSuccessful)
        {
            return PartialView(nameof(Editable), result.Value);
        }

        if (result.HasErrors)
        {
            foreach (var error in result.Errors?.OfType<BadRequestError>() ?? [])
            {
                ModelState.Merge(error.ModelState);
            }
        }
        else
        {
            ModelState.TryAddModelError(string.Empty, "There was an unexpected error saving your changes.");
        }

        return PartialView();
    }
}
