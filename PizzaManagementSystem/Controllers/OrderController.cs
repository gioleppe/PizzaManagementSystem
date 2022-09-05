using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PizzaManagementSystem.Domain.Classes;
using PizzaManagementSystem.Services;

namespace PizzaManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IValidator<OrderItemDto> _orderItemValidator;
    private readonly IOrderService _orderService;

    public OrderController(ILogger<OrderController> logger, IOrderService orderService,
        IValidator<OrderItemDto> orderItemValidator)
    {
        _logger = logger;
        _orderService = orderService;
        _orderItemValidator = orderItemValidator;
    }

    /// <summary>
    ///     This endpoint lets you place a new order. You must provide an array of valid order items.
    ///     Pizza types available at launch are as follows:
    ///     Margherita = 0,
    ///     Ortolana = 5,
    ///     Diavola = 10,
    ///     Bufalina = 15.
    ///     Of course, you must provide a quantity greater than 0 for each order item
    /// </summary>
    /// <param name="orderItems">The items in this order</param>
    /// <returns>
    ///     A summary of the freshly created order, containing the computed Total for the order as well as the Order Id
    ///     and the quantity of orders pending before this one.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<OrderSummary>> Create([FromBody] IEnumerable<OrderItemDto> orderItems)
    {
        var items = orderItems as OrderItemDto[] ?? orderItems.ToArray();
        var validationResults = items.Select(x => _orderItemValidator.Validate(x)).ToList();
        if (validationResults.Any(x => !x.IsValid))
        {
            var errors = validationResults.SelectMany(x => x.Errors);
            return BadRequest(errors);
        }

        var summary = await _orderService.CreateOrder(items);
        _logger.LogInformation($"Created order: {summary.OrderId}, total: {summary.TotalAmount}");
        return StatusCode(201, summary);
    }

    /// <summary>
    /// This endpoint lets you retrieve the next order in line, along with its information
    /// </summary>
    /// <returns></returns>
    [HttpGet("Next")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Order>> GetNext()
    {
        var order = await _orderService.GetNextOrder();
        return order is null
            ? NotFound()
            : Ok(order);
    }

    /// <summary>
    /// This endpoint lets you close the next order
    /// </summary>
    /// <returns></returns>
    [HttpDelete("CloseNext")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> CompleteOrder()
    {
        var closedOrder = await _orderService.CloseOrder();
        return closedOrder is null
            ? BadRequest("No orders available!")
            : Ok($"Successfuly closed order {closedOrder.OrderId}");
    }
}