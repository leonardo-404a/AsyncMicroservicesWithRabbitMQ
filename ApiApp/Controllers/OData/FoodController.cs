using MediatR;
using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Application.FoodModule.Query;

namespace PizzaPlace.WebApi.Controllers.OData;

[ApiController]
[Route("odata/food")]
public class FoodController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> RetrieveAll()
    {
        return Ok(await _mediator.Send(new RetrieveFoodsQuery()));
    }
}
