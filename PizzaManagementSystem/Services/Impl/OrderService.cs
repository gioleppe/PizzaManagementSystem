using Microsoft.EntityFrameworkCore;
using PizzaManagementSystem.Database;
using PizzaManagementSystem.Domain.Classes;

namespace PizzaManagementSystem.Services.Impl;

public class OrderService : IOrderService
{
    private readonly Dictionary<int, int> Prices;

    private readonly PizzaContext _context;

    public OrderService(PizzaContext context)
    {
        _context = context;
        Prices = new Dictionary<int, int>();
        var dynamicPrices = context.PizzaTypes.Select(x => new {x.Type, x.Price});
        foreach (var dynamicPrice in dynamicPrices)
        {
            Prices.Add(dynamicPrice.Type, dynamicPrice.Price);
        }
    }

    public async Task<OrderSummary> CreateOrder(IEnumerable<OrderItemDto> orderItems)
    {
        var orderItemDtos = orderItems.ToList();

        var totalAmount = orderItemDtos.Select(x => Prices[x.Type] * x.Quantity).Sum();
        var totFloat = (float)totalAmount / 100;
        var items = orderItemDtos.Select(x => new OrderItem
        {
            PizzaTypeId = x.Type,
            Quantity = x.Quantity
        }).ToList();

        var newOrder = new Order
        {
            Items = items,
            TotalAmount = totFloat
        };

        var pendingOrders = await _context.Orders.CountAsync();

        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();
        
        
        return new OrderSummary
        {
            OrderId = newOrder.OrderId,
            TotalAmount = totFloat,
            PendingOrders = pendingOrders
        };
    }

    public Task<Order?> GetNextOrder()
    {
        return _context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync();
    }

    public async Task<Order?> CloseOrder()
    {
        var order = await _context.Orders.FirstOrDefaultAsync();

        if (order is null)
            return order;

        _context.Remove(order);
        await _context.SaveChangesAsync();

        return order;
    }
}