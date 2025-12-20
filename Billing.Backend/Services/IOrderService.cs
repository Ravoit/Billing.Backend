using Billing.Backend.Models;

namespace Billing.Backend.Services;

public interface IOrderService
{
    Task<IResult> ProcessOrderAsync(OrderRequest order);
}