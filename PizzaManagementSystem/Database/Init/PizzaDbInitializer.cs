
using PizzaManagementSystem.Domain.Classes;

namespace PizzaManagementSystem.Database.Init;

public static class DbInitializer
{
    public static void Initialize(PizzaContext context)
    {

        if (context.PizzaTypes.Any())
        {
            return;
        }

        var pizzaTypes = new[]
        {
            new PizzaType { Type = 0, Name = "Margherita", Price = 500 },
            new PizzaType { Type = 5, Name = "Ortolana", Price = 600 },
            new PizzaType { Type = 10, Name = "Diavola", Price = 650 },
            new PizzaType { Type = 15, Name = "Bufalina", Price = 700 },
        };

        context.PizzaTypes.AddRange(pizzaTypes);
        context.SaveChanges();
            
    }
}