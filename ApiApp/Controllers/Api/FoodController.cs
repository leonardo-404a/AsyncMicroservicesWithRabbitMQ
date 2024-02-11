using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Application.FoodModule.Commands;
using PizzaPlace.Infrastructure.Modules.BusinessModule;

namespace PizzaPlace.WebApi.Controllers.Api;

[ApiController]
[Route("api/food")]
public class FoodController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFoodCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok(ResourceMessages.FoodCreatedSuccessfully);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateFoodCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Ok(ResourceMessages.FoodUpdatedSuccessfully);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
