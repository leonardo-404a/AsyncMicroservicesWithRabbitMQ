using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Application.OrderModule.Query;
using PizzaPlace.Domain.OrderModule.Enums;

namespace PizzaPlace.WebApi.Controllers.OData;

[ApiController]
[Route("odata/orders")]
public class OrderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Retrieve()
    {
        return Ok(await _mediator.Send(new RetrieveOrdersQuery()));
    }

    [HttpGet("by-status/{status}")]
    public async Task<IActionResult> RetrieveByStatus(OrderStatus status)
    {
        return Ok(await _mediator.Send(new RetrieveOrderByStatusQuery(status)));
    }

    [HttpGet("by-type/{type}")]
    public async Task<IActionResult> RetrieveByType(OrderType type)
    {
        return Ok(await _mediator.Send(new RetrieveOrderByTypeQuery(type)));
    }
}
