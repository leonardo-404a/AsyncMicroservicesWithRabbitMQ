using ApiApp.Application.OrderModule;
using Microsoft.AspNetCore.Mvc;
using SharedLib;

namespace ApiApp.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController(IBusPublisher busPublisher) : ControllerBase
{
    private readonly IBusPublisher _busPublisher = busPublisher;

    [HttpPost]
    public IActionResult Post([FromBody] CreateOrderCommand order)
    {
        _busPublisher.Publish(new OrderCreatedEvent { Order = order });

        return Ok("Pedido recebido com sucesso!");
    }
}
