using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogiTrack.Models;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly LogiTrackContext _context;
    public OrderController(LogiTrackContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var orders = _context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .ToList();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var order = _context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .FirstOrDefault(o => o.OrderId == id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public IActionResult Add([FromBody] Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return CreatedAtAction(nameof(Get), new { id = order.OrderId }, order);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.OrderId == id);
        if (order == null) return NotFound();
        _context.Orders.Remove(order);
        _context.SaveChanges();
        return NoContent();
    }
}
