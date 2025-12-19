using Backend.Models;

namespace Backend.Services;

public interface IOrderService
{
    Task<IResult> ProcessOrderAsync(OrderRequest order);
}