using Internship.Api.Models;

namespace Internship.Api.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderAsync(int id, Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<decimal> GetTotalRevenueAsync();
        Task<IEnumerable<object>> GetOrderStatisticsAsync();
    }
}
