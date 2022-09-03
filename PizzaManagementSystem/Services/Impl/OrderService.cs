using Microsoft.EntityFrameworkCore;
using PizzaManagementSystem.Database;
using PizzaManagementSystem.Domain.Classes;
using PizzaManagementSystem.Domain.Enums;

namespace PizzaManagementSystem.Services.Impl;

public class OrderService : IOrderService
{
    private static readonly Dictionary<PizzaType, int> Prices = new()
    {
        { PizzaType.Margherita, 500 },
        { PizzaType.Ortolana, 600 },
        { PizzaType.Diavola, 650 },
        { PizzaType.Bufalina, 700 }
    };

    private static Queue<Order> Queue = new();
    private readonly PizzaContext _context;

    public OrderService(PizzaContext context)
    {
        _context = context;
    }

    public async Task<OrderSummary> CreateOrder(IEnumerable<OrderItemDto> orderItems)
    {
        var orderItemDtos = orderItems.ToList();

        var totalAmount = orderItemDtos.Select(x => Prices[x.Type] * x.Quantity).Sum();
        var totFloat = (float)totalAmount / 100;
        var items = orderItemDtos.Select(x => new OrderItem
        {
            Type = x.Type,
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