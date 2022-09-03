using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PizzaManagementSystem.Database;
using PizzaManagementSystem.Domain.Classes;
using PizzaManagementSystem.Domain.Validators;
using PizzaManagementSystem.Services;
using PizzaManagementSystem.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.UseUrls(builder.Configuration["AspNetCoreUrls"]);
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// services
builder.Services.AddScoped<IOrderService, OrderService>();

// validators
builder.Services.AddScoped<IValidator<OrderItemDto>, OrderItemValidator>();

// dbcontext
builder.Services.AddDbContext<PizzaContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnectionString"));
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<PizzaContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();