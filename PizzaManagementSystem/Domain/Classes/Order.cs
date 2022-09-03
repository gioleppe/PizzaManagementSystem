using PizzaManagementSystem.Domain.Enums;

namespace PizzaManagementSystem.Domain.Classes;

public class Order
{
    public int OrderId { get; set; }
    public IEnumerable<OrderItem> Items { get; set; }
    public float TotalAmount { get; set; }
}

public class OrderItemDto
{
    public PizzaType Type { get; set; }
    public int Quantity { get; set; }
}

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public PizzaType Type { get; set; }
    public int Quantity { get; set; }
}

public class OrderSummary
{
    public int OrderId { get; set; }
    public float TotalAmount { get; set; }
    public int PendingOrders { get; set; }
}