using FluentValidation;
using PizzaManagementSystem.Domain.Classes;

namespace PizzaManagementSystem.Domain.Validators;

public class OrderItemValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemValidator()
    {
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}