using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Application.OrderModule.Commands;
using PizzaPlace.Infrastructure.Modules.BusinessModule;

namespace PizzaPlace.WebApi.Controllers.Api;

[ApiController]
[Route("api/orders")]
public class OrderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand order)
    {
        try
        {
            await _mediator.Send(order);

            return Ok(ResourceMessages.OrderCreatedSuccessfully);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusCommand order)
    {
        try
        {
            await _mediator.Send(order);

            return Ok(ResourceMessages.OrderUpdatedSuccessfully);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
