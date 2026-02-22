using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyMirror.TeamPulse.Application.Features;

namespace MyMirror.TeamPulse.Api.Controllers;

[ApiController]
[Route("api/pulse")]
public class PulseController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] SubmitPulseCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return Ok(new { id });
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
    {
        var summary = await _mediator.Send(new GetPulseSummaryQuery(), ct);
        return Ok(summary);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(), ct);
        return Ok(categories);
    }
}
