using PizzaManagementSystem.Domain.Classes;

namespace PizzaManagementSystem.Services;

public interface IOrderService
{
    Task<OrderSummary> CreateOrder(IEnumerable<OrderItemDto> orderItems);
    Task<Order?> GetNextOrder();
    Task<Order?> CloseOrder();
}