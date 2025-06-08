using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LogiTrack.Models;

[ApiController]
[Route("api/inventory")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly LogiTrackContext _context;
    public InventoryController(LogiTrackContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_context.InventoryItems.ToList());
    }

    [HttpPost]
    public IActionResult Add([FromBody] InventoryItem item)
    {
        _context.InventoryItems.Add(item);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetAll), new { id = item.ItemId }, item);
    }

    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.InventoryItems.Find(id);
        if (item == null) return NotFound();
        _context.InventoryItems.Remove(item);
        _context.SaveChanges();
        return NoContent();
    }
}
