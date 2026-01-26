using Microsoft.AspNetCore.Mvc;
using Internship.Api.Services;
using Internship.Api.Models;

namespace Internship.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdOrder = await _orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedOrder = await _orderService.UpdateOrderAsync(id, order);
            if (updatedOrder == null)
                return NotFound();

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser(int userId)
        {
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByStatus(string status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(orders);
        }

        [HttpGet("revenue")]
        public async Task<ActionResult<decimal>> GetTotalRevenue()
        {
            var revenue = await _orderService.GetTotalRevenueAsync();
            return Ok(new { TotalRevenue = revenue });
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<IEnumerable<object>>> GetOrderStatistics()
        {
            var statistics = await _orderService.GetOrderStatisticsAsync();
            return Ok(statistics);
        }
    }
}
