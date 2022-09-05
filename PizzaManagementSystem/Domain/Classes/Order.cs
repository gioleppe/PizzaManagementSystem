namespace PizzaManagementSystem.Domain.Classes;

public class Order
{
    public int OrderId { get; set; }
    public IEnumerable<OrderItem> Items { get; set; }
    public float TotalAmount { get; set; }
}

public class OrderItemDto
{
    public int Type { get; set; }
    public int Quantity { get; set; }
}

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int PizzaTypeId { get; set; }
    public int Quantity { get; set; }
}

public class OrderSummary
{
    public int OrderId { get; set; }
    public float TotalAmount { get; set; }
    public int PendingOrders { get; set; }
}