using Microsoft.EntityFrameworkCore;
using PizzaManagementSystem.Domain.Classes;

namespace PizzaManagementSystem.Database;

public class PizzaContext : DbContext
{
    public PizzaContext(DbContextOptions<PizzaContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
}