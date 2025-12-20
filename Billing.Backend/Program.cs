using Billing.Backend.Gateways;
using Billing.Backend.Gateways.Implementations;
using Billing.Backend.Models;
using Billing.Backend.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddOpenApi();

// Available payment gateways
builder.Services.AddSingleton<IPaymentGateway>(sp =>
    new BankPaymentGateway(sp.GetRequiredService<ILogger<BankPaymentGateway>>()));
builder.Services.AddSingleton<IPaymentGateway>(sp =>
    new PayPalPaymentGateway(sp.GetRequiredService<ILogger<PayPalPaymentGateway>>()));

builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

var ordersApi = app.MapGroup("/orders");

ordersApi.MapPost("/",
    async ([FromBody] OrderRequest order, [FromServices] IOrderService orderService) =>
    await orderService.ProcessOrderAsync(order));

app.Run();