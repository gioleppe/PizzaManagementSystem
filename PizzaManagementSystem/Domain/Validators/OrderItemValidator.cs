using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PizzaManagementSystem.Database;
using PizzaManagementSystem.Domain.Classes;

namespace PizzaManagementSystem.Domain.Validators;

public class OrderItemValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemValidator(PizzaContext context)
    {
        RuleFor(x => x.Type)
            .Must(dtoPizzaType => context.PizzaTypes.Any(pizzaType => pizzaType.Type == dtoPizzaType))
            .WithMessage($"no Pizza exists with the provided type");
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("It's not possible to order zero pizzas of a certain type!");
    }
}